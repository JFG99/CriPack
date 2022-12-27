using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces.IComponents
{
    public interface ITabularRecord
    {
        int Index { get; set; }
        ulong Offset { get; set; }
        ulong Length { get; set; }  
        ulong Value { get; set; }
    }
}
