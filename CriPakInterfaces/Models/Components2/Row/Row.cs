using CriPakInterfaces.IComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces.Models.Components2
{
    public abstract class Row<T> : IRowValue
    {
        public int Id { get; set; }
        public int TypeSelect { get; set; }
        public Type Type { get; set; }
        public string Name { get; set; }
        public long Position { get; set; }
        public ulong Value { get; set; }
        public virtual int Length => 0;
        public new abstract Type GetType();
        public abstract T GetValue();
    }
}
