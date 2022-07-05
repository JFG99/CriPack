using CriPakInterfaces.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace PatchRepository
{
    public class Patcher
    {
        public void Patch(CriPak package, string cpkDir, Dictionary<string, string> fileList)
        {
            var oldFile = new BinaryReader(File.OpenRead(package.FilePath));
            var newCPK = new BinaryWriter(File.OpenWrite(cpkDir));
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
