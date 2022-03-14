using CriPakInterfaces.IComponents;
using CriPakInterfaces.Models.Components2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CriPakInterfaces
{
    public static class ByteConverter
    {
        
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
            {0xA, (packet, offset) => new RowString(){Value = packet.ReadCString(4 + offset, Encoding.UTF8) , Type = typeof(string), TypeSelect = 0xA, Position = offset, Length = packet.GetLastStringLength() + 1} },
            {0xB, (packet, offset) => new RowByteArray(){Value = new byte[]{ }, Type = typeof(byte[]), TypeSelect = 0xB, Position = offset} },
        }; 
        
        public static Dictionary<int, Func<IEnumerable<byte>, IComponentsNew.IRowValue>> MapBytes = new Dictionary<int, Func<IEnumerable<byte>, IComponentsNew.IRowValue>>()
        {
            {0, (bytes) => new Models.ComponentsNew.Row8(){Value = bytes.First() } },
            {1, (bytes) => new Models.ComponentsNew.Row8(){Value = bytes.First() } },
            {2, (bytes) => new Models.ComponentsNew.Row16(){Value = BitConverter.ToUInt16(bytes.Reverse().ToArray(), 0) } },
            {3, (bytes) => new Models.ComponentsNew.Row16(){Value = BitConverter.ToUInt16(bytes.Reverse().ToArray(), 0) } },
            {4, (bytes) => new Models.ComponentsNew.Row32(){Value = BitConverter.ToUInt32(bytes.Reverse().ToArray(), 0) } },
            {5, (bytes) => new Models.ComponentsNew.Row32(){Value = BitConverter.ToUInt32(bytes.Reverse().ToArray(), 0) } },
            {6, (bytes) => new Models.ComponentsNew.Row64(){Value = BitConverter.ToUInt64(bytes.Reverse().ToArray(), 0) } },
            {7, (bytes) => new Models.ComponentsNew.Row64(){Value = BitConverter.ToUInt64(bytes.Reverse().ToArray(), 0) } },
            {8, (bytes) => new Models.ComponentsNew.RowFloat(){Value = BitConverter.ToUInt64(bytes.Reverse().ToArray(), 0) } },
            {0xA, (bytes) => new Models.ComponentsNew.Row32(){Value = BitConverter.ToUInt32(bytes.Reverse().ToArray(), 0) } },
            {0xB, (bytes) => new Models.ComponentsNew.Row32(){Value = BitConverter.ToUInt32(bytes.Reverse().ToArray(), 0) } }
        };
    }
}

//Encoding.UTF8.GetString(bytes.Skip(offset).ToArray())

//{ 0xA, (bytes) => new RowString() { Value = "NotImplemented", Type = typeof(string), TypeSelect = 0xA, Length = bytes.Count() + 1 } },
//{ 0xB, (bytes) => new RowByteArray() { Value = new byte[] { }, Type = typeof(byte[]), TypeSelect = 0xB } },