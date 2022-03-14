﻿using CriPakInterfaces;
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
    public class CpkMapper : Mapper, IDetailMapper<CpkMeta>
    {
        public CpkMeta Map(IEntity header, IRowValue rowValue)
        {
            var packet = (IOriginalPacket)header.Packet;
            packet.MakeDecyrpted();
            var value = Map(header, (int)packet.ReadBytesFrom(4, 4, true) - 29);
            return new CpkMeta()
            {
                Columns = value.Columns,
                Rows = value.Rows,
                Packet = value.Packet,
                Offset = 0x10,
                PacketLength = 0x10 + value.Packet.PacketBytes.Count(),
                Files = (uint)value.Rows.Where(x => x.Name == "Files").FirstOrDefault().Value,
                Align = (ushort)value.Rows.Where(x => x.Name == "Align").FirstOrDefault().Value,
            };
        }
    }
}
