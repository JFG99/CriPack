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
            var offsetRowData = rowValue.Where(x => x.Name.Contains("Offset")).FirstOrDefault();
            var packet = (IOriginalPacket)header.Packet;
            packet.MakeDecyrpted();
            var values = Map(header, (int)packet.ReadBytesFrom(4, 4, true));     
            return new EtocHeader()
            {
                Columns = values.Columns,
                Rows = values.Rows,
                Packet = header.Packet,
                PacketLength = (ulong)header.Packet.PacketBytes.Count(),
                MetaOffsetPosition = offsetRowData.RowOffset,
                PackageOffsetPosition = (ulong)offsetRowData.Modifier.ReflectedValue("Value")
            };
        }
    }
}
