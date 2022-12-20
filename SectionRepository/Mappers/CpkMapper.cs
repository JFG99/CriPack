using CriPakInterfaces;
using CriPakInterfaces.Models.Components;
using System.Collections.Generic;


namespace SectionRepository.Mappers
{
    public class CpkMapper : Mapper, IDetailMapper<Section>
    {
        public Section Map(IPacket packet, IEnumerable<Row> rowValue)
        {
            var cpkMeta = MapSection(packet, 29);
            return cpkMeta;                     
        }
    }
}
