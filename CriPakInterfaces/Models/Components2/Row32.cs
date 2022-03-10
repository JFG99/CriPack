using CriPakInterfaces.IComponents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CriPakInterfaces.Models.Components2
{
    public class Row32 : Row<uint>, IUint32
    {
        [DefaultValue(0xFFFFFFFF)]
        public new uint Value { get; set; }
        public new Type Type { get; set; }
        public override int Length => 4;

        public override uint GetValue()
        {
            return Value;
        }
        public override Type GetType()
        {
            return Value.GetType();
        }
    }
}
