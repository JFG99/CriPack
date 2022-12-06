using CriPakInterfaces.IComponents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CriPakInterfaces.Models.Components
{
    public class Row16 : IUint16
    {
        [DefaultValue(0xFFFF)]
        public ushort Value { get; set; }
    }
}
