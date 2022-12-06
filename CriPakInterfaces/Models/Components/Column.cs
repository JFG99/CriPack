using System.Collections.Generic;
using CriPakInterfaces.Models.Components.Enums;

namespace CriPakInterfaces.Models.Components
{
    public class Column
    {
        public byte Flag { get; set; }
        public string Name { get; set; }
        public int NameOffset { get; set; }
        public int Mask => Flag & (int)CRITYPE.MASK;
        public int StoredMask => Flag & (int)STORAGE.MASK;
        public bool Stored => !(StoredMask == (int)STORAGE.NONE || StoredMask == (int)STORAGE.ZERO || StoredMask == (int)STORAGE.CONSTANT);
        public int RowReadLength => RowConvertMask[Mask];

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
            {0xB, 4}
        };
    }
}
