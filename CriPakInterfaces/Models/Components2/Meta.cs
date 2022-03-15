using CriPakInterfaces.IComponents;
using CriPakInterfaces.Models.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CriPakInterfaces.Models.Components2
{
    public class Meta : Entity, IMeta
    {
        public Meta()
        {
            Columns = new List<Column>();
            Rows = new List<ComponentsNew.Row>();
        }
        public IEnumerable<Column> Columns { get; set; }
        public IEnumerable<ComponentsNew.Row> Rows { get; set; }
        public long Offset { get; set; }
    }
}
