using CriPakInterfaces.IComponents;
using CriPakInterfaces.Models.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces
{
    public interface IMeta : IEntity
    {
        IEnumerable<Column> Columns { get; set; }
        IEnumerable<IRowValue> Rows { get; set; }
        long Offset { get; set; }                
    }
}
