using CriPakInterfaces.IComponents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CriPakInterfaces.Models.Components
{
    public class RowString : IString
    {
        [DefaultValue(0xFFFFFFFFFFFFFFFF)]
        public string Value { get; set; }
    }
}
