using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces.Models.Components
{
    public class CriFile : PackagedFile, ICriFile, IPackagedFile
    {
        public CriFile() { }
        public CriFile(string fileName, ulong fileOffset, Type fileOffsetType, long fileOffsetPos, string tocName, string fileType, bool encrypted) 
            : base(fileName, fileOffset, fileOffsetPos, tocName, fileType, encrypted)
        {
            FileOffsetType = fileOffsetType;
        }

        public string LocalDir { get; set; }
        public string DirName { get; set; }
        public long FileSizePos { get; set; }
        public Type FileSizeType { get; set; }
        public Type FileOffsetType { get; set; }
        public long ExtractSizePos { get; set; }
        public Type ExtractSizeType { get; set; }
        public ulong Offset { get; set; }
        public string UserString { get; set; }
        public ulong UpdateDateTime { get; set; }
    }
}
