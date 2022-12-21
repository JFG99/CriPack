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
                SpacerLength = (int)packet.ReadBytes(4) + 1,
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
                MetaData = sectionMeta,
                HeaderData = sectionHeader
            };
        }
    }
}