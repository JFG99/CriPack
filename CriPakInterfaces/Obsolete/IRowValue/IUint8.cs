using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces.IComponents
{
    public interface IUint8 : IRowValue
    {
        new byte Value { get; set; }
        new Type Type { get; set; }
        byte GetValue();
    }
}
