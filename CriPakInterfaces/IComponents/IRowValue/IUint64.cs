using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CriPakInterfaces.IComponents
{
    public interface IUint64 : IRowValue
    {
        ulong Value { get; set; }
    }
}
