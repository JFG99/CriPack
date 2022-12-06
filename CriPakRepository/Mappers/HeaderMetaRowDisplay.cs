using CriPakInterfaces;
using CriPakInterfaces.Models;
using CriPakInterfaces.Models.Components;
using CriPakInterfaces.Models.Components.Enums;
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
                    FileName = x.FileName,
                    Offset = x.PackageOffsetPosition,
                    ArchiveLength = x.PacketLength,
                    Type = Category.HDR,
                });
            });
            return displayList;
        }
    }
}
