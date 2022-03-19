using CriPakInterfaces;
using CriPakInterfaces.IComponents;
using CriPakInterfaces.Models.Components2;
using CriPakInterfaces.Models;
using CriPakRepository.Repository;
using System.IO;
using File = CriPakInterfaces.Models.Components2.File;
using System.Collections.Generic;
using System.Linq;
using CriPakRepository.Helpers;
using System;

namespace CriPakRepository.Writers
{
    public class FileWriter:  Writer<IFiles>, IWriter<IFiles>
    {
        public IEndianReader Stream { get; set; }
        public override void Write(IFiles data)
        {
            foreach(var entry in data.FileMeta)
            {
                var fileData = GetFileData(entry);
                var bytes  = fileData.PacketBytes;
                if (CompressionCheck(fileData, entry.CompressedString))
                {                    
                    bytes = fileData.PacketBytes.ToArray().DecompressLegacyCRI(); 
                }
                WriteFile(bytes, entry.FileName);
            }

            //Debug.WriteLine(" FileName :{0}\n    FileOffset:{1:x8}    ExtractSize:{2:x8}   ChunkSize:{3:x8} {4}",
            //                                            entries[i].FileName.ToString(),
            //                                            (long)entries[i].FileOffset,
            //                                            entries[i].ExtractSize,
            //                                            entries[i].FileSize,
            //                                            ((float)i / (float)entries.Count) * 100f);
            //string dstpath = outDir + "/" + currentName;
        }

        private void WriteFile(IEnumerable<byte> fileData, string fileName)
        {
            if (!Directory.Exists(OutputDirectory))
            {
                Directory.CreateDirectory(OutputDirectory);
            }
            System.IO.File.WriteAllBytes(Path.Combine(OutputDirectory, fileName), fileData.ToArray());
        }


        private IPacket GetFileData(File data)
        {
            Stream = new EndianReader<FileStream, EndianData>(System.IO.File.Open(FileName, FileMode.Open, FileAccess.Read, FileShare.Read), new EndianData(true));
            Stream.BaseStream.Position = Convert.ToInt64(data.Location);
            var original = new OriginalPacket()
            {
                PacketBytes = Stream.ReadBytes( data.IsCompressed ? data.FileSize : data.ExtractSize)
            };
            Stream.Close();
            return original;
        }

        private bool CompressionCheck(IPacket file, string compressedName)
        {
            return compressedName.Equals(file.ReadString(8));
        }
    }
}
