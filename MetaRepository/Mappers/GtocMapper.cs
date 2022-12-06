using CriPakInterfaces;
using CriPakInterfaces.IComponents;
using CriPakInterfaces.Models.Components;
using CriPakInterfaces.Models.Components;
using CriPakRepository.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetaRepository.Mappers
{
    public class GtocMapper : Mapper, IDetailMapper<GtocHeader>
    {
        public GtocHeader Map(IDisplayList header, IEnumerable<Row> rowValue) 
        {
            var offsetRowData = rowValue.Where(x => x.Name.Contains("Offset")).FirstOrDefault();
            var packet = (IOriginalPacket)header.Packet;            
            return new GtocHeader()
            {
                Packet = packet,
                PacketLength = header.Packet.PacketBytes.Count(),
                MetaOffsetPosition = offsetRowData.RowOffset,
                PackageOffsetPosition = (ulong)offsetRowData.Modifier.ReflectedValue("Value")
            };            
        }
    }
}
