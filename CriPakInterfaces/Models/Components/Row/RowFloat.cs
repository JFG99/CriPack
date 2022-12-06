using CriPakInterfaces.IComponents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CriPakInterfaces.Models.Components
{
    public class RowFloat :  IFloat
    {
        [DefaultValue(0xFFFFFFFFFFFFFFFF)]
        public  float Value { get; set; }       
    }
}
