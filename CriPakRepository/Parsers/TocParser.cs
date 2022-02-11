﻿using CriPakInterfaces;
using CriPakInterfaces.Models;
using CriPakInterfaces.Models.Components;
using CriPakRepository.Helpers;
using CriPakRepository.Mappers;
using CriPakRepository.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CriPakRepository.Parsers
{
    public class TocParser : ParserRepository
    {
        public override bool Parse(CriPak package)
        {
            package.Reader.BaseStream.Seek((long)package.TocOffset, SeekOrigin.Begin);
            if (package.Reader.ReadCString(4) != "TOC ")
            {
                package.Reader.Close();
                return false;
            }

            package.ReadUTFData();
            package.TocPacket = package.UtfPacket;

            if (!package.ReadDataRows())
            {
                return false;
            }
            package.MapTocData();
            return true;
        }
    }
}