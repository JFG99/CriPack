using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CriPakRepository
{

    public class EndianWriter : BinaryWriter
    {
        //Not really used or needed but I'll leave for now
        public EndianWriter(Stream input, Encoding encoding, bool isLittleEndian) : base(input, encoding)
        {
            IsLittleEndian = isLittleEndian;
        }

        public EndianWriter(Stream input, bool isLittleEndian) : this(input, Encoding.UTF8, isLittleEndian) { }

        public bool IsLittleEndian { get; set; }

        public void Write<T>(T value)
        {
            dynamic input = value;
            byte[] someBytes = BitConverter.GetBytes(input);
            if (!IsLittleEndian)
                someBytes = someBytes.Reverse().ToArray();

            base.Write(someBytes);
        }

        //public void Write(CriFile entry)
        //{
        //    if (entry.ExtractSizeType == typeof(Byte))
        //    {
        //        Write((Byte)entry.ExtractedFileSize);
        //    }
        //    else if (entry.ExtractSizeType == typeof(UInt16))
        //    {
        //        Write((UInt16)entry.ExtractedFileSize);
        //    }
        //    else if (entry.ExtractSizeType == typeof(UInt32))
        //    {
        //        Write((UInt32)entry.ExtractedFileSize);
        //    }
        //    else if (entry.ExtractSizeType == typeof(UInt64))
        //    {
        //        Write((UInt64)entry.ExtractedFileSize);
        //    }
        //    else if (entry.ExtractSizeType == typeof(Single))
        //    {
        //        Write((Single)entry.ExtractedFileSize);
        //    }
        //    else
        //    {
        //        throw new Exception("Not supported type!");
        //    }
        //}
    }
}
