using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces.Models.Components2
{
    public class File
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public ulong Location { get; set; }
        public ulong ByteLength  { get; set; }
        public int ExtractSize { get; set; }
        public int FileSize { get; set; }
        public string CompressedString = "CRILAYLA";
        public bool IsCompressed => (FileSize / (float)ExtractSize) < 1.0;
    }
}
