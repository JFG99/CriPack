using CriPakInterfaces.IComponentsNew;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CriPakInterfaces.Models.ComponentsNew
{
    public class Row8 :  IUint8
    {
        [DefaultValue(0xFF)]
        public byte Value { get; set; }
    }
}
