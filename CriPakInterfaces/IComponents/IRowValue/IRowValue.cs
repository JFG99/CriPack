using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CriPakInterfaces.IComponents
{
    public interface IRowValue<TType> : IRowValue where TType : struct
    {
        TType GetRowValue();
    }

    public interface IRowValue { }
}