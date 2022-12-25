using CriPakInterfaces.IComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces.Models.Components
{
    public class Value<TType> : IValue<TType> where TType : struct
    {
        protected TType _value;

        protected Value(TType value)
        {
            _value = value;
        }

        public TType GetValue()
        {
            return _value;            
        }

        public void SetValue(TType value)
        {
            _value = value;
        }
    }
}
