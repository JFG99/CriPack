using CriPakInterfaces;
using CriPakInterfaces.IComponents;
using CriPakInterfaces.Models.Components;
using CriPakRepository.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SectionRepository.Mappers
{
    public class EtocMapper : Mapper, IDetailMapper<Section>
    {
        public Section Map(IPacket packet, IEnumerable<Row> rowValue)
        {
            var section = MapSection(packet, (int)packet.ReadBytesFrom(4, 4, false));
            section.Offset = (long)rowValue.GetModifierWhere<IUint64, ulong>(x => x.Name.Contains("Offset"));
            return section;
        }
    }
}
