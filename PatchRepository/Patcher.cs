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

        //Current Proof code. Very Bad, Very Slow, not complete.  But Skeleton outline is in place and functions for whats here.
        //TODO:
        //      Implement Cascading offset adjustments, while CPK is being written.
        //      Fix Header offsets and sizes.
        //      Clean up the newly patch CPK for final write.


        public void Patch(CriPak package, string cpkDir, Dictionary<string, string> fileList)
        {
            var oldFile = new EndianReader<FileStream, EndianData>(System.IO.File.Open(package.FilePath, FileMode.Open, FileAccess.Read, FileShare.Read), new EndianData(true));
            var newCPK = new BinaryWriter(System.IO.File.OpenWrite(cpkDir));
            var patchList = package.ViewList.Where(x => fileList.Keys.Any(y => x.FileName.ToLower().Equals(y.ToLower()))).OrderBy(x => x.Offset).ToList();
            var firstPatchOffset = patchList.First().Offset;
            var unpatchedList = package.ViewList;
            var patchLoc = unpatchedList.IndexOf(patchList.First());
            var testloc = package.Sections[1].HeaderData.Rows.Where(x => x.Id == 390);
            //var patchedFilesForRepack = MapPatchList(fileList, package.ViewList);
            //var test = newCPK;


            oldFile.CopyStream(newCPK.BaseStream, firstPatchOffset);
            var tempPatch = new List<PatchList>();
            foreach (var file in patchList)
            {
                //TODO: work out the position coordination.  Crilayla has some footers in oldFile that aren't necessary, and not in the new ones.  
                //Need to ensure the correct positions are being set and that oldFile is streaming appropriately.  
                if(file.Offset > newCPK.BaseStream.Position - tempPatch.Sum(x => (long)x.LengthDifference))
                {
                    //This.  Copy the distance between current File.Offset and where the oldFile BaseStream is.  BaseStream needs properly handled at the end of each loop.
                    oldFile.CopyStream(newCPK.BaseStream, (int)file.ArchiveLength);
                }
                var newFile = new PatchList();
                var patchStream = new EndianReader<FileStream, EndianData>(System.IO.File.Open(fileList[file.FileName.ToLower()], FileMode.Open, FileAccess.Read, FileShare.Read), new EndianData(true));
                
                newFile.IndexInArchive = unpatchedList.ToList().IndexOf(file);
                var oldFilea = unpatchedList.ToArray()[newFile.IndexInArchive];
                newFile.Id = file.Id;
                newFile.FileName = file.FileName;
                newFile.Offset = file.Offset;
                newFile.Type = file.Type;
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
                tempPatch.Add(newFile); 
            }
            var testing = "";

            /*TODO:                
            comparing patch list(remove the ones that are same as they are processed to shorten comparison time and to stop comparing when list is empty)                        
            either:
            
                stream binary from old CPK to new CPK
                or
                read binary from patched file 
                compress if needed
                stream to new CPK   
                
                Track file size difference for patched files. Cascade location updates in the TOC FileOffset while writing.

            When Finished: Update HDR Meta with new content sizes and ETOC start location.
            */


        }

        //After this runs the new CPK needs to be written so the compression can also be written without losing it.
        //Compression method is the Really slow.  Need to improve speeds.  
        //Some files get higher compression, some get less.  Why is this crilayla method not exact in its compression for same size files? Especially the Textures.

        private IEnumerable<PatchList> MapPatchList(Dictionary<string,string> filePathsForPatch, IEnumerable<IFileViewer> currentFiles)
        {
            var currentFilesForPatch = currentFiles.Where(x => filePathsForPatch.Keys.Any(y => x.FileName.ToLower().Equals(y.ToLower())));
            var tempPatch = new List<PatchList>();
            foreach (var item in currentFilesForPatch)
            {
                var newFile = new PatchList();
                newFile.IndexInArchive = currentFiles.ToList().IndexOf(item);
                var oldFile = currentFiles.ToArray()[newFile.IndexInArchive];
                newFile.Id = oldFile.Id;
                newFile.FileName = oldFile.FileName;
                newFile.Offset = oldFile.Offset;
                newFile.Type = oldFile.Type;
                tempPatch.Add(newFile);
            }
            var patch = new List<PatchList>();
            foreach(var file in filePathsForPatch.Values.ToList())
            {
                var newFile = tempPatch.Where(x => file.ToUpper().Contains(x.FileName)).First();
                var oldFile = currentFiles.ToArray()[newFile.IndexInArchive];
                var patchFile = new BinaryReader(System.IO.File.OpenRead(file));
                var bytes = System.IO.File.ReadAllBytes(file);
                var compressedFile = bytes;
                if (oldFile.Percentage < 100)
                {
                    compressedFile = bytes.CompressCRILAYLA();
                }                
                newFile.LengthDifference = (ulong)bytes.Length - oldFile.ArchiveLength;
                newFile.ArchiveLength = (ulong)compressedFile.Length;
                newFile.ExtractedLength = (uint)bytes.Length;
                newFile.IsPatched = true;                
                patch.Add(newFile);
            }
            return patch;

            //var entries = package.CriFileList.OrderBy(x => x.FileOffset).ToList();
            //var badEntries = package.CriFileList.Where(x => x.FileOffsetType == null); 
            //var i = 0;
            //var msg = "";
            //foreach (var entry in entries)
            //{
            //    //Dispatcher.Invoke(() => progressbar1.Value = i++ / (float)package.CriFileList.Count * 100f);

            //    if (entry.FileType != "CONTENT")
            //    {
            //        if (entry.FileType == "FILE")
            //        {
            //            // *****I'm too lazy to figure out how to update the ContentOffset position so this works :)
            //            // No it doesnt.
            //            if ((ulong)newCPK.BaseStream.Position < package.ContentOffset)
            //            {
            //                ulong padLength = package.ContentOffset - (ulong)newCPK.BaseStream.Position;
            //                for (ulong z = 0; z < padLength; z++)
            //                {
            //                    newCPK.Write((byte)0);
            //                }
            //            }
            //        }
            //        Debug.WriteLine($"Got File: {entry.FileName}");

            //        //If Not in dictionary, copy original data
            //        if (!fileList.ContainsKey(entry.FileName))
            //        {
            //            oldFile.BaseStream.Seek((long)entry.FileOffset, SeekOrigin.Begin);

            //            entry.FileOffset = (ulong)newCPK.BaseStream.Position;

            //            if (entry.FileName.ToString() == "ETOC_HDR")
            //            {
            //                package.EtocOffset = entry.FileOffset;
            //                Debug.WriteLine($"Fix ETOC_OFFSET to {package.EtocOffset}");
            //            }

            //            package.UpdateCriFile(entry);
            //            byte[] chunk = oldFile.ReadBytes(entry.CompressedFileSize);
            //            newCPK.Write(chunk);

            //            if ((newCPK.BaseStream.Position % 0x800) > 0 && i <= package.CriFileList.Count)
            //            {
            //                long cur_pos = newCPK.BaseStream.Position;
            //                for (int j = 0; j < (0x800 - (cur_pos % 0x800)); j++)
            //                {
            //                    newCPK.Write((byte)0);
            //                }
            //            }
            //        }
            //        else
            //        {
            //            //Got patch file name
            //            //msg = $"Patching: {entry.FileName}";
            //            //Dispatcher.Invoke(() => updateTextblock(msg));


            //            var newFileBytes = File.ReadAllBytes(fileList[entry.FileName]);
            //            entry.FileOffset = (ulong)newCPK.BaseStream.Position;
            //            msg = $"Storing data: {newFileBytes.Length}\r\n";
            //            var originalSize = 0;
            //            if (isForceCompress && (entry.CompressedFileSize < entry.ExtractedFileSize) && entry.FileType == "FILE")
            //            {
            //                msg = $"Compressing and storing data:{newFileBytes.Length}\r\n";
            //                originalSize = newFileBytes.Length;
            //                newFileBytes = newFileBytes.CompressCRILAYLA();
            //            }
            //            WriteChunkData(newCPK, entry, newFileBytes, originalSize, msg);

            //            if ((newCPK.BaseStream.Position % 0x800) > 0 && i < entries.Count - 1)
            //            {
            //                long cur_pos = newCPK.BaseStream.Position;
            //                for (int j = 0; j < (0x800 - (cur_pos % 0x800)); j++)
            //                {
            //                    newCPK.Write((byte)0);
            //                }
            //            }
            //        }
            //    }
            //    else
            //    {
            //        // Content is special.... just update the position
            //        package.UpdateCriFile(entry);
            //    }
            //}

            //WriteCPK(newCPK);
            //Dispatcher.Invoke(() => updateTextblock("Writing TOC...."));

            //WriteITOC(newCPK);
            //WriteTOC(newCPK);
            //WriteETOC(newCPK, package.EtocOffset);
            //WriteGTOC(newCPK);

            //newCPK.Close();
            //oldFile.Close();
            //Dispatcher.Invoke(() => updateTextblock($"Saving CPK to {cpkDir}...."));
            //MessageBox.Show("CPK Patched.");
            //Dispatcher.Invoke(() => progressbar1.Value = 0f);
        }
    }
}
