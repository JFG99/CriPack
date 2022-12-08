using CriPakInterfaces.IComponents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CriPakInterfaces.Models.Components
{
    public class Row32 : IRowValue, IUint32
    {
        public Row32(uint value)  
        { 
            Value = value;
        }

        [DefaultValue(0xFFFFFFFF)]
        public uint Value { get; set; }
    }
}
