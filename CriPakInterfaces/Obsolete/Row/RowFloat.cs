using CriPakInterfaces.IComponents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CriPakInterfaces.Models.Components2
{
    public class RowFloat : Row<float>, IFloat
    {
        [DefaultValue(0xFFFFFFFFFFFFFFFF)]
        public new float Value { get; set; }
        public new Type Type { get; set; }
        public override int Length => 4;

        public override Type GetType()
        {
            return Value.GetType();
        }

        public override float GetValue()
        {
            return Value;
        }
    }
}
