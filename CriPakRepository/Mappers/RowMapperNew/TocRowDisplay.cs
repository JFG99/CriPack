using CriPakInterfaces.Models;
using CriPakInterfaces.Models.Components;
using CriPakInterfaces.Models.Components2;
using CriPakRepository.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CriPakRepository.Mappers
{
    public static class TocRowDisplay
    {
        public static IEnumerable<DisplayList> MapTocRowsToDisplay(this TocHeader header)
        {
            var displayList = new List<DisplayList>();
            header.Rows.GroupBy(x => x.Id).ToList().ForEach(x =>
            {
                var size = int.TryParse(x.Where(y => y.Name == "FileSize").First().Modifier.ReflectedValue("Value").ToString(), out var numSize) ? numSize : 0;
                var extractedSize = int.TryParse(x.Where(y => y.Name == "ExtractSize").First().Modifier.ReflectedValue("Value").ToString(), out var numExtract) ? numExtract : 0;
                displayList.Add(new DisplayList
                {
                    Id = x.Key + 1,
                    DisplayName = x.Where(y => y.Name == "FileName").First().StringName,
                    PackageOffset = (ulong)x.Where(y => y.Name == "FileOffset").First().Modifier.ReflectedValue("Value") + 0x800,// This is the header offset of 2048.  
                    Size = size,
                    ExtractedSize = extractedSize,
                    Type = "FILE",
                    Percentage = size / (float)extractedSize * 100
                });
            });
            return displayList;
        }
    }
}
