using CriPakInterfaces.IComponents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CriPakInterfaces.Models.Components
{ 
    public class Row64 : RowValue<ulong>, IRowValue, IUint64
    {
        public Row64(ulong value) : base(value) { }

        [DefaultValue(0xFFFFFFFFFFFFFFFF)]
        public ulong Value { get; set; }
    }
}
