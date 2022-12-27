using CriPakInterfaces.Models.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces.Models
{
    public class SectionHeader
    {
        public IEnumerable<Column> Columns { get; set; }
        public IEnumerable<Row> Rows { get; set; }
    }
}
