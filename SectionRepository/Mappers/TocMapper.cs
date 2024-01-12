using CriPakInterfaces;
using CriPakInterfaces.IComponents;
using CriPakInterfaces.Models.Components;
using CriPakRepository.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SectionRepository.Mappers
{
    public class TocMapper : Mapper, IDetailMapper<Section>
    {        
        public Section Map(IPacket packet, IEnumerable<Row> rowValue)
        {
            var tocSection = MapSection(packet);
            tocSection.Content = packet;
            tocSection.Offset = (long)rowValue.GetModifierWhere<IUint64, ulong>(x => x.Name.Contains("Offset"));
            //var testToc = tocSection.HeaderData.Rows.ToList().Where(x => x.StringName.Contains("SDIF_CAMP_OPTION"));
            return tocSection;
        }
    }
}