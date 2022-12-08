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
                var size = int.TryParse(x.Where(y => y.Name == "FileSize").First().Modifier.ReflectedValue("Value").ToString(), out var numSize) ? numSize : 0;
                var test = x.Where(y => y.Name == "FileSize").First().Modifier;
                var extractedSize = int.TryParse(x.Where(y => y.Name == "ExtractSize").First().Modifier.ReflectedValue("Value").ToString(), out var numExtract) ? numExtract : 0;
                displayList.Add(new DisplayList
                {
                    Id = x.Key + 1,
                    FileName = x.Where(y => y.Name == "FileName").First().StringName,
                    Offset = (ulong)x.Where(y => y.Name == "FileOffset").First().Modifier.ReflectedValue("Value") + 0x800,// This is the header offset of 2048.  
                    ArchiveLength = size,                    
                    ExtractedLength = extractedSize,
                    Type = ItemType.FILE
                }) ;
            });
            return displayList;
        }
    }
}
