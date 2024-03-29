﻿using CriPakInterfaces.Models.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces.Models
{
    public class SectionMeta
    {
        public long TableSize { get; set; }
        public int ColumnOffset { get; set; }
        public int RowOffset { get; set; }
        public int ColumnNamesOffset { get; set; }
        public int DataOffset { get; set; }
        public int SpacerLength { get; set; }
        public int NumColumns { get; set; }
        public int RowLength { get; set; }
        public int NumRows { get; set; }
        public int EndColumnAdjustment { get; set; }
        public int EndColumnOffset => EndColumnAdjustment == 0 ? DataOffset - (DataOffset - ColumnNamesOffset - RowOffset - SpacerLength - 1) : DataOffset - EndColumnAdjustment;
    }
}
