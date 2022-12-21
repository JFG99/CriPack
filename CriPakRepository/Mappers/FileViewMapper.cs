using CriPakInterfaces;
using CriPakInterfaces.IComponents;
using CriPakInterfaces.Models;
using CriPakInterfaces.Models.Components;
using CriPakInterfaces.Models.Components.Enums;
using CriPakRepository.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CriPakRepository.Mappers
{
    public static class FileViewMapper
    {
        public static IEnumerable<IFileViewer> MapToViewer(this IEnumerable<ISection> sections)
        {
            var viewList = new List<IFileViewer>();
            sections.ToList().ForEach(x =>
            {
                var entry = new FileViewer();
                if (x.HeaderData != null && x.HeaderData.Columns.Any(y => y.Name == "FileName"))
                {
                    x.HeaderData.Rows.GroupBy(y => y.Id).ToList().ForEach(y =>
                    {
                        entry = new FileViewer();
                        entry.Id = (int)y.GetModifierWhere<IUint32, uint>(z => z.Name == "ID");
                        entry.FileName = y.Where(z => z.Name == "FileName").First().StringName;
                        entry.Offset = (long)y.GetModifierWhere<IUint64, ulong>(z => z.Name == "FileOffset") + 0x800; // This is the header offset of 2048.  
                        entry.ArchiveLength = y.GetModifierWhere<IUint32, uint>(z => z.Name == "FileSize");
                        entry.ExtractedLength = y.GetModifierWhere<IUint32, uint>(z => z.Name == "ExtractSize");
                        entry.Type = ItemType.FILE;
                        viewList.Add(entry);
                    });
                }
                else
                {
                    entry.Offset = x.Offset; // This is the header offset of 2048.  
                    entry.ArchiveLength = (ulong)x.MetaData.TableSize;
                    entry.FileName = x.Name;
                    viewList.Add(entry);
                }
            });
            return viewList;
        }
    }
}
