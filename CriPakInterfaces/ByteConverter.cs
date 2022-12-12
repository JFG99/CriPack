using CriPakInterfaces.IComponents;
using CriPakInterfaces.Models.Components;
using System;
using System.Collections.Generic;
using System.Linq;

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
        
        public static Dictionary<int, Func<IEnumerable<byte>, IRowValue>> MapBytes = new Dictionary<int, Func<IEnumerable<byte>, IRowValue>>()
        {
            {0, (bytes) => new UInt8(bytes.First()) },
            {1, (bytes) => new UInt8(bytes.First()) },
            {2, (bytes) => new Models.Components.UInt16(BitConverter.ToUInt16(bytes.Reverse().ToArray(), 0)){ } },
            {3, (bytes) => new Models.Components.UInt16(BitConverter.ToUInt16(bytes.Reverse().ToArray(), 0)){ } },
            {4, (bytes) => new Models.Components.UInt32(BitConverter.ToUInt32(bytes.Reverse().ToArray(), 0)){ } },
            {5, (bytes) => new Models.Components.UInt32(BitConverter.ToUInt32(bytes.Reverse().ToArray(), 0)){ } },
            {6, (bytes) => new Models.Components.UInt64(BitConverter.ToUInt64(bytes.Reverse().ToArray(), 0)){ } },
            {7, (bytes) => new Models.Components.UInt64(BitConverter.ToUInt64(bytes.Reverse().ToArray(), 0)){ } },
            {8, (bytes) => new Float(BitConverter.ToUInt64(bytes.Reverse().ToArray(), 0)){ } },
            {0xA, (bytes) => new Models.Components.UInt32(BitConverter.ToUInt32(bytes.Reverse().ToArray(), 0)){ } },
            {0xB, (bytes) => new Models.Components.UInt32(BitConverter.ToUInt32(bytes.Reverse().ToArray(), 0)){ } }
        };
    }
}

