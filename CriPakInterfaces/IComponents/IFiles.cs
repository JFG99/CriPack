using CriPakInterfaces.Models.Components2;
using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces.IComponents
{
    public interface IFiles
    {
        IEnumerable<File> FileMeta { get; set; }
        IEnumerable<byte> WriteableBytes { get; set; }
    }
}
