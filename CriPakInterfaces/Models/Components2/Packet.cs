using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CriPakInterfaces.Models.Components2
{
    public class Packet : IPacket
    {   
        public Packet()
        {
            PacketBytes = new List<byte>();
            ReadOffset = 0;
        }
        public IEnumerable<byte> PacketBytes { get; set; }
        public long Size => PacketBytes.Count();
        public bool IsEncrypted => CheckEncryption();
        protected int ReadOffset { get; set; }
        
        public override string ToString()
        {
            return string.Join( " ", PacketBytes.ToList().Select(x =>  string.Format("{0:X2}", x)));
        }

        protected IEnumerable<byte> ProcessBytes()
        {
            var seed = 0x0000655f;
            var decrypted = new List<byte>();
            foreach (var entry in PacketBytes)
            {
                decrypted.Add((byte)(entry ^ (byte)(seed & 0xff)));
                //seed modifier
                seed *= 0x00004115;
            }
            return decrypted;
        }

        private bool CheckEncryption()
        {
            return !string.Join("", PacketBytes.Take(4).ToList().Select(x => string.Format("{0:X2}", x))).Equals($"40555446"); //@UTF
        }
    }
}
