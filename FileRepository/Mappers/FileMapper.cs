using CriPakInterfaces;
using CriPakInterfaces.IComponents;
using CriPakInterfaces.Models.Components;
using FileList = CriPakInterfaces.Models.Components.Files;
using CriPakRepository.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using CriPakInterfaces.Models;
using CriPakInterfaces.Models.Components.Enums;

namespace FileRepository.Mappers
{
    public class FileMapper : Mapper, IExtractorMapper<FileList>
    {
        public FileList Map(IEnumerable<IFileViewer> headers)
        {
            var etocOffset = headers.First(x => x.FileName.Equals("ETOC")).Offset;
            var toc = headers.Where(x => x.Type == ItemType.FILE);
            var tocFiles = toc.Select(x =>
            {
                return new File()
                {
                    Id = x.Id,
                    FileName = x.FileName,
                    Location = (ulong)x.Offset,
                    FileSize = Convert.ToInt32(x.ArchiveLength),
                    ExtractSize = Convert.ToInt32(x.ExtractedLength)
                };
            });
            
            //var gr FileSize = oups = toc
            //    .GroupBy(x => x.Id)
            //    .Select(x =>
            //    new TabularRecord
            //    {
            //        Index = x.Key,
            //        Offset = x.GetModifierWhere<IUint64, ulong>(x => x.Name == "FileOffset") + 0x800
            //    });
            //var filesWithLengths = groups
            //    .OrderBy(x => x.Value)
            //    .SelectWithNext((curr, next) => { curr.Length = next.Offset - curr.Offset; return curr; })
            //    .WhenLast(x => { x.Length = etocOffset - x.Offset; return x; });
            //var tocFiles = toc
            //    .GroupBy(x => x.Id)
            //    .Join(filesWithLengths, t => t.Key, f => f.Index, (t, f) =>                          
            //        new File
            //        {
            //            Id = t.Key,
            //            FileName = t.Where(x=>x.Name == "FileName").ToArray()[0].StringName,
            //            Location = f.Offset,
            //            ByteLength = f.Length,
            //            ExtractSize = Convert.ToInt32(t.GetModifierWhere<IUint32, uint>(x => x.Name == "ExtractSize")),
            //            FileSize = Convert.ToInt32(t.GetModifierWhere<IUint32, uint>(x => x.Name == "FileSize"))
            //        }
            //    );
            return new FileList() { FileMeta = tocFiles };
        }
    }
}
