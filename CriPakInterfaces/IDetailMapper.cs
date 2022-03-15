using CriPakInterfaces.IComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces
{
    public interface IDetailMapper<out T>
    {
        T Map(IEntity header, Models.ComponentsNew.Row rowValue);
    }
}
