using CriPakInterfaces;
using CriPakInterfaces.Models;
using CriPakInterfaces.Models.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CriPakRepository.Mappers
{
    public static class HeaderMetaRowDisplay
    {
        public static IEnumerable<DisplayList> MapHeaderRowsToDisplay(this IEnumerable<IHeader> header)
        {
            var displayList = new List<DisplayList>();
            header.ToList().ForEach(x =>
            {
                displayList.Add(new DisplayList
                {
                    Id = 0,
                    DisplayName = x.DisplayName,
                    PackageOffset = x.PackageOffsetPosition,
                    Size = Convert.ToInt32(x.PacketLength),
                    ExtractedSize = 0,
                    Type = "HDR",
                    Percentage = 0
                });
            });
            return displayList;
        }
    }
}
