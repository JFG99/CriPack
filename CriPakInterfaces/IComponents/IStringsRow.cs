using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces.IComponents
{
    public interface IStringsRow : IModifier
    {
        string Name{ get; set; } 
        int Mask { get; set; }
        List<byte> ByteSegment { get; set; }
        int RowOffset { get; set; }
        bool IsStringsModifier { get; set; }
        bool IsDataModifier { get; set; }
    } 
}