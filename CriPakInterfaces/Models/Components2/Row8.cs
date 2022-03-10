using CriPakInterfaces.IComponents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CriPakInterfaces.Models.Components2
{
    public class Row8 : Row<byte>, IUint8
    {
        [DefaultValue(0xFF)]
        public new byte Value { get; set; }
        public new Type Type { get; set; }
        public override int Length => 1;
        public override byte GetValue()
        {
            return Value;
        }
        public override Type GetType()
        {
            return Value.GetType();
        }
    }
}
