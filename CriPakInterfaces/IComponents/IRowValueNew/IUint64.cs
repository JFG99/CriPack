using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces.IComponentsNew
{
    public interface IUint64 : IRowValue
    {
        ulong Value { get; set; }
    }
}
