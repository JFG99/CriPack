using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using LibCPK.Interfaces;
using CriPakInterfaces;

namespace CriPakRepository
{

    public class EndianWriter<TStream, T> : BinaryWriter, IEndianWriter
        where TStream : Stream
        where T : IEndian, new()
    {
        private readonly T _endian;
        public bool IsOpen { get; private set; }

        public EndianWriter(TStream stream, T endian) : base(stream, Encoding.UTF8)
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
        public void CopyFrom(Stream input, int bytes)
        {
            byte[] buffer = new byte[81920];
            int read;
            while (bytes > 0 && (read = input.Read(buffer, 0, Math.Min(buffer.Length, bytes))) > 0)
            {
                BaseStream.Write(buffer, 0, read);
                bytes -= read;
            }
        }
        public void CopyFrom(Stream input, long bytes)
        {
            var buffer = new byte[81920];
            int read;
            while (bytes > 0 && (read = input.Read(buffer, 0, (int)Math.Min(buffer.Length, bytes))) > 0)
            {
                BaseStream.Write(buffer, 0, read);
                bytes -= read;
            }
        }
    }
}
