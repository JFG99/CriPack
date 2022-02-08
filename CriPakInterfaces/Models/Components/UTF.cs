using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces.Models.Components
{
    public class UTF
    {
        public UTF()
        {
            Columns = new List<Column>();
            Rows = new List<Row>();
        }

        public List<Column> Columns { get; set; }
        public List<Row> Rows { get; set; }
        public long RowsOffeset { get; set; }
        public long StringsOffset { get; set; }
        public long DataOffset { get; set; }
        public int TableSize { get; set; }
        public int TableName { get; set; }
        public short NumColumns { get; set; }
        public short RowLength { get; set; }
        public int NumRows { get; set; }
    }

}
