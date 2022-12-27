using CriPakInterfaces.IComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces.Models
{
    public class TabularRecord : ITabularRecord
    {
        public int Index { get; set; }
        public ulong Offset { get; set; }
        public ulong Length { get; set; }
        public ulong Value { get; set; }
    }
}
