using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakInterfaces
{
    public interface ICpkParser : IParser<IEndian>
    {
        bool Parse(string path, Encoding encoding = null);
    }
}
