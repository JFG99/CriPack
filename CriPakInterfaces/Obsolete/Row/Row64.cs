using CriPakInterfaces.IComponents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CriPakInterfaces.Models.Components2 
{ 
    public class Row64 : Row<ulong>, IUint64
    {
        [DefaultValue(0xFFFFFFFFFFFFFFFF)]
        public new ulong Value { get; set; }
        public new Type Type { get; set; }
        public override int Length => 8;
        public override ulong GetValue()
        {
            return Value;
        }
        public override Type GetType()
        {
            return Value.GetType();
        }
    }
}
