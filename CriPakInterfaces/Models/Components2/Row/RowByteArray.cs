using CriPakInterfaces.IComponents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CriPakInterfaces.Models.Components2
{
    public class RowByteArray : Row<byte[]>, IByteArray
    {
        [DefaultValue(0xFFFFFFFFFFFFFFFF)]
        public new byte[] Value { get; set; }
        public new Type Type { get; set; }

        public override Type GetType()
        {
            return Value.GetType();
        }

        public override byte[] GetValue()
        {
            return Value;
        }
    }
}
