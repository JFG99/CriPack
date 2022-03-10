using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces.IComponents
{
    public interface IUint64 : IRowValue
    {
        new ulong Value { get; set; }
        new Type Type { get; set; }
        ulong GetValue();
    }
}
