using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces.IComponents
{
    public interface IUint32 : IRowValue<uint>
    {
        uint Value { get; set; }
    }
}
