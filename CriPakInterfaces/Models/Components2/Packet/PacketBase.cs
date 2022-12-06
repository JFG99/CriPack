using System.Collections.Generic;
using System.Linq;

namespace CriPakInterfaces.Models.Components2
{
    public class PacketBase
    {
        public IEnumerable<byte> PacketBytes { get; set; }
        public IEnumerable<byte> DecryptedBytes { get; set; }
        protected int LastStringLength { get; set; }
        protected int ReadOffset { get; set; }

        public override string ToString()
        {
            return string.Join(" ", PacketBytes.ToList().Select(x => string.Format("{0:X2}", x)));
        }
    }
}