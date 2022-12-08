using CriPakInterfaces;
using CriPakInterfaces.Models.Components;
using CriPakRepository.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MetaRepository.Mappers
{
    public class ContentMapper : IDetailMapper<ContentHeader>
    {
        public ContentHeader Map(IDisplayList header, IEnumerable<Row> rowValue)
        {
            var size = Convert.ToInt64(rowValue.Where(x => x.Name.Contains("Size")).FirstOrDefault().Modifier.ReflectedValue("Value"));
            var offsetRowData = rowValue.Where(x => x.Name.Contains("Offset")).FirstOrDefault();
            return new ContentHeader()
            {
                Id = 0,
                PacketLength = size,
                MetaOffsetPosition = offsetRowData.RowOffset,
                PackageOffsetPosition = (ulong)offsetRowData.Modifier.ReflectedValue("Value")
            };
        }
    }
}
