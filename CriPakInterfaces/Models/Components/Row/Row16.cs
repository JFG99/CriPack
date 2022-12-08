using CriPakInterfaces.IComponents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CriPakInterfaces.Models.Components
{
    public class Row16 : RowValue<ushort>, IRowValue, IUint16
    {
        public Row16(ushort value) : base(value) { }

        [DefaultValue(0xFFFF)]
        public ushort Value { get; set; }
    }
}
