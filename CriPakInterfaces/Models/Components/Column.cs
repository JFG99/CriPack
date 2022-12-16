using System;
using System.Collections.Generic;
using System.Linq;
using CriPakInterfaces.Models.Components.Enums;

namespace CriPakInterfaces.Models.Components
{
    public class Column
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NameLength { get; set; }
        public byte[] ByteSegment { get; set; }
        public bool IsSegmentRemoved { get; set; }
        public int OffsetInTable { get; set; }
        public ulong OffsetInData => (ulong)BitConverter.ToInt16(ByteSegment.Skip(1).Reverse().ToArray(), 0);
        public byte Flag => ByteSegment[0];
        public int TypeMask => !IsSegmentRemoved ? Flag & (int)CRITYPE.MASK : 0;
        public int RowStorageMask => !IsSegmentRemoved ? Flag & (int)STORAGE.MASK : 0;
        public bool IsStoredInRow => !(RowStorageMask == (int)STORAGE.NONE || RowStorageMask == (int)STORAGE.ZERO || RowStorageMask == (int)STORAGE.CONSTANT);
        public int RowReadLength => RowConvertMask[TypeMask];

        private Dictionary<int, int> RowConvertMask = new Dictionary<int, int>()
        {
            {0, 1},
            {1, 1},
            {2, 2},
            {3, 2},
            {4, 4},
            {5, 4},
            {6, 8},
            {7, 8},
            {8, 4},
            {0xA, 4},
            {0xB, 8}
        };
    }
}
