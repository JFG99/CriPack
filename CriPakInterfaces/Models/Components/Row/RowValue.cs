using CriPakInterfaces.IComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces.Models.Components
{
    public class RowValue<TType> 
    {
        protected TType _value;

        protected RowValue(TType value)
        {
            _value = value;
        }

        public TType GetRowValue()
        {
            return _value;
        }
    }
}
