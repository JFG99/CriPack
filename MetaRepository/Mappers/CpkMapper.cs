using CriPakInterfaces;
using CriPakInterfaces.IComponents;
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
            var value = Map(header.Packet, (int)header.Packet.ReadBytesFrom(4, 4, true) - 21);
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
    }
}
