using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces.Models.Components
{
    public enum STORAGE : int
    {
        MASK = 0xf0,
        NONE = 0x00,
        ZERO = 0x10,
        CONSTANT = 0x30,
        PERROW = 0x50
    }
}
