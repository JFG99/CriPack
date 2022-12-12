using CriPakInterfaces;
using CriPakInterfaces.IComponents;
using CriPakInterfaces.Models.Components;
using CriPakRepository.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetaRepository.Mappers
{
    public class EtocMapper : Mapper, IDetailMapper<EtocHeader>
    {
        public EtocHeader Map(IDisplayList header, IEnumerable<Row> rowValue)
        {
            var values = Map(header.Packet, (int)header.Packet.ReadBytesFrom(4, 4, true));     
            return new EtocHeader()
            {
                Columns = values.Columns,
                Rows = values.Rows,
                Packet = header.Packet,
                PacketLength = (ulong)header.Packet.PacketBytes.Count(),
                MetaOffsetPosition = rowValue.Where(x => x.Name.Contains("Offset")).FirstOrDefault().RowOffset,
                PackageOffsetPosition = rowValue.GetModifierWhere<IUint64, ulong>(x => x.Name.Contains("Offset"))
            };
        }
    }
}
