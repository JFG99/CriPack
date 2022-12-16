using CriPakInterfaces;
using CriPakInterfaces.IComponents;
using CriPakInterfaces.Models.Components;
using CriPakRepository.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SectionRepository.Mappers
{
    public class TocMapper : Mapper, IDetailMapper<TocHeader>, IDetailMapper2<Section>
    {
        public TocHeader Map(IDisplayList header, IEnumerable<Row> rowValue)  
        {
            //var TocSection = MapSection(header);
            //TocSection.Id = 1;
            var values = Map(header.Packet, 0);           

            return new TocHeader()
            {
                Columns = values.Columns,
                Rows = values.Rows,
                Packet = header.Packet,
                PacketLength = (ulong)header.Packet.PacketBytes.Count(),
                MetaOffsetPosition = rowValue.Where(x => x.Name.Contains("Offset")).FirstOrDefault().RowOffset,
                PackageOffsetPosition = rowValue.GetModifierWhere<IUint64, ulong>(x => x.Name.Contains("Offset"))            
            };
        }
        public Section Map(IPacket packet, IEnumerable<Row> rowValue)
        {
            var tocSection = MapSection(packet);
            return tocSection;
        }
    }
}