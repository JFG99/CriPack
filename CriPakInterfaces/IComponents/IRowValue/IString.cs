using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces.IComponents
{
    public interface IString : IRowValue
    {
        new string Value { get; set; }
        new Type Type { get; set; }
        string GetValue();
    }
}
