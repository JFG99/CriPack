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
    public class ContentMapper : IDetailMapper<ContentHeader>
    {
        public ContentHeader Map(IEntity header, IEnumerable<CriPakInterfaces.Models.ComponentsNew.Row> rowValue)
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
