using CriPakInterfaces;
using CriPakInterfaces.Models.Components;
using System.Collections.Generic;


namespace SectionRepository.Mappers
{
    public class CpkMapper : Mapper, IDetailMapper<Section>
    {
        public Section Map(IPacket packet, IEnumerable<Row> rowValue)
        {
            var cpkSection = MapSection(packet, 29);
            cpkSection.Content = packet;
            return cpkSection;                     
        }
    }
}
