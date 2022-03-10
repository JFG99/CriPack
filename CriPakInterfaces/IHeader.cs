using CriPakInterfaces.IComponents;
using CriPakInterfaces.Models.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces
{
    public interface IHeader
    {
        string DisplayName { get; set; }
        string ValidationName { get; set; }
        IPacket Packet { get; set; }
        IEnumerable<Column> Columns { get; set; }
        IEnumerable<IRowValue> Rows { get; set; }
        ulong Offset { get; set; }                
    }
}
