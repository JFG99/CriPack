using CriPakInterfaces;
using CriPakInterfaces.IComponents;
using CriPakInterfaces.Models;
using CriPakInterfaces.Models.Components;
using CriPakRepository.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SectionRepository.Mappers
{
    public class ContentMapper : IDetailMapper<Section>
    {
        public Section Map(IPacket packet, IEnumerable<Row> rowValue)
        {
            var content = new Section()
            {
                Name = "CONTENT",
                Offset = (long)rowValue.GetModifierWhere<IUint64, ulong>(x => x.Name.Contains("Offset")),
                MetaData = new SectionMeta() { TableSize = (long)rowValue.GetModifierWhere<IUint64, ulong>(x => x.Name.Contains("Size")) }
            };
            return content;
        }
    }
}
