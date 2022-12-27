using CriPakInterfaces.IComponents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CriPakInterfaces.Models.Components
{
    public class UInt8 : Value<byte>, IUint8
    {
        public UInt8(byte value) : base(value) { }

        [DefaultValue(0xFF)]
        public byte Value => GetValue();
    }
}
