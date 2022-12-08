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
                    Offset = x.Where(y => y.Name == "FileOffset").Select(y => y.Modifier).OfType<IUint64>().First().Value + 0x800,// This is the header offset of 2048.  
                    ArchiveLength = x.Where(y => y.Name == "FileSize").Select(y => y.Modifier).OfType<IUint32>().First().Value,                    
                    ExtractedLength = x.Where(y => y.Name == "ExtractSize").Select(y => y.Modifier).OfType<IUint32>().First().Value,
                    Type = ItemType.FILE
                }) ;
            });
            return displayList;
        }
    }
}
