using CriPakInterfaces;
using CriPakInterfaces.IComponents;
using CriPakInterfaces.Models.Components;
using CriPakInterfaces.Models;
using CriPakRepository.Repository;
using System.IO;
using File = CriPakInterfaces.Models.Components.File;
using System.Collections.Generic;
using System.Linq;
using CriPakRepository.Helpers;
using System;
using System.Threading;

namespace CriPakRepository.Writers
{
    public class FileWriter:  Writer<IFiles>, IWriter<IFiles>
    {
        public IEndianReader Stream { get; set; }
        public override void Write(IFiles data)
        {
            //TODO: Make use of Async stream.  This is a lot faster than it was, but async streams would speed it up even further. 
            var fileCount = 0;
            Stream = new EndianReader<FileStream, EndianData>(System.IO.File.Open(FileName, FileMode.Open, FileAccess.Read, FileShare.Read), new EndianData(true));
            foreach (var entry in data.FileMeta)
            {
                Stream.BaseStream.Position = Convert.ToInt64(entry.Location);
                if (entry.IsCompressed)
                {
                    WriteFile(Stream.ReadBytes(entry.FileSize).DecompressLegacyCRI(), entry.FileName);                    
                }
                else
                {
                    using (Stream file = System.IO.File.Create(Path.Combine(OutputDirectory, entry.FileName)))
                    {
                        CopyStream(file, entry.FileSize);
                    }                
                }
                fileCount++;
                Progress.Report((int)(fileCount / (float)data.FileMeta.Count() * 100));
            }
        }

        private void WriteFile(byte[] fileData, string fileName)
        {
            if (!Directory.Exists(OutputDirectory))
            {
                Directory.CreateDirectory(OutputDirectory);
            }
            System.IO.File.WriteAllBytes(Path.Combine(OutputDirectory, fileName), fileData);     
        }

        private void CopyStream(Stream output, int bytes)
        {
            byte[] buffer = new byte[81920];
            int read;
            while (bytes > 0 && (read = Stream.BaseStream.Read(buffer, 0, Math.Min(buffer.Length, bytes))) > 0)
            {
                output.Write(buffer, 0, read);
                bytes -= read;
            }
        }
    }
}
