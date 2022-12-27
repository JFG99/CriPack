using CriPakInterfaces.IComponents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CriPakInterfaces.Models.Components
{
    public class ByteArray : IByteArray
    {
        [DefaultValue(0xFFFFFFFFFFFFFFFF)]
        public byte[] Value { get; set; }
    }
}
