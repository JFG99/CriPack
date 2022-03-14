using CriPakInterfaces.IComponents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CriPakInterfaces.Models.Components2
{
    public class Row16 : Row<ushort>, IUint16
    {
        [DefaultValue(0xFFFF)]
        public new ushort Value { get; set; }
        public new Type Type { get; set; }
        public override int Length => 2;
        public override Type GetType()
        {
            return Value.GetType();
        }
        public override ushort GetValue()
        {
            return Value;
        }
    }
}
