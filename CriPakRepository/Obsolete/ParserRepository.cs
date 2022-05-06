using CriPakInterfaces;
using CriPakInterfaces.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakRepository.Repositories
{
    public abstract class ParserRepository : IParserRepository //<TOut> Where TOut  : Some CriFile Interface Type 
    {
        //This needs to be built as a base repo to the Parsers  
        //The orchestration should send all 'Read' traffic to here
        //Parsers get injected here as available resources.
        //Where this determines which read is required before sending it to the correct parser.
        public abstract bool Parse(CriPakOld package);
    }
}
