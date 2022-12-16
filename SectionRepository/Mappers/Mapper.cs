using CriPakInterfaces;
using CriPakInterfaces.Models;
using CriPakInterfaces.Models.Components;
using CriPakRepository.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace SectionRepository.Mappers
{
    public class Mapper 
    {
        //EndColumnOffset is a struggle.  I have so far been unable to identify a universal formula.
        //For now some tables require extra manual specifiers.
        public Meta Map(IPacket packet, int endColumnOffset) 
        {
            packet.MakeDecrypted();
            var TableSize = (int)packet.ReadBytesFrom(4, 4, true);
            // CPK Header & UTF Header are ignored, so add 8 to each primary offset
            var RowsOffset = (int)packet.ReadBytes(4) + 8;
            var StringsOffset = (int)packet.ReadBytes(4) + 8;
            var DataOffset = (int)packet.ReadBytes(4) + 8;
            var TableNameOffset = (int)packet.ReadBytes(4) + StringsOffset;
            var NumColumns = (short)packet.ReadBytes(2);
            var RowLength = (short)packet.ReadBytes(2);
            var NumRows = (int)packet.ReadBytes(4);
            if (endColumnOffset == 0)
            {
                endColumnOffset = DataOffset - (TableSize - StringsOffset - RowsOffset);
            }
            var columnBytes = packet.GetBytes(RowsOffset - 32).ToList();
            var columns = columnBytes.ParseSegments(new List<Column>()).ThenData(packet, endColumnOffset, StringsOffset).ToList();                      
            var rows = packet.GetRows(columns, RowsOffset, RowLength, DataOffset, StringsOffset);

            return new Meta()
            {
                Rows = rows,
                Columns = columns,
                Packet = packet
            };
        }

        public Section MapSection(IPacket packet, int adjustment = 0)
        {
            //var packet = header.Packet;
            packet.MakeDecrypted();
            _ = (int)packet.ReadBytes(4); // Encoding 
            var sectionMeta = new SectionMeta()
            {
                TableSize = (int)packet.ReadBytes(4),
                RowOffset = (int)packet.ReadBytes(4) + 8,
                ColumnNamesOffset = (int)packet.ReadBytes(4) + 8,
                DataOffset = (int)packet.ReadBytes(4) + 8,
                SpacerLength = (int)packet.ReadBytes(4),
                NumColumns = (short)packet.ReadBytes(2),
                RowLength = (short)packet.ReadBytes(2),
                NumRows = (int)packet.ReadBytes(4),
                ColumnOffset = packet.GetReadOffset(),
                //Some tables have weirdness with the ending and need a
                //defined adjustment value to get proper locations. 
                EndColumnAdjustment = adjustment
            };

            var columns = packet.GetBytes(sectionMeta.RowOffset - 32)
                .ToList()
                .ParseSegments(new List<Column>())
                .ThenData(packet, sectionMeta.EndColumnOffset, sectionMeta.ColumnNamesOffset)
                .ToList();           

            var sectionHeader = new SectionHeader()
            {
                Columns = columns,
                Rows = packet.GetRows(columns, sectionMeta).ToList()
            };

            return new Section()
            {
                //Name = header.SelectionName, 
                MetaData = sectionMeta,
                HeaderData = sectionHeader
            };
        }
    }
}