using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces.IComponents
{
    public interface IUint16 : IRowValue
    {
        new ushort Value { get; set; }
        new Type Type { get; set; }
        ushort GetValue();
    }
}
