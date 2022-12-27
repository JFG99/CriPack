using CriPakInterfaces;
using CriPakInterfaces.IComponents;
using CriPakInterfaces.Models;
using CriPakInterfaces.Models.Components;
using CriPakRepository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CriPakRepository.Repositories
{
    public class DetailRepository<TMapper, TOut> : IDetailRepository
        where TMapper : IDetailMapper<TOut>
        where TOut : ISection, new()
    {
        private readonly TMapper _mapper;
        public IEndianReader Stream { get; set; }
        private string ArchivePath { get; set; }
        public long CurrentPosition { get; set; }
        private IEnumerable<byte> Buffer { get; set; }

        public DetailRepository(TMapper mapper)
        {
            _mapper = mapper;
        }

        public TOut Get(string inFile, IEnumerable<Row> rowValue, string validate)
        {
            ArchivePath = inFile;
            if (Stream == null || !Stream.IsOpen)
            {
                OpenStream();
            }
            CurrentPosition = rowValue?.FirstOrDefault(x => x.Name.Contains("Offset"))?.Modifier is IUint64 position ? Convert.ToInt64(position.Value) : 0;
            if (validate == "CONTENT" || ValidatePacketName(validate))
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

        public bool ValidatePacketName(string name)
        {
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

        private void OpenStream()
        {
            Stream = new EndianReader<FileStream, EndianData>(System.IO.File.OpenRead(ArchivePath), new EndianData(true));
        }
    }
}
