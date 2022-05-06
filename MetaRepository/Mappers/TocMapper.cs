using CriPakInterfaces;
using CriPakInterfaces.IComponents;
using CriPakInterfaces.Models.Components;
using CriPakInterfaces.Models.Components2;
using CriPakRepository.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetaRepository.Mappers
{
    public class TocMapper : Mapper, IDetailMapper<TocHeader>
    {
        public TocHeader Map(IEntity header, IEnumerable<CriPakInterfaces.Models.ComponentsNew.Row> rowValue)  
        {
            var offsetRowData = rowValue.Where(x => x.Name.Contains("Offset")).FirstOrDefault();
            var packet = (IOriginalPacket)header.Packet;
            packet.MakeDecyrpted();       
            var RowsOffset = (int)packet.ReadBytesFrom(8, 4, true) + 8;
            var StringsOffset = (int)packet.ReadBytes(4) + 8;
            var values = Map(header, StringsOffset + RowsOffset + 7);          
            
            return new TocHeader()
            {
                Columns = values.Columns,
                Rows = values.Rows,
                Packet = packet,
                PacketLength = header.Packet.PacketBytes.Count(),
                MetaOffsetPosition = offsetRowData.RowOffset,
                PackageOffsetPosition = (ulong)offsetRowData.Modifier.ReflectedValue("Value")            
            };
        }
    }
}