using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces
{
    public interface IPackagedFile : ICriFile
    {
        int CompressedFileSize { get; set; }
        float CompressionPercentage { get; set; }
        int ExtractedFileSize { get; set; }
        //Do I need this. This is for extraction(?)
        string LocalName { get; set; }
    }
}
