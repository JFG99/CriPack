using CriPakInterfaces.IComponents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CriPakInterfaces.Models.Components
{ 
    public class Row64 : IRowValue, IUint64
    {
        public Row64(ulong value)
        {
            Value = value;
        }

        [DefaultValue(0xFFFFFFFFFFFFFFFF)]
        public ulong Value { get; set; }
    }
}
