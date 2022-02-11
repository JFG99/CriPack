using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces.Models.Components
{
    public enum CRITYPE : int
    {
        MASK = 0x0f,
        DATA = 0x0b,
        STRING = 0x0a,
        FLOAT = 0x08,
        EIGHTBYTE2 = 0x07,
        EIGHTBYTE = 0x06,
        FOURBYTE2 = 0x05,
        FOURBYTE = 0x04,
        TWOBYTE2 = 0x03,
        TWOBYTE = 0x02,
        ONEBYTE2 = 0x01,
        ONEBYTE = 0x00
    }
}
