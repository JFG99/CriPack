using CriPakInterfaces.IComponents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CriPakInterfaces.Models.Components
{
    public class Float : RowValue<float>, IFloat
    {
        public Float(float value) : base(value) { }        

        [DefaultValue(0xFFFFFFFFFFFFFFFF)]
        public float Value { get; set; }   
    }
}
