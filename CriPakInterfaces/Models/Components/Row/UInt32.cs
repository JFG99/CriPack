using CriPakInterfaces.IComponents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CriPakInterfaces.Models.Components
{
    public class UInt32 : Value<uint>, IUint32
    {
        public UInt32(uint value) : base(value) { }

        [DefaultValue(0xFFFFFFFF)]
        public uint Value => GetValue();
    }
}
