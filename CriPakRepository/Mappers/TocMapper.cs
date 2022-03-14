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
    public class TocMapper : Mapper, IDetailMapper<TocHeader>
    {
        public TocHeader Map(IEntity header, IRowValue rowValue)  
        {
            var packet = (IOriginalPacket)header.Packet;
            packet.MakeDecyrpted();       
            var RowsOffset = (int)packet.ReadBytesFrom(8, 4, true) + 8;
            var StringsOffset = (int)packet.ReadBytes(4) + 8;
            var values = Map(header, StringsOffset + RowsOffset + 7);          
            
            return new TocHeader()
            {
                Columns = values.Columns,
                Rows = values.Rows,
                Packet = packet,
                PacketLength = header.Packet.PacketBytes.Count(),
                MetaOffsetPosition = rowValue.Position,
                PackageOffsetPosition = rowValue.Value            
            };

        }
    }
}
//var offset = 0;
//var stringOffset = 0;
//var dataOffset = 0;
//var rows = fileHeaderData.Select(x => x.Columns
//.Select((y) =>
//{
//    var row = ByteConverter.MapBytes[y.Mask](x.Bytes, (y.Mask == 0xA) ? stringOffset : ((y.Mask == 0xB) ? dataOffset : offset));
//    row.Id = x.Id;
//    row.Name = y.Name;
//    var newOffset = row.Length;
//    if (y.Mask == 0xA)
//    {
//        stringOffset = row.Length;
//        newOffset = 4;
//    }
//    else if (y.Mask == 0xB)
//    {
//        dataOffset = row.Length;
//        newOffset = 4;
//    }
//    offset += newOffset;
//    return row;
//})).First().ToList();
////var rows = rowst.First();