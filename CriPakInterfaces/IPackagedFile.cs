using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces
{
    public interface IPackagedFile : ICriFile
    {
        int CompressedFileSize { get; set; }
        float CompressionPercentage { get; }
        int ExtractedFileSize { get; set; }
    }
}
