using CriPakInterfaces.IComponents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CriPakInterfaces.Models.Components2
{
    public class RowString : Row<string>, IString
    {
        [DefaultValue(0xFFFFFFFFFFFFFFFF)]
        public new string Value { get; set; }
        public new Type Type { get; set; }

        public override Type GetType()
        {
            return Value.GetType();
        }

        public override string GetValue()
        {
            return Value;
        }
    }
}
