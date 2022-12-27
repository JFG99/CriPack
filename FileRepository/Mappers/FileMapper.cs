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
            return new FileList() { FileMeta = tocFiles };
        }
    }
}
