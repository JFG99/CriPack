using System;
using System.Linq;
using System.Text;
using System.IO;
using CriPakInterfaces;

namespace CriPakRepository.Repository

{
    public class EndianReader<TStream, T> : BinaryReader, IEndianReader
        where TStream : Stream
        where T : IEndian, new()
    {
        private readonly T _endian;
        public bool IsOpen { get; private set; }

        public EndianReader(TStream stream, T endian) : base(stream, Encoding.UTF8)
        {
            _endian = endian;
            IsOpen = true;
        }

        public override Stream BaseStream => base.BaseStream;

        public bool IsLittleEndian 
        { 
            get => _endian.IsLittleEndian;
            set => _endian.IsLittleEndian = value;
        }
        public byte[] Buffer
        {
            get => _endian.Buffer;
            set => _endian.Buffer = value;
        }

        public override void Close()
        {
            IsOpen = false;
            base.Close();
        }
        public override byte ReadByte() => base.ReadByte();

        public override double ReadDouble()
        {
            if (_endian.IsLittleEndian)
                return base.ReadDouble();
            FillMyBuffer(8);
            return BitConverter.ToDouble(_endian.Buffer.Take(8).Reverse().ToArray(), 0);
        }

        public override short ReadInt16()
        {
            if (_endian.IsLittleEndian)
                return base.ReadInt16();
            FillMyBuffer(2);
            return BitConverter.ToInt16(_endian.Buffer.Take(2).Reverse().ToArray(), 0);

        }

        public override int ReadInt32()
        {
            if (_endian.IsLittleEndian)
                return base.ReadInt32();
            FillMyBuffer(4);
            return BitConverter.ToInt32(_endian.Buffer.Take(4).Reverse().ToArray(), 0);

        }

        public override long ReadInt64()
        {
            if (_endian.IsLittleEndian)
                return base.ReadInt64();
            FillMyBuffer(8);
            return BitConverter.ToInt64(_endian.Buffer.Take(8).Reverse().ToArray(), 0);

        }

        public override float ReadSingle()
        {
            if (_endian.IsLittleEndian)
                return base.ReadSingle();
            FillMyBuffer(4);
            return BitConverter.ToSingle(_endian.Buffer.Take(4).Reverse().ToArray(), 0);
        }

        public override ushort ReadUInt16()
        {
            if (_endian.IsLittleEndian)
                return base.ReadUInt16();
            FillMyBuffer(2);
            return BitConverter.ToUInt16(_endian.Buffer.Take(2).Reverse().ToArray(), 0);
        }


        public override uint ReadUInt32()
        {
            if (_endian.IsLittleEndian)
                return base.ReadUInt32();
            FillMyBuffer(4);
            return BitConverter.ToUInt32(_endian.Buffer.Take(4).Reverse().ToArray(), 0);
        }

        public override ulong ReadUInt64()
        {
            if (_endian.IsLittleEndian)
                return base.ReadUInt64();
            FillMyBuffer(8);
            return BitConverter.ToUInt64(_endian.Buffer.Take(8).Reverse().ToArray(), 0);
        }

        private void FillMyBuffer(int numBytes)
        {
            int offset = 0;
            int num2 = 0;
            if (numBytes == 1)
            {
                num2 = BaseStream.ReadByte();
                if (num2 == -1)
                {
                    throw new EndOfStreamException("Attempted to read past the end of the stream.");
                }
                _endian.Buffer[0] = (byte)num2;
            }
            else
            {
                do
                {
                    num2 = BaseStream.Read(_endian.Buffer, offset, numBytes - offset);
                    if (num2 == 0)
                    {
                        throw new EndOfStreamException("Attempted to read past the end of the stream.");
                    }
                    offset += num2;
                }
                while (offset < numBytes);
            }
        }
    }       
}
