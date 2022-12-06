using CriPakInterfaces.IComponents;
using CriPakInterfaces.Models.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces
{
    public interface IMeta : IDisplayList
    {
        IEnumerable<Column> Columns { get; set; }
        IEnumerable<Models.Components.Row> Rows { get; set; }
        long Offset { get; set; }                
    }
}
