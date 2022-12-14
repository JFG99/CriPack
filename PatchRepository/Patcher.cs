using CriPakInterfaces.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CriPakRepository.Helpers;

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
            var oldFile = new BinaryReader(File.OpenRead(package.FilePath));
            var newCPK = new BinaryWriter(File.OpenWrite(cpkDir));
            var patchList = package.DisplayList.Where(x => fileList.Keys.Any(y => x.FileName.ToLower().Equals(y.ToLower())));           
            var unpatchedList = package.DisplayList;
            var patchLoc = unpatchedList.IndexOf(patchList.First());
            var patchedFilesForRepack = MapPatchList(fileList, package.DisplayList);
            var test = newCPK;
            /*TODO:
                Read in PatchList and Compress as need for a populated DisplayList ordered by Offset.
                Foreach item in patch list:   
                    Find index from unpatched.
                    Set Flag on file that is being patched
                    Get archive length difference between the 2.
                     --That difference will move the offset of ALL files remaining in the unpatched list
                    Cascade the difference in the Offset values.                    
                Process corrected "unpatched list" for final values and locations to update the Headers
                Foreach item in "new" full list
                    check patched flag and either
                        stream binary from old CPK to new CPK
                        or
                        read binary from patched file 
                        compress if needed
                        stream to new CPK   
            */


        }

        //After this runs the new CPK needs to be written so the compression can also be written without losing it.
        //Compression method is the Really slow.  Need to improve speeds.  
        //Some files get higher compression, some get less.  Why is this crilayla method not exact in its compression for same size files? Especially the Textures.

        private IEnumerable<PatchList> MapPatchList(Dictionary<string,string> filePathsForPatch, IEnumerable<DisplayList> currentFiles)
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
                var patchFile = new BinaryReader(File.OpenRead(file));
                var bytes = File.ReadAllBytes(file);
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
