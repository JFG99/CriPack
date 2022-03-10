using CriPakInterfaces.IComponents;
using CriPakInterfaces.Models.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CriPakInterfaces.Models.Components2
{
    public class Header : IHeader
    {
        public Header()
        {
            Columns = new List<Column>();
            Rows = new List<IRowValue>();
        }
        public string DisplayName { get; set; }
        public string ValidationName { get; set; }
        public IPacket Packet { get; set; }
        public IEnumerable<Column> Columns { get; set; }
        public IEnumerable<IRowValue> Rows { get; set; }
        public ulong Offset { get; set; }
    }
}
