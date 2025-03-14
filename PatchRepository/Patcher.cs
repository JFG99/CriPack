using CriPakInterfaces.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CriPakRepository.Helpers;
using CriPakInterfaces;
using CriPakRepository;
using CriPakInterfaces.Models.Components;
using CriPakInterfaces.Models.Components.Enums;
using System.Net;

namespace PatchRepository
{
    public class Patcher
    {
        public void Patch(CriPak package, string cpkDir, Dictionary<string, string> fileList)
        {
            var oldFile = new EndianReader<FileStream, EndianData>(System.IO.File.Open(package.FilePath, FileMode.Open, FileAccess.Read, FileShare.Read), new EndianData(true));
            var newCPK = new EndianWriter<FileStream, EndianData>(System.IO.File.OpenWrite(cpkDir), new EndianData(true));
            var patchList = package.ViewList.Where(x => fileList.Keys.Any(y => x.FileName.ToLower().Equals(y.ToLower()))).OrderBy(x => x.Offset).ToList();
            var firstPatchOffset = patchList.First().Offset;
            var unpatchedList = package.ViewList;
            newCPK.CopyFrom(oldFile.BaseStream, firstPatchOffset);
            var modifiedInNewArchive = new List<PatchList>(); 
            var currentIndex = unpatchedList.ToList().IndexOf(patchList[0]);
            var nextInOld = unpatchedList[currentIndex + 1];
            var resetPositionAfterPatch = false;
            foreach (var file in patchList)
            {
                while (nextInOld.Id < file.Id)
                {
                    nextInOld = unpatchedList[currentIndex];
                    if (resetPositionAfterPatch)
                    {
                        oldFile.BaseStream.Position = nextInOld.Offset;
                        resetPositionAfterPatch = false;
                    }
                    // Have to subtract the length of the archive header for all file changes, but not the other table headers.
                    modifiedInNewArchive.Add(CreateEntry(nextInOld, newCPK.BaseStream.Position - 2048, currentIndex));
                    newCPK.CopyFrom(oldFile.BaseStream, Convert.ToInt64(nextInOld.ArchiveLength));
                    newCPK.PadEndOfFile();
                    nextInOld = unpatchedList[++currentIndex];
                    oldFile.BaseStream.Position = nextInOld.Offset;
                }
                                
                var patchStream = new EndianReader<FileStream, EndianData>(System.IO.File.Open(fileList[file.FileName.ToLower()], FileMode.Open, FileAccess.Read, FileShare.Read), new EndianData(true));
                modifiedInNewArchive.Add(CreateEntry(file, newCPK.BaseStream.Position - 2048, currentIndex));
                //For now Im not going to deal with compression. Its slow and I don't think fully accurate.
                //if (file.Percentage < 100)
                //{
                //    var bytes = System.IO.File.ReadAllBytes(fileList[file.FileName.ToLower()]);
                //    var compressedFile = bytes;
                //    //compressedFile = bytes.CompressCRILAYLA();
                //    newFile.LengthDifference = (ulong)compressedFile.Length - file.ArchiveLength;
                //    newFile.ArchiveLength = (ulong)compressedFile.Length;
                //    newFile.ExtractedLength = (uint)bytes.Length;
                //    var memStream = new EndianReader<MemoryStream, EndianData>(new MemoryStream(compressedFile), new EndianData(true));
                //    memStream.CopyStream(newCPK.BaseStream, memStream.BaseStream.Length);
                //}
                //else
                //{
                modifiedInNewArchive.Last().LengthDifference = (ulong)patchStream.BaseStream.Length - file.ArchiveLength;
                modifiedInNewArchive.Last().ArchiveLength = (ulong)patchStream.BaseStream.Length;
                modifiedInNewArchive.Last().ExtractedLength = (uint)patchStream.BaseStream.Length;
                modifiedInNewArchive.Last().IsPatched = true;
                newCPK.CopyFrom(patchStream.BaseStream, patchStream.BaseStream.Length);
                newCPK.PadEndOfFile();
                //}
                resetPositionAfterPatch = true;
                currentIndex = unpatchedList.ToList().IndexOf(nextInOld);
                nextInOld = unpatchedList[++currentIndex];
                oldFile.BaseStream.Position = nextInOld.Offset;
            }
            var lastFile = unpatchedList.Where(x => x.Id != 0).Last();
            while (nextInOld.Id != 0 && nextInOld.Id <= lastFile.Id)
            {                
                modifiedInNewArchive.Add(CreateEntry(nextInOld, newCPK.BaseStream.Position - 2048, currentIndex));
                newCPK.CopyFrom(oldFile.BaseStream, Convert.ToInt64(nextInOld.ArchiveLength));
                newCPK.PadEndOfFile();
                nextInOld = unpatchedList[++currentIndex];
                oldFile.BaseStream.Position = nextInOld.Offset;
            }

            //This captures the ETOC table
            if (oldFile.BaseStream.Position <= nextInOld.Offset)
            {
                newCPK.CopyFrom(oldFile.BaseStream, nextInOld.Offset - oldFile.BaseStream.Position); 
                newCPK.PadEndOfFile();
            }
            modifiedInNewArchive.Add(CreateEntry(nextInOld, newCPK.BaseStream.Position, currentIndex));
            newCPK.CopyFrom(oldFile.BaseStream, oldFile.BaseStream.Length - nextInOld.Offset);
            oldFile.Close();
            newCPK.UpdateSections(package, modifiedInNewArchive);
            newCPK.Close();
        }       

        private PatchList CreateEntry(IFileViewer oldFile, long offset, int index)
        {
            var newFile = new PatchList();
            newFile.Id = oldFile.Id;
            newFile.Offset = offset;  
            newFile.FileName = oldFile.FileName;
            newFile.Type = oldFile.Type;
            newFile.ArchiveLength = oldFile.ArchiveLength;
            newFile.ExtractedLength = oldFile.ExtractedLength;
            newFile.IndexInArchive = index;
            return newFile;
        }        

    }
}
