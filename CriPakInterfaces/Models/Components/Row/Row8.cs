using CriPakInterfaces.IComponents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CriPakInterfaces.Models.Components
{
    public class Row8 :  IUint8
    {
        [DefaultValue(0xFF)]
        public byte Value { get; set; }
    }
}
