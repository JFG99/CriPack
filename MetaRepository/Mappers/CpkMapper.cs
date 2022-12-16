using CriPakInterfaces;
using CriPakInterfaces.IComponents;
using CriPakInterfaces.Models;
using CriPakInterfaces.Models.Components;
using CriPakRepository.Helpers;
using System.Collections.Generic;
using System.Linq;


namespace MetaRepository.Mappers
{
    public class CpkMapper : Mapper, IDetailMapper<CpkMeta>
    {
        public CpkMeta Map(IDisplayList header, IEnumerable<Row> rowValue)
        {
            var CpkMeta = MapSection(header, 29);
            CpkMeta.Id = 0;
            var value = Map(header.Packet, CpkMeta.MetaData.EndColumnOffset);
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
        public CpkMeta Map(ISection header, IEnumerable<Row> rowValue)
        {
            var CpkMeta = MapSection(header, 29);
            CpkMeta.Id = 0;
            var value = Map(header.Packet, CpkMeta.MetaData.EndColumnOffset);
            return new CpkMeta()
            {
                Columns = value.Columns,
                Rows = value.Rows,
                Packet = value.Packet,
                Offset = 0x10,
                PacketLength = (ulong)(0x10 + value.Packet.PacketBytes.Count()),
                Files = value.Rows.GetModifierWhere<IUint32, uint>(x => x.Name == "Files"),
                Align = value.Rows.GetModifierWhere<IUint16, ushort>(x => x.Name == "Align"),
            };
        }
    }
}
