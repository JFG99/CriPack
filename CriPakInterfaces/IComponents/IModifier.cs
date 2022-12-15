using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces.IComponents
{
    public interface IModifier
    {
        int Id { get; set; }
        IValue Modifier { get; set; }
    }
}
