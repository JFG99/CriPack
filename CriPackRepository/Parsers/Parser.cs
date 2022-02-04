using CriPakInterfaces;
using System;

namespace CriPakRepository.Parsers
{
    public class Parser<T> : IParser<T> where T : IEndian
    {
        public bool Parse(IEndianReader br, ulong startOffset)
        {
            return false;
        }            
    }
}
