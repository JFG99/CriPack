using CriPakInterfaces.IComponents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CriPakInterfaces.Models.Components
{
    public class Row8 : RowValue<byte>, IRowValue, IUint8
    {
        public Row8(byte value) : base(value) {}

        [DefaultValue(0xFF)]
        public byte Value { get; set; }
    }
}
