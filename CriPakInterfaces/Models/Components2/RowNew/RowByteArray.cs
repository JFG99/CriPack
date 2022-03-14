using CriPakInterfaces.IComponentsNew;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CriPakInterfaces.Models.ComponentsNew
{
    public class RowByteArray : IByteArray
    {
        [DefaultValue(0xFFFFFFFFFFFFFFFF)]
        public byte[] Value { get; set; }
    }
}
