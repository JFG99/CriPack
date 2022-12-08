using CriPakInterfaces;
using CriPakInterfaces.IComponents;
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
            var packet = (IOriginalPacket)header.Packet;            
            return new GtocHeader()
            {
                Packet = packet,
                PacketLength = (ulong)header.Packet.PacketBytes.Count(),
                MetaOffsetPosition = rowValue.Where(x => x.Name.Contains("Offset")).FirstOrDefault().RowOffset,
                PackageOffsetPosition = rowValue.GetModifierWhere<IUint64, ulong>(x => x.Name.Contains("Offset"))
            };            
        }
    }
}
