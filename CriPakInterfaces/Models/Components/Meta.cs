using CriPakInterfaces.IComponents;
using CriPakInterfaces.Models.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CriPakInterfaces.Models.Components
{
    public class Meta : Entity, IMeta
    {
        public Meta()
        {
            Columns = new List<Column>();
            Rows = new List<Row>();
        }
        public IEnumerable<Column> Columns { get; set; }
        public IEnumerable<Row> Rows { get; set; }
        public long Offset { get; set; }
        public string Name { get; set; }
    }
}
