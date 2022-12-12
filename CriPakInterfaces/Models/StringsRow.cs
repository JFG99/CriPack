using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces.IComponents
{
    public class StringsRow : IStringsRow
    {
        public int Id { get; set; }
        public string Name{ get; set; } 
        public int Mask { get; set; }
        public List<byte> ByteSegment { get; set; }
        public IRowValue Modifier { get; set; }
        public int RowOffset { get; set; }
        public bool IsStringsModifier { get; set; }
        public bool IsDataModifier { get; set; }
    } 
}