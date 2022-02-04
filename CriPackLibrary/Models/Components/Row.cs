using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces.Models.Components
{
    public class Row
    {
        public Row()
        {
            Type = -1;
        }

        public int Type { get; set; }
        public byte uint8 { get; set; }
        public ushort uint16 { get; set; }
        public uint uint32 { get; set; }
        public ulong uint64 { get; set; }
        public float ufloat { get; set; }
        public string str { get; set; }
        public byte[] data { get; set; }
        public long position { get; set; }

        public object GetValue()
        {
            switch (Type)
            {
                case 0:
                case 1: return uint8;
                case 2:
                case 3: return uint16;
                case 4:
                case 5: return uint32;
                case 6:
                case 7: return uint64;
                case 8: return ufloat;
                case 0xA: return str;
                case 0xB: return data;
                default: return null;
            }
        }
        public new System.Type GetType()
        {
            switch (Type)
            {
                case 0:
                case 1: return uint8.GetType();

                case 2:
                case 3: return uint16.GetType();

                case 4:
                case 5: return uint32.GetType();

                case 6:
                case 7: return uint64.GetType();

                case 8: return ufloat.GetType();

                case 0xA: return str.GetType();

                case 0xB: return data.GetType();

                default: return null;
            }
        }

    }
}
