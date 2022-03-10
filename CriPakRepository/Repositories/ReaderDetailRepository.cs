using CriPakInterfaces;
using CriPakInterfaces.Models;
using CriPakInterfaces.Models.Components2;
using CriPakRepository.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CriPakRepository.Repositories
{
    public class ReaderDetailRepository<TMapper, TOut> : IReaderDetailRepository
        where TMapper : IDetailMapper<TOut>
        where TOut : IHeader, new()
    {
        private readonly TMapper _mapper;
        private readonly IHeader _header;
        public IEndianReader Stream { get; set; }
        private IEnumerable<byte> Buffer { get; set; }

        public ReaderDetailRepository(TMapper mapper, TOut header)
        {
            _mapper = mapper;
            _header = header;
        }

        public TOut Get(string inFile)
        {
            if (!ValidatePacketName(inFile, _header.ValidationName)) return default(TOut);
            _header.Packet = GetPacket();
            return _mapper.Map(_header);
        }

        public IPacket GetPacket()
        {
            Stream.IsLittleEndian = true;
            _ = Stream.ReadInt32();//Spacer 4 bytes.  
            var utfSize = Stream.ReadInt64();
            var original = new OriginalPacket()
            {
                PacketBytes = Stream.ReadBytes((int)utfSize)
            };
            Stream.IsLittleEndian = false;
            return original;
        }
        public bool ValidatePacketName(string inFile, string name) => ValidatePacketName(inFile, name, 0);

        public bool ValidatePacketName(string inFile, string name, int offset)
        {
            Stream = new EndianReader<FileStream, EndianData>(File.OpenRead(inFile), new EndianData(true));
            GetBuffer(offset);
            if (Encoding.UTF8.GetString(Buffer.ToArray()) != name)
            {
                Stream.Close();
                return false;
            }
            return true;
        }

        private void GetBuffer(int offset, int count = 4)
        {
            Stream.BaseStream.Position = offset;
            Buffer = Stream.ReadBytes(count);
        }

    }
}
