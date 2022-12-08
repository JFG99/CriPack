using CriPakInterfaces.IComponents;
using CriPakInterfaces.Models;
using CriPakInterfaces.Models.Components.Enums;
using CriPakRepository.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace CriPakRepository.Mappers
{
    public static class TocRowDisplay
    {
        public static IEnumerable<DisplayList> MapTocRowsToDisplay(this ITocHeader header)
        {
            var displayList = new List<DisplayList>();
            header.Rows.GroupBy(x => x.Id).ToList().ForEach(x =>
            {
                displayList.Add(new DisplayList
                {
                    Id = x.Key + 1,
                    FileName = x.Where(y => y.Name == "FileName").First().StringName,
                    Offset = x.GetModifierWhere<IUint64, ulong>(y => y.Name == "FileOffset") + 0x800, // This is the header offset of 2048.  
                    ArchiveLength = x.GetModifierWhere<IUint32, uint>(y => y.Name == "FileSize"),
                    ExtractedLength = x.GetModifierWhere<IUint32, uint>(y => y.Name == "ExtractSize"),
                    Type = ItemType.FILE
                }) ;
            });
            return displayList;
        }
    }
}
