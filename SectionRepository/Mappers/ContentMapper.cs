using CriPakInterfaces;
using CriPakInterfaces.IComponents;
using CriPakInterfaces.Models.Components;
using CriPakRepository.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SectionRepository.Mappers
{
    public class ContentMapper : IDetailMapper<ContentHeader>, IDetailMapper2<Section>
    {
        public ContentHeader Map(IDisplayList header, IEnumerable<Row> rowValue)
        {
            return new ContentHeader()
            {
                Id = 0,
                PacketLength = rowValue.GetModifierWhere<IUint64, ulong>(x => x.Name.Contains("Size")),
                MetaOffsetPosition = rowValue.Where(x => x.Name.Contains("Offset")).FirstOrDefault().RowOffset,
                PackageOffsetPosition = rowValue.GetModifierWhere<IUint64, ulong>(x => x.Name.Contains("Offset"))
            };
        }

        public Section Map(IPacket packet, IEnumerable<Row> rowValue)
        {
            var content = new Section()
            {
                Name = "CONTENT"
            };
            return content;
        }
    }
}
