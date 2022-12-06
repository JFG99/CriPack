using CriPakInterfaces.IComponents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CriPakInterfaces.Models.Components
{
    public class Row32 :  IUint32
    {
        [DefaultValue(0xFFFFFFFF)]
        public uint Value { get; set; }
    }
}
