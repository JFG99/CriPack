using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces.IComponents
{
    public interface IUint32 : IValue<uint>
    {
        uint Value { get; }
    }
}
