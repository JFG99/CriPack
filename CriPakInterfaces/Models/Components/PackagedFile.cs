using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces.Models.Components
{
    public class PackagedFile : IPackagedFile
    {
        public PackagedFile()
        {
        }

        public PackagedFile(string fileName, ulong fileOffset, long fileOffsetPos, string tocName, string fileType, bool encrypted)
        {
            FileName = fileName;
            FileOffset = fileOffset;
            FileOffsetPos = fileOffsetPos;
            TOCName = tocName;
            FileType = fileType;
            IsEncrypted = encrypted;
        }

        public int? FileId { get; set; }
        public string FileName { get; set; }
        public int FileSize { get; set; }
        public ulong FileOffset { get; set; }
        public int CompressedFileSize { get; set; }
        public string FileType { get; set; }
        public float CompressionPercentage { get; set; }
        public int ExtractedFileSize { get; set; }
        public string LocalName { get; set; }
        public string TOCName { get; set; }
        public long FileOffsetPos { get; set; }
        public bool IsEncrypted { get; set; }
    }
}
