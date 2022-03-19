using CriPakInterfaces.IComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces.Models.Components2
{
    public class Files : IFiles
    {
        public IEnumerable<File> FileMeta { get; set; }
        public IEnumerable<byte> WriteableBytes { get; set; }
    }
}
