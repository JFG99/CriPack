using CriPakInterfaces.Models;
using CriPakInterfaces.Models.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CriPakRepository.Mappers
{
    public static class EtocRowMapper
    {
        public static void MapEtocData(this CriPak package)
        {
            var updateRowList = package.Utf.Rows.Where(x => x.Name == "UpdateDateTime");
            var localDirList = package.Utf.Rows.Where(x => x.Name == "LocalDir");
            if (updateRowList.Any())
            {
                package.CriFileList = package.CriFileList.Join(updateRowList, t => t.FileId, ur => ur.Id, (t, ur) =>
                {
                    t.UpdateDateTime = ur.uint64;
                    return t;
                }).ToList();
            }
            if (localDirList.Any())
            {
                package.CriFileList = package.CriFileList.Join(localDirList, t => t.FileId, ld => ld.Id, (t, ld) =>
                {

                    t.LocalDir = ld.str;
                    return t;
                }).ToList();
            }
        }
    }
}
