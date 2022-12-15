using CriPakInterfaces;
using CriPakInterfaces.IComponents;
using CriPakInterfaces.Models.Components;
using CriPakRepository.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MetaRepository.Mappers
{
    public class TocMapper : Mapper, IDetailMapper<TocHeader>
    {
        public TocHeader Map(IDisplayList header, IEnumerable<Row> rowValue)  
        {
            var values = Map(header.Packet, 0);


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
                        Name =  x.Name,
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
                    Rows = rowValue.Select((x,i) => new Row()
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

            return new TocHeader()
            {
                Columns = values.Columns,
                Rows = values.Rows,
                Packet = header.Packet,
                PacketLength = (ulong)header.Packet.PacketBytes.Count(),
                MetaOffsetPosition = rowValue.Where(x => x.Name.Contains("Offset")).FirstOrDefault().RowOffset,
                PackageOffsetPosition = rowValue.GetModifierWhere<IUint64, ulong>(x => x.Name.Contains("Offset"))            
            };
        }
    }
}