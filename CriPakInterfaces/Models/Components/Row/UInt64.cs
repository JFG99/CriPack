using CriPakInterfaces.IComponents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CriPakInterfaces.Models.Components
{ 
    public class UInt64 : RowValue<ulong>, IUint64
    {
        public UInt64(ulong value) : base(value) { }

        [DefaultValue(0xFFFFFFFFFFFFFFFF)]
        public ulong Value { get; set; }
    }
}
