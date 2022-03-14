using CriPakInterfaces.IComponentsNew;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CriPakInterfaces.Models.ComponentsNew
{
    public class Row16 : IUint16
    {
        [DefaultValue(0xFFFF)]
        public ushort Value { get; set; }
    }
}
