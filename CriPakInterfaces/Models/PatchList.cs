using System;


namespace CriPakInterfaces.Models
{
    public class PatchList : DisplayList, IPatchList
    {
        public ulong LengthDifference { get; set; }
        public bool IsPatched { get; set; }
        public int IndexInArchive { get; set; }
    }
}
