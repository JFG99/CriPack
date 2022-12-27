using CriPakInterfaces.IComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces.Models.Components
{
    public class Row : IModifier
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Mask { get; set; }
        public string StringName { get; set; }
        public IEnumerable<byte> ByteSegment { get; set; }
        public IValue Modifier { get; set; }
        public int RowOffset { get; set; }
    }
}
