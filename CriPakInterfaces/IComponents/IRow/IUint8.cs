using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces.IComponents
{
    public interface IUint8 : IValue<byte>
    {
        byte Value { get; }
    }
}
