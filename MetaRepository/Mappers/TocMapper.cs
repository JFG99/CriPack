using CriPakInterfaces;
using CriPakInterfaces.IComponents;
using CriPakInterfaces.Models.Components;
using CriPakRepository.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace MetaRepository.Mappers
{
    public class TocMapper : Mapper, IDetailMapper<TocHeader>
    {
        public TocHeader Map(IDisplayList header, IEnumerable<Row> rowValue)  
        {
            var values = Map(header, 0);

            return new TocHeader()
            {
                Columns = values.Columns,
                Rows = values.Rows,
                Packet = header.Packet,
                PacketLength = (ulong)header.Packet.PacketBytes.Count(),
                MetaOffsetPosition = rowValue.Where(x => x.Name.Contains("Offset")).FirstOrDefault().RowOffset,
                PackageOffsetPosition = rowValue.GetModifierWhere<IUint64, ulong>(x => x.Name.Contains("Offset")) //  .Where(x => x.Name.Contains("Offset")).Select(x => x.Modifier).OfType<IUint64>().First().Value            
            };
        }
    }
}