using CriPakInterfaces;
using CriPakInterfaces.IComponents;
using CriPakInterfaces.Models;
using CriPakInterfaces.Models.Components2;
using CriPakRepository.Helpers;
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
        where TOut : IDisplayList, new()
    {
        private readonly TMapper _mapper;
        private readonly IDisplayList _header;
        public IEndianReader Stream { get; set; }
        public string SelectionName => _header.SelectionName;
        public long CurrentPosition { get; set; }
        private IEnumerable<byte> Buffer { get; set; }

        public ReaderDetailRepository(TMapper mapper, TOut header)
        {
            _mapper = mapper;
            _header = header;
        }

        public TOut Get(string inFile, IEnumerable<CriPakInterfaces.Models.ComponentsNew.Row> rowValue)
        {
            CurrentPosition = Convert.ToInt64(rowValue?.FirstOrDefault(x => x.Name.Contains("Offset"))?.Modifier.ReflectedValue("Value") ?? 0);
            if (ValidatePacketName(inFile, _header.ValidationName))
            {
                _header.Packet = GetPacket();
            }
            return _mapper.Map(_header, rowValue);
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
            CurrentPosition = Stream.BaseStream.Position;
            Stream.Close();
            return original;
        }
        public bool ValidatePacketName(string inFile, string name)
        {
            Stream = new EndianReader<FileStream, EndianData>(System.IO.File.OpenRead(inFile), new EndianData(true));
            GetBuffer();
            if (Encoding.UTF8.GetString(Buffer.ToArray()) != name)
            {
                Stream.Close();
                return false;
            }
            return true;
        }

        private void GetBuffer(int count = 4)
        {
            Stream.BaseStream.Position = CurrentPosition;
            Buffer = Stream.ReadBytes(count);
        }

    }
}
