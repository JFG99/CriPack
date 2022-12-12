using CriPakInterfaces;
using CriPakInterfaces.IComponents;
using CriPakInterfaces.Models.Components;
using FileList = CriPakInterfaces.Models.Components.Files;
using CriPakRepository.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FileRepository.Mappers
{
    public class FileMapper : Mapper, IExtractorMapper<FileList>
    {
        public FileList Map(IEnumerable<IDisplayList> headers)
        {
            var etocOffset = headers.OfType<EtocHeader>().First().PackageOffsetPosition;
            var toc = headers.OfType<ITocHeader>().First().Rows;            
            var groups = toc.GroupBy(x => x.Id)
                          .Select(x =>
                          new
                          {
                              Index = x.Key,
                              Value = x.Where(y => y.Name == "FileOffset").Select(y => y.Modifier).Cast<IUint64>().ToArray()[0].Value + 0x800
                          })
                          .OfType<ITabularRecord>();
            var filesWithLengths = groups.OrderBy(x => x.Value)
                                         .AggregateDifference(etocOffset, 0);
            var tocFiles = toc.GroupBy(x => x.Id)
                          .Join(filesWithLengths, t => t.Key, f => f.Index, (t, f) =>                          
                              new File
                              {
                                  Id = t.Key,
                                  FileName = t.Where(x=>x.Name == "FileName").ToArray()[0].StringName,
                                  Location = f.Offset,
                                  ByteLength = f.Length,
                                  ExtractSize = Convert.ToInt32(t.GetModifierWhere<IUint32, uint>(x => x.Name == "ExtractSize")),
                                  FileSize = Convert.ToInt32(t.GetModifierWhere<IUint32, uint>(x => x.Name == "FileSize"))
                              }
                          );
            return new FileList() { FileMeta = tocFiles };
        }
    }
}
