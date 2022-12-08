using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces.IComponents
{
    public interface IUint8 : IRowValue<byte>
    {
        byte Value { get; set; }
    }
}
