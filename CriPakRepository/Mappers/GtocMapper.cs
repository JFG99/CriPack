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
    public class GtocMapper : Mapper, IDetailMapper<GtocHeader>
    {
        public GtocHeader Map(IEntity header, IRowValue rowValue) 
        {
            var packet = (IOriginalPacket)header.Packet;
            
            return new GtocHeader()
            {
                Packet = packet,
                PacketLength = header.Packet.PacketBytes.Count(),
                MetaOffsetPosition = rowValue.Position,
                PackageOffsetPosition = rowValue.Value
            };            
        }
    }
}
