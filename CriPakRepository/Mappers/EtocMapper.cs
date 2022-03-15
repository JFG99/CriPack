using CriPakInterfaces;
using CriPakInterfaces.IComponents;
using CriPakInterfaces.Models.Components;
using CriPakInterfaces.Models.Components2;
using CriPakRepository.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CriPakRepository.Mappers
{
    public class EtocMapper : Mapper, IDetailMapper<EtocHeader>
    {
        public EtocHeader Map(IEntity header, CriPakInterfaces.Models.ComponentsNew.Row rowValue)
        {
            var packet = (IOriginalPacket)header.Packet;
            packet.MakeDecyrpted();
            var values = Map(header, (int)packet.ReadBytesFrom(4, 4, true));     
            return new EtocHeader()
            {
                Columns = values.Columns,
                Rows = values.Rows,
                Packet = header.Packet,
                PacketLength = header.Packet.PacketBytes.Count(),
                MetaOffsetPosition = rowValue.RowOffset,
                PackageOffsetPosition = (ulong)rowValue.Modifier.ReflectedValue("Value")
            };
        }
    }
}
