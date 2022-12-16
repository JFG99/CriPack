using CriPakInterfaces;
using CriPakInterfaces.Models.Components;
using CriPakRepository.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace MetaRepository.Mappers
{
    public class Mapper 
    {
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
    }
}