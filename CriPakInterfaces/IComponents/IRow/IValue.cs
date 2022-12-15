using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CriPakInterfaces.IComponents
{
    public interface IValue<TType> : IValue where TType : struct
    {
        TType GetValue();
    }

    

    public interface IValue { }
}