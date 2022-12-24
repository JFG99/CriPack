using CriPakInterfaces.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CriPakRepository.Helpers;
using CriPakInterfaces;
using CriPakRepository.Repository;
using CriPakInterfaces.Models.Components;

namespace PatchRepository
{
    public class Patcher
    {
        //TODO:
        //      Fix Header offsets and sizes.

        public void Patch(CriPak package, string cpkDir, Dictionary<string, string> fileList)
        {
            var oldFile = new EndianReader<FileStream, EndianData>(System.IO.File.Open(package.FilePath, FileMode.Open, FileAccess.Read, FileShare.Read), new EndianData(true));
            var newCPK = new BinaryWriter(System.IO.File.OpenWrite(cpkDir));
            var patchList = package.ViewList.Where(x => fileList.Keys.Any(y => x.FileName.ToLower().Equals(y.ToLower()))).OrderBy(x => x.Offset).ToList();
            var firstPatchOffset = patchList.First().Offset;
            var unpatchedList = package.ViewList;
            oldFile.CopyStream(newCPK.BaseStream, firstPatchOffset);
            var modifiedInNewArchive = new List<PatchList>(); 
            var currentIndex = unpatchedList.ToList().IndexOf(patchList[0]);
            var nextInOld = unpatchedList[currentIndex];
            foreach (var file in patchList)
            {
                while (nextInOld.Id < file.Id)
                {
                    modifiedInNewArchive.Add(CreateEntry(nextInOld, newCPK.BaseStream.Position, currentIndex)); 
                    oldFile.CopyStream(newCPK.BaseStream, Convert.ToInt64(nextInOld.ArchiveLength));
                    nextInOld = unpatchedList[++currentIndex + 1];
                }
                
                var patchStream = new EndianReader<FileStream, EndianData>(System.IO.File.Open(fileList[file.FileName.ToLower()], FileMode.Open, FileAccess.Read, FileShare.Read), new EndianData(true));
                
                var newFile = CreateEntry(file, newCPK.BaseStream.Position, currentIndex);
                if (file.Percentage < 100)
                {
                    var bytes = System.IO.File.ReadAllBytes(fileList[file.FileName.ToLower()]);
                    var compressedFile = bytes;
                    compressedFile = bytes.CompressCRILAYLA();
                    newFile.LengthDifference = (ulong)compressedFile.Length - file.ArchiveLength;
                    newFile.ArchiveLength = (ulong)compressedFile.Length;
                    newFile.ExtractedLength = (uint)bytes.Length;
                    var memStream = new EndianReader<MemoryStream, EndianData>(new MemoryStream(compressedFile), new EndianData(true));
                    memStream.CopyStream(newCPK.BaseStream, memStream.BaseStream.Length);
                }
                else
                {
                    newFile.LengthDifference = (ulong)patchStream.BaseStream.Length - file.ArchiveLength;
                    newFile.ArchiveLength = (ulong)patchStream.BaseStream.Length;
                    newFile.ExtractedLength = (uint)patchStream.BaseStream.Length;
                    patchStream.CopyStream(newCPK.BaseStream, patchStream.BaseStream.Length);
                }
                newFile.IsPatched = true;
                modifiedInNewArchive.Add(newFile);
                currentIndex = unpatchedList.ToList().IndexOf(file);
                nextInOld = unpatchedList[currentIndex + 1];
            }
            var lastFile = unpatchedList.Where(x => x.Id != 0).Last();
            while (nextInOld.Id != 0 && nextInOld.Id <= lastFile.Id)
            {
                modifiedInNewArchive.Add(CreateEntry(nextInOld, newCPK.BaseStream.Position, currentIndex));
                oldFile.CopyStream(newCPK.BaseStream, Convert.ToInt64(nextInOld.ArchiveLength));
                nextInOld = unpatchedList[++currentIndex + 1];
            }
            oldFile.BaseStream.Position = nextInOld.Offset;
            //This captures the ETOC table
            modifiedInNewArchive.Add(CreateEntry(nextInOld, newCPK.BaseStream.Position, currentIndex));
            oldFile.CopyStream(newCPK.BaseStream, oldFile.BaseStream.Length - nextInOld.Offset);
            oldFile.Close();
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
