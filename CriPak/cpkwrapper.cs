using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CriPakInterfaces.Models.Components;
using CriPakRepository.Helpers;
using CriPakRepository.Parsers;
using CriPakInterfaces;

namespace CriPakComplete
{

    public class myPackage
    {
        //public CPK cpk { get; set; }
        public string BaseName { get; set; }
        public string baseName { get; set; }
        public string basePath { get; set; }
        public string BasePath { get; set; }
        public string CpkName { get; set; }
        public string cpk_Name { get; set; }
        public string fileName { get; set; }
    }
    public class CPKTable
    {
        public int id { get; set; }
        public string FileName { get; set; }
        public string _localName { get; set; }
        //public string DirName;
        public UInt64 FileOffset { get; set; }
        public int FileSize { get; set; }
        public int ExtractSize { get; set; }
        public string FileType { get; set; }
        public float Pt { get; set; }
    }

    public class cpkwrapper
    {
        //File Counts....  Just get from CriPak list.
        public int nums = 0;
        public List<PackagedFile> tablePkgFiles;
        public cpkwrapper(string inFile)
        {
            tablePkgFiles = new List<PackagedFile>();
            var CriPak = new CriPakInterfaces.Models.CriPak
            {   
                CpkName = inFile,
                Encoding = Encoding.GetEncoding(65001)
            };

            var parser = new CpkParser();
            parser.Parse(CriPak);

            var entries = CriPak.CriFileList.OrderBy(x => x.FileOffset).ToList();
            bool bFileRepeated = entries.CheckListRedundant();
            
            foreach (var entry in entries)
            {
                /*
                Console.WriteLine("FILE ID:{0},File Name:{1},File Type:{5},FileOffset:{2:x8},Extract Size:{3:x8},Chunk Size:{4:x8}", entry.ID,
                                                            (((entry.DirName != null) ? entry.DirName + "/" : "") + entry.FileName),
                                                            entry.FileOffset,
                                                            entry.ExtractSize,
                                                            entry.FileSize,
                                                            entry.FileType);
                */                
                
                if (entry.FileType != null)
                {
                    nums += 1;

                    var t = new PackagedFile();
                    if (entry.FileId == null)
                    {
                        t.FileId = -1;
                    }
                    else
                    {
                        t.FileId = Convert.ToInt32(entry.FileId);
                    }
                    if (t.FileId >= 0 && bFileRepeated)
                    {
                        t.FileName = ((entry.DirName != null) ? entry.DirName + "/" : "") + string.Format("[{0}]",t.FileId.ToString()) + entry.FileName;
                    }
                    else
                    {
                        t.FileName = ((entry.DirName != null) ? entry.DirName + "/" : "") +  entry.FileName;
                    }
                    t.LocalName = entry.FileName.ToString();

                    t.FileOffset = Convert.ToUInt64(entry.FileOffset);
                    t.CompressedFileSize = Convert.ToInt32(entry.FileSize);
                    t.ExtractedFileSize = Convert.ToInt32(entry.ExtractedFileSize);
                    t.FileType = entry.FileType;
                    if (entry.FileType == "FILE")
                    {
                        t.CompressionPercentage = (float)Math.Round(t.CompressedFileSize / (float)t.ExtractedFileSize, 2) * 100f;
                    }
                    else
                    {
                        t.CompressionPercentage = (float)1f * 100f;
                    }
                    tablePkgFiles.Add(t);
                }
            }
        }
    }
}
