using CriPakInterfaces.Models.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces
{
    public interface IDetailMapper<out T>
    {
        T Map(IDisplayList header, IEnumerable<Row> rowValue); 
    }

    public interface IDetailMapper2<out TOut> 
    {
        TOut Map(IPacket header, IEnumerable<Row> rowValue);
    }

    public interface IMapper<out T>
    {
        T Map(IEnumerable<Row> rowValue);
    }
}
