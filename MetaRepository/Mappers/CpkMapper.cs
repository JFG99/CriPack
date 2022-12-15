using CriPakInterfaces;
using CriPakInterfaces.IComponents;
using CriPakInterfaces.Models;
using CriPakInterfaces.Models.Components;
using CriPakRepository.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace MetaRepository.Mappers
{
    public class CpkMapper : Mapper, IDetailMapper<CpkMeta>
    {
        public CpkMeta Map(IDisplayList header, IEnumerable<Row> rowValue)
        {
            var MetaData = MapMeta(header.Packet);
            var value = Map(header.Packet, (int)header.Packet.ReadBytesFrom(4, 4, true) - 21);
            var HeaderData = MapHeader(value);
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
        public SectionMeta MapMeta(IPacket packet)
        {
            packet.MakeDecrypted();
            _ = (int)packet.ReadBytes(4); // Encoding 
            return new SectionMeta()
            {
                TableSize = (int)packet.ReadBytes(4),
                ColumnOffset = 32,
                RowOffset = (int)packet.ReadBytes(4) + 8,
                ColumnNamesOffset = (int)packet.ReadBytes(4) + 8,
                DataOffset = (int)packet.ReadBytes(4) + 8,
                SpacerLength = (int)packet.ReadBytes(4),
                NumColumns = (short)packet.ReadBytes(2),
                RowLength = (short)packet.ReadBytes(2),
                NumRows = (int)packet.ReadBytes(4),
            };                  

        }

        public SectionHeader MapHeader(Meta value)
        {
            return new SectionHeader()
            {
                Columns = value.Columns,
                Rows = value.Rows
            };

        }
    }
}
