using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces
{
    public interface ICriFile : ICriEntity
    {
        string FileName {get;set; }
        string TOCName { get; set; }
        ulong FileOffset { get; set; }
        long FileOffsetPos { get; set; }
        string FileType { get; set; }
        bool IsEncrypted { get; set; }
    }
}
