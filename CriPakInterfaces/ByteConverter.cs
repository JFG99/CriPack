using CriPakInterfaces.IComponents;
using CriPakInterfaces.Models.Components2;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CriPakInterfaces
{
    public static class ByteConverter
    {
        public static Dictionary<int, int> ReadLength = new Dictionary<int, int>()
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
            {0xA, 8},
            {0xB, 4}

        };
        public static Dictionary<int, Func<byte[], int, long>> MapInt = new Dictionary<int, Func<byte[], int, long>>()
        {
            {2, (value, index) => BitConverter.ToInt16(value, index) },
            {4, (value, index) => BitConverter.ToInt32(value, index) },
            {8, (value, index) => BitConverter.ToInt64(value, index) },

        };
        public static Dictionary<int, Func<byte[], int, ulong>> MapUInt = new Dictionary<int, Func<byte[], int, ulong>>()
        {
            {2, (value, index) => BitConverter.ToUInt16(value, index) }, 
            {3, (value, index) => BitConverter.ToUInt16(value, index) },
            {4, (value, index) => BitConverter.ToUInt32(value, index) },
            {5, (value, index) => BitConverter.ToUInt32(value, index) },
            {6, (value, index) => BitConverter.ToUInt64(value, index) },
            {7, (value, index) => BitConverter.ToUInt64(value, index) },

        };
        public static Dictionary<int, Func<byte[], int, float>> Map = new Dictionary<int, Func<byte[], int, float>>()
        {
            {8, (value, index) => BitConverter.ToSingle(value, index) }

        };

        public static Dictionary<int, Func<IOriginalPacket, int, IRowValue>> MapRow = new Dictionary<int, Func<IOriginalPacket, int, IRowValue>>()
        {
            {0, (packet, offset) => new Row8(){Value = packet.GetByteFrom(offset), Type = typeof(byte), TypeSelect = 0, Position = offset} },
            {1, (packet, offset) => new Row8(){Value = packet.GetByteFrom(offset), Type = typeof(byte), TypeSelect = 1, Position = offset} },
            {2, (packet, offset) => new Row16(){Value = BitConverter.ToUInt16(packet.GetBytesFrom(offset, 2).Reverse().ToArray(), 0), Type = typeof(ushort), TypeSelect = 2, Position = offset} },
            {3, (packet, offset) => new Row16(){Value = BitConverter.ToUInt16(packet.GetBytesFrom(offset, 2).Reverse().ToArray(), 0), Type = typeof(ushort), TypeSelect = 3, Position = offset} },
            {4, (packet, offset) => new Row32(){Value = BitConverter.ToUInt32(packet.GetBytesFrom(offset, 4).Reverse().ToArray(), 0), Type = typeof(uint), TypeSelect = 4, Position = offset} },
            {5, (packet, offset) => new Row32(){Value = BitConverter.ToUInt32(packet.GetBytesFrom(offset, 4).Reverse().ToArray(), 0), Type = typeof(uint), TypeSelect = 5, Position = offset} },
            {6, (packet, offset) => new Row64(){Value = BitConverter.ToUInt64(packet.GetBytesFrom(offset, 8).Reverse().ToArray(), 0), Type = typeof(ulong), TypeSelect = 6, Position = offset} },
            {7, (packet, offset) => new Row64(){Value = BitConverter.ToUInt64(packet.GetBytesFrom(offset, 8).Reverse().ToArray(), 0), Type = typeof(ulong), TypeSelect = 7, Position = offset} },
            {8, (packet, offset) => new RowFloat(){Value = BitConverter.ToUInt64(packet.GetBytesFrom(offset, 4).Reverse().ToArray(), 0), Type = typeof(float), TypeSelect = 8, Position = offset} },
            {0xA, (packet, offset) => new RowString(){Value = "Not Implemented", Type = typeof(string), TypeSelect = 0xA, Position = offset} },
            {0xB, (packet, offset) => new RowByteArray(){Value = new byte[]{ }, Type = typeof(byte[]), TypeSelect = 0xB, Position = offset} },
        };
    }
}
