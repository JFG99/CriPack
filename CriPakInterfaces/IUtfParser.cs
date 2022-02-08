using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces
{
    public interface IUtfParser<T> : IParser<T> where T : IEndian
    {
        bool Parse(IEndianReader br, Encoding encoding = null);
    }
}
