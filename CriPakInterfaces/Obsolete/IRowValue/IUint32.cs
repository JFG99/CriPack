using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces.IComponents
{
    public interface IUint32 : IRowValue
    {
        new uint Value { get; set; }
        new Type Type { get; set; }
        uint GetValue();
    }
}
