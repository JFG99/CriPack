using CriPakInterfaces;
using CriPakInterfaces.IComponents;
using CriPakInterfaces.Models;
using CriPakInterfaces.Models.Components;
using CriPakRepository.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CriPakRepository.Repositories
{
    public class DetailRepository<TMapper, TOut> : IDetailRepository
        where TMapper : IDetailMapper2<TOut>
        where TOut : ISection, new()
    {
        private readonly TMapper _mapper;
        private readonly ISection _header;
        public IEndianReader Stream { get; set; }
        public string SelectionName => _header.Name;
        public long CurrentPosition { get; set; }
        private IEnumerable<byte> Buffer { get; set; }

        public DetailRepository(TMapper mapper, ISection header)
        {
            _mapper = mapper;
            _header = header;
        }

        public TOut Get(string inFile, IEnumerable<Row> rowValue, string validate)
        {
            CurrentPosition = rowValue?.FirstOrDefault(x => x.Name.Contains("Offset"))?.Modifier is IUint64 position ?  Convert.ToInt64(position.Value) :  0;
            if (ValidatePacketName(inFile, validate))
            {
                return _mapper.Map(GetPacket(), rowValue); 
            }
            return default(TOut);
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
