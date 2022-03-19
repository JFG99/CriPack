using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces.IComponents
{
    public interface IRowValue
    {
        int Id { get; set; }
        int TypeSelect { get; set; }
        Type Type { get; set; }
        string Name { get; set; }
        long Position { get; set; }
        ulong Value { get; set; }
        int Length { get; }
        Type GetType();
    }
}