using CriPakInterfaces;
using CriPakInterfaces.IComponents;
using CriPakInterfaces.Models;
using CriPakInterfaces.Models.Components;
using CriPakRepository.Helpers;
using System.Collections.Generic;
using System.Linq;


namespace SectionRepository.Mappers
{
    public class CpkMapper : Mapper, IDetailMapper<CpkMeta>, IDetailMapper2<Section>
    {
        public CpkMeta Map(IDisplayList header, IEnumerable<Row> rowValue)
        {
            //var CpkMeta = MapSection(header, 29);
            //CpkMeta.Id = 0;
            var value = Map(header.Packet, 29);
            return new CpkMeta()
            {
                Columns = value.Columns,
                Rows = value.Rows,
                Packet = value.Packet,
                Offset = 0x10,
                PacketLength = (ulong)(0x10 + value.Packet.PacketBytes.Count()),
                Files = value.Rows.GetModifierWhere<IUint32, uint>(x => x.Name == "Files") ,
                Align = value.Rows.GetModifierWhere<IUint16, ushort>(x => x.Name == "Align"),
            };
        }
        public Section Map(IPacket packet, IEnumerable<Row> rowValue)
        {
            var CpkMeta = MapSection(packet, 29);
            return CpkMeta;                     
        }
    }
}
