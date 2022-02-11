using CriPakInterfaces.Models;
using CriPakInterfaces.Models.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CriPakRepository.Mappers
{
    public static class TocRowMapper
    {
        public static void MapTocData(this CriPak package)
        {
            var fTocOffset = package.TocOffset;
            if (fTocOffset > 0x800)
            {
                fTocOffset = 0x800;
            }
            var tempList = package.Utf.Rows.Where(x => x.Name == "FileName")
                .Select(x => new CriFile
                {
                    FileId = x.Id,
                    FileName = x.str,
                    FileType = "FILE",
                    Offset = (package.TocOffset < 0 || package.ContentOffset < fTocOffset) ? package.ContentOffset : fTocOffset
                }).ToList();

            var dirNameRowList = package.Utf.Rows.Where(x => x.Name == "DirName");
            var userString = package.Utf.Rows.Where(x => x.Name == "UserString");
            if (dirNameRowList.Any())
            {
                tempList = tempList.Join(dirNameRowList, t => t.FileId, dn => dn.Id, (t, dn) =>
                {
                    t.DirName = dn.str;
                    return t;
                }).ToList();
            }
            if (userString.Any())
            {
                tempList = tempList.Join(userString, t => t.FileId, us => us.Id, (t, us) =>
                {
                    t.UserString = us.str;
                    return t;
                }).ToList();
            }

            package.CriFileList.AddRange(tempList.Join(package.Utf.Rows.Where(x => x.Name == "FileSize"), t => t.FileId, fs => fs.Id, (t, fs) =>
            {
                t.FileSize = int.TryParse(fs?.GetValue()?.ToString(), out var num) ? num : 0;
                t.FileSizePos = fs.Position;
                t.FileSizeType = fs?.GetType();
                return t;
            }).Join(package.Utf.Rows.Where(x => x.Name == "ExtractSize"), t => t.FileId, es => es.Id, (t, es) =>
            {
                t.ExtractedFileSize = int.TryParse(es?.GetValue()?.ToString(), out var num) ? num : 0;
                t.ExtractSizePos = es.Position;
                t.ExtractSizeType = es?.GetType();
                return t;
            }).Join(package.Utf.Rows.Where(x => x.Name == "FileOffset"), t => t.FileId, fo => fo.Id, (t, fo) =>
            {
                t.FileOffset = fo.uint64 + t.Offset;
                t.FileOffsetPos = fo.Position;
                t.FileOffsetType = fo?.GetType();
                return t;
            }).ToList());
        }
    }
}
