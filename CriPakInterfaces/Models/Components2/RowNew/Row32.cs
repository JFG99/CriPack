using CriPakInterfaces.IComponentsNew;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CriPakInterfaces.Models.ComponentsNew
{
    public class Row32 :  IUint32
    {
        [DefaultValue(0xFFFFFFFF)]
        public uint Value { get; set; }
    }
}
