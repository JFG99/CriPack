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
            var packet = (IOriginalPacket)header.Packet;
            packet.MakeDecyrpted();
            var value = Map(header, (int)packet.ReadBytesFrom(4, 4, true) - 21);
            return new CpkMeta()
            {
                Columns = value.Columns,
                Rows = value.Rows,
                Packet = value.Packet,
                Offset = 0x10,
                PacketLength = (ulong)(0x10 + value.Packet.PacketBytes.Count()),
                Files = value.Rows.ToList().Where(x => x.Name == "Files").Select(x => x.Modifier).OfType<IUint32>().First()?.Value ?? 0,
                Align = value.Rows.ToList().Where(x => x.Name == "Align").Select(x => x.Modifier).OfType<IUint16>().First()?.Value ?? 0,
            };
        }
    }
}
