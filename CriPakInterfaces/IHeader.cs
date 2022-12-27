using CriPakInterfaces.Models.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces
{
    public interface IHeader 
    {
        long MetaOffsetPosition { get; set; }
        ulong PackageOffsetPosition { get; set; }


        
        new int Id { get; set; }
        //IEnumerable<Column> Columns { get; set; }
        //IEnumerable<Row> Rows { get; set; }
        string Name { get; set; }
        ulong Offset { get; set; }
    }
}
