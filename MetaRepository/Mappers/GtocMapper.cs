using CriPakInterfaces;
using CriPakInterfaces.IComponents;
using CriPakInterfaces.Models.Components;
using CriPakRepository.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetaRepository.Mappers
{
    public class GtocMapper : Mapper, IDetailMapper<GtocHeader>
    {
        public GtocHeader Map(IDisplayList header, IEnumerable<Row> rowValue) 
        {
            //Im not sure if 154 is accurate to all GTOC.  It is for my testing, but GTOC doesn't seem to have any need for parsing.
            var values = Map(header.Packet, 154);
            GetGtocInnerTables(header.Packet, values.Rows);
            return new GtocHeader()
            {
                Columns = values.Columns,
                Rows = values.Rows,
                Packet = header.Packet,
                PacketLength = (ulong)header.Packet.PacketBytes.Count(),
                MetaOffsetPosition = rowValue.Where(x => x.Name.Contains("Offset")).FirstOrDefault().RowOffset,
                PackageOffsetPosition = rowValue.GetModifierWhere<IUint64, ulong>(x => x.Name.Contains("Offset"))
            };            
        }

        //Just going to leave this here.  Each inner GTOC table has speciallized headers.  GLink and Attr, don't really seem necessary, and FLink has some kind of sequential file link in it. 
        //None of this should be impacted by changing file sizes.  Pretty sure the old application simply replaced the hex data with 00 for GTOC anyways.  
        private void GetGtocInnerTables(IPacket packet, IEnumerable<Row> rowValues)
        {
            var segments = rowValues.Skip(3)
                .Select(x => x.ByteSegment
                              .Select((y, i) => new { Index = i, Value = y })
                              .GroupBy(y => y.Index/4)
                       ).Select(x => new
                       {
                           Offset = BitConverter.ToInt32(x.Where(y => y.Key == 0).SelectMany(y => y.Select(z => z.Value).Reverse().ToArray()).ToArray(), 0),
                           Length = BitConverter.ToInt32(x.Where(y => y.Key == 1).SelectMany(y => y.Select(z => z.Value).Reverse().ToArray()).ToArray(), 0)
                       }
                       ).ToList();
            var tables = segments.Select(x => new Packet(packet.GetBytesFrom(x.Offset + 160, x.Length)));                  
            return;
        }
    }
}
