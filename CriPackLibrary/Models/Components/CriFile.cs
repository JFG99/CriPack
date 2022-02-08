using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces.Models.Components
{
    public class CriFile
    {
        public CriFile(){ }

        public CriFile(string fileName, ulong fileOffset, Type fileOffsetType, long fileOffsetPos, string tocName, string fileType, bool encrypted)
        {
            FileName = fileName;
            FileOffset = fileOffset;
            FileOffsetType = fileOffsetType;
            FileOffsetPos = fileOffsetPos;
            TOCName = tocName;
            FileType = fileType;
            Encrypted = encrypted;
        }

        public int ID { get; set; }
        public string TOCName { get; set; }
        public bool Encrypted { get; set; }
        public string LocalDir { get; set; }
        public string DirName { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public int FileSize { get; set; }
        public long FileSizePos { get; set; }
        public Type FileSizeType { get; set; }
        public ulong FileOffset { get; set; }
        public long FileOffsetPos { get; set; }
        public Type FileOffsetType { get; set; }
        public int ExtractSize { get; set; }
        public long ExtractSizePos { get; set; }
        public Type ExtractSizeType { get; set; }
        public ulong Offset { get; set; }
        public string UserString { get; set; }
        public ulong UpdateDateTime { get; set; }
    }
}
