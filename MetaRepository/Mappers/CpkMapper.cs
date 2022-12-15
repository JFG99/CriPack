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

            var test = new Section()
            {
                Id = header.Id,
                Name = header.FileName,
                MetaData = new Meta()
                {
                    Id = header.Id,
                    Rows = rowValue.Select(x => new Row()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        StringName = x.StringName,
                        ByteSegment = x.ByteSegment,
                        RowOffset = x.RowOffset
                    })
                    //Name = rowValue.Where(x => x.Name.Contains("Offset")).FirstOrDefault().Name,
                    //Offset = rowValue.Where(x => x.Name.Contains("Offset")).FirstOrDefault().RowOffset
                },
                HeaderData = new Header()
                {
                    Id = header.Id,
                    Rows = rowValue.Select((x, i) => new Row()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        StringName = x.StringName,
                        ByteSegment = x.ByteSegment,
                        RowOffset = (int)rowValue.GetModifierWhere<IUint64, ulong>(i)
                    })
                    //Name = rowValue.Where(x => x.Name.Contains("Offset")).FirstOrDefault().Name,
                    //Offset = Convert.ToInt64(rowValue.GetModifierWhere<IUint64, ulong>(x => x.Name.Contains("Offset")))
                }

            };

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
            var test = new SectionMeta()
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
            // CPK Header & UTF Header are ignored, so add 8 to each primary offset
            var RowsOffset = (int)packet.ReadBytes(4) + 8;
            var StringsOffset = (int)packet.ReadBytes(4) + 8;
            var DataOffset = (int)packet.ReadBytes(4) + 8;
            var TableNameOffset = (int)packet.ReadBytes(4) + StringsOffset;
            var NumColumns = (short)packet.ReadBytes(2);
            var RowLength = (short)packet.ReadBytes(2);
            var NumRows = (int)packet.ReadBytes(4);

            return test;
        }
    }
}
