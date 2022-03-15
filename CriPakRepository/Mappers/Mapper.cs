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
    public class Mapper 
    {
        public Meta Map(IEntity header, int aggregateModifier)
        {
            var packet = (IOriginalPacket)header.Packet;
            packet.MakeDecyrpted();
            var TableSize = (int)packet.ReadBytesFrom(4, 4, true);
            // CPK Header & UTF Header are ignored, so add 8 to each offset
            var RowsOffset = (int)packet.ReadBytes(4) + 8;
            var StringsOffset = (int)packet.ReadBytes(4) + 8;
            var DataOffset = (int)packet.ReadBytes(4) + 8;
            var TableName = (int)packet.ReadBytes(4);
            var NumColumns = (short)packet.ReadBytes(2);
            var RowLength = (short)packet.ReadBytes(2);
            var NumRows = (int)packet.ReadBytes(4);
            var NullSpacer = 7;//+ 7 for <NULL>. spacer


            var headerBytes = packet.GetBytes(NumColumns * 5).ToList();
            var skip = headerBytes.Where((x, i) => i % 5 == 0).ToList().FindIndex(x => x == 0);//locate block data where byte 0 == 0
            while (skip > -1)//iterate and delete 4 byte blocks where byte[0] == 0, then append 4 more bytes each time
            {
                headerBytes.RemoveRange(skip * 5, 4);
                headerBytes.AddRange(packet.GetBytes(4));
                skip = headerBytes.Where((x, i) => i % 5 == 0).ToList().FindIndex(x => x == 0);
            }//This gets us a corrected byte header list where byte[0] is flag and byte 1-4 is padding size.

            var columnLocations = headerBytes.ParseColumnLocations(aggregateModifier, StringsOffset);
            var columns = headerBytes.GetColumns(columnLocations, packet);

#if DEBUG // For Debugging, these extra data break values are useful, but splitting the annonymous linq requires more reflection and takes an extra 1-2 seconds.  
            var byteRows = packet.GetByteRows(RowsOffset, RowLength, StringsOffset);
            var rowMeta = byteRows.GetRowMeta(columns, RowsOffset, RowLength);
            var stringData = rowMeta.SelectMany(x => x).GetStringData(packet, DataOffset, StringsOffset);
            var rows = rowMeta.SelectMany(x => x).Merge(stringData);
#else
            var rows = packet.GetRows(columns, RowsOffset, RowLength, DataOffset, StringsOffset);
#endif
                       
            return new Meta()
            {
                Rows = rows,
                Columns = columns,
                Packet = header.Packet
            };
        }
    }
}