using CriPakInterfaces;
using CriPakInterfaces.IComponents;
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
            return new ContentHeader()
            {
                Id = 0,
                PacketLength = rowValue.GetModifierWhere<IUint64, ulong>(x => x.Name.Contains("Size")),
                MetaOffsetPosition = rowValue.Where(x => x.Name.Contains("Offset")).FirstOrDefault().RowOffset,
                PackageOffsetPosition = rowValue.GetModifierWhere<IUint64, ulong>(x => x.Name.Contains("Offset"))
            };
        }
    }
}
