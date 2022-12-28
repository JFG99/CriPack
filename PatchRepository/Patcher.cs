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
using static System.Net.WebRequestMethods;

namespace PatchRepository
{
    public class Patcher
    {
        //TODO:
        //      Fix Header offsets and sizes.

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
                    if (resetPositionAfterPatch)
                    {
                        oldFile.BaseStream.Position = nextInOld.Offset;
                        resetPositionAfterPatch = false;
                    }
                    // Have to subtract the length of the archive header for all file changes, but not the other table headers.
                    modifiedInNewArchive.Add(CreateEntry(nextInOld, newCPK.BaseStream.Position - 2048, currentIndex));
                    newCPK.CopyFrom(oldFile.BaseStream, Convert.ToInt64(nextInOld.ArchiveLength));
                    nextInOld = unpatchedList[++currentIndex];
                    if (oldFile.BaseStream.Position <= nextInOld.Offset)
                    {
                        newCPK.CopyFrom(oldFile.BaseStream, nextInOld.Offset - oldFile.BaseStream.Position);
                    }
                }
                                
                var patchStream = new EndianReader<FileStream, EndianData>(System.IO.File.Open(fileList[file.FileName.ToLower()], FileMode.Open, FileAccess.Read, FileShare.Read), new EndianData(true));
                modifiedInNewArchive.Add(CreateEntry(file, newCPK.BaseStream.Position - 2048, currentIndex));
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
                oldFile.BaseStream.Position = nextInOld.Offset;
                //}
                resetPositionAfterPatch = true;
                currentIndex = unpatchedList.ToList().IndexOf(file);
                nextInOld = unpatchedList[++currentIndex];
            }
            var lastFile = unpatchedList.Where(x => x.Id != 0).Last();
            while (nextInOld.Id != 0 && nextInOld.Id <= lastFile.Id)
            {                
                modifiedInNewArchive.Add(CreateEntry(nextInOld, newCPK.BaseStream.Position - 2048, currentIndex));
                newCPK.CopyFrom(oldFile.BaseStream, Convert.ToInt64(nextInOld.ArchiveLength));
                nextInOld = unpatchedList[++currentIndex]; 
                if (oldFile.BaseStream.Position < nextInOld.Offset)
                {
                    newCPK.CopyFrom(oldFile.BaseStream, nextInOld.Offset - oldFile.BaseStream.Position);
                }
            }

            //This captures the ETOC table
            if (oldFile.BaseStream.Position < nextInOld.Offset)
            {
                newCPK.CopyFrom(oldFile.BaseStream, nextInOld.Offset - oldFile.BaseStream.Position);
            }
            modifiedInNewArchive.Add(CreateEntry(nextInOld, newCPK.BaseStream.Position, currentIndex));
            newCPK.CopyFrom(oldFile.BaseStream, oldFile.BaseStream.Length - nextInOld.Offset);
            oldFile.Close();
            UpdateSections(newCPK, package, modifiedInNewArchive);
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

        private void UpdateSections(EndianWriter<FileStream, EndianData> newCpk, CriPak package, List<PatchList> modifiedInNewArchive)
        {
            //TODO: Add packet data to Sections so it can be modified easily and then streamed to the file.
            var cpkSection = package.Sections.First(x => x.Name.Equals("CPK"));
            var tocSection = package.Sections.First(x => x.Name.Equals("TOC"));
            var etocOffset = modifiedInNewArchive.First(x => x.FileName == "ETOC").Offset;
            var contentTableSize = etocOffset - package.Sections.First(x => x.Name == "CONTENT").Offset;
            var cpkContentSizeRowOffset = cpkSection.HeaderData.Rows.First(x => x.Name.Equals("ContentSize")).RowOffset; 
            var cpkEtocRowOffset = cpkSection.HeaderData.Rows.First(x => x.Name.Equals("EtocOffset")).RowOffset;
            var filesToUpdate = modifiedInNewArchive.Where(x => x.Type == ItemType.FILE);
            var tocPatchedPacket = new PatchedPacket() { PacketBytes = tocSection.Content.PacketBytes }; 
            tocPatchedPacket.MakeDecrypted(); 
            foreach (var file in filesToUpdate)
            {
                var tocRowData = tocSection.HeaderData.Rows.Where(x => x.Id == file.Id);
                var tocArchiveSizeRowOffset = tocRowData.First(x => x.Name.Equals("FileSize")).RowOffset;
                var tocExtractizeRowOffset = tocRowData.First(x => x.Name.Equals("ExtractSize")).RowOffset;
                var tocFileOffsetRowOffset = tocRowData.First(x => x.Name.Equals("FileOffset")).RowOffset;

                tocPatchedPacket.DecryptedBytes = tocPatchedPacket.DecryptedBytes.ToList()
                    .Splice(tocArchiveSizeRowOffset, BitConverter.GetBytes(Convert.ToInt32(file.ArchiveLength)).Reverse().ToList())
                    .Splice(tocExtractizeRowOffset, BitConverter.GetBytes(file.ExtractedLength).Reverse().ToList())
                    .Splice(tocFileOffsetRowOffset, BitConverter.GetBytes(file.Offset).Reverse().ToList());

            }

            var patchedPacket = new PatchedPacket() { PacketBytes = cpkSection.Content.PacketBytes };
            patchedPacket.MakeDecrypted();
            var packet = patchedPacket.DecryptedBytes.ToList();
            patchedPacket.DecryptedBytes = packet.Splice(cpkContentSizeRowOffset, BitConverter.GetBytes(contentTableSize).Reverse().ToList())
                .Splice(cpkEtocRowOffset, BitConverter.GetBytes(etocOffset).Reverse().ToList());

            patchedPacket.Encrypt();
            var cpkStream = new EndianReader<MemoryStream, EndianData>(new MemoryStream(patchedPacket.PacketBytes.ToArray()), new EndianData(true));
            newCpk.BaseStream.Position = cpkSection.Offset + 16;
            cpkStream.BaseStream.Position = 0;
            newCpk.CopyFrom(cpkStream.BaseStream, cpkStream.BaseStream.Length);


            tocPatchedPacket.Encrypt();
            var tocStream = new EndianReader<MemoryStream, EndianData>(new MemoryStream(tocPatchedPacket.PacketBytes.ToArray()), new EndianData(true));
            newCpk.BaseStream.Position = tocSection.Offset + 16;
            tocStream.BaseStream.Position = 0;
            newCpk.CopyFrom(tocStream.BaseStream, tocStream.BaseStream.Length);

        }

       
    }
}
