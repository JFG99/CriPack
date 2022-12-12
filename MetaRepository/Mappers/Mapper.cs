using CriPakInterfaces;
using CriPakInterfaces.IComponents;
using CriPakInterfaces.Models.Components;
using CriPakRepository.Helpers;
using System.Linq;
using System.Net.Sockets;

namespace MetaRepository.Mappers
{
    public class Mapper 
    {
        public Meta Map(IDisplayList header, int endColumnOffset)
        {
            return Map(header.Packet, endColumnOffset);
            
        }

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

            var columnBytes = packet.GetBytes(RowsOffset - 32).ToList();//Chunk of offsets that point to Column titles.  Add each to StringOffset to get specific locations.
            var skip = columnBytes.Where((x, i) => i % 5 == 0).ToList().FindIndex(x => x == 0);//locate block data where byte[0] == 0, because this is an empty column string and needs to be ignored.
            var skipCount = 0;// need to know how many are deleted for later.
            while (skip > -1)//iterate and delete 4 byte blocks where byte[0] == 0
            {
                columnBytes.RemoveRange(skip * 5, 4);
                skip = columnBytes.Where((x, i) => i % 5 == 0).ToList().FindIndex(x => x == 0);
                skipCount++;
            }//This gets us a corrected column offset lists where byte[0] is flag and byte[1..4] is padding size.

            // Some headers(the CPK META) are a little odd. The String Data offset from it is obtained from a mapping and isn't really obtainable through basic inspection.
            // That mapper supplies a specific value that is used to locate the end of the Column offset values.  Otherwise, we can figure out exactly how to find with this simple subtraction. 
            if (endColumnOffset == 0)
            {
                endColumnOffset = DataOffset - (TableSize - StringsOffset - RowsOffset + skipCount);
            }
            var columnLocations = columnBytes.ParseColumnLocations(endColumnOffset, StringsOffset);
            var columns = columnBytes.GetColumns(columnLocations, packet);
            var rows = packet.GetRows(columns, RowsOffset, RowLength, DataOffset, StringsOffset);

            //For Debugging, these extra data break values are useful.
            //var byteRows = packet.GetByteRows(RowsOffset, RowLength, StringsOffset);
            //var rowMeta = byteRows.GetRowMeta(columns, RowsOffset, RowLength);
            //var stringData = rowMeta.SelectMany(x => x).GetStringData(packet, DataOffset, StringsOffset);
            //var rows = rowMeta.SelectMany(x => x).Merge(stringData);

            return new Meta()
            {
                Rows = rows,
                Columns = columns,
                Packet = packet
            };
        }
    }
}