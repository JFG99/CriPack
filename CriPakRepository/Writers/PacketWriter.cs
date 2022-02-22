using CriPakRepository.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CriPakRepository.Writers
{
    public class PacketWriter
    {
        public void Write(BinaryWriter cpk, string ID, ulong position, byte[] packet)
        {
            if (position != 0xffffffffffffffff)
            {
                cpk.BaseStream.Seek((long)position, SeekOrigin.Begin);
                //byte[] encrypted;
                //if (isUtfEncrypted == true)
                //{
                //    encrypted = packet.DecryptUTF(); // Yes it says decrypt...
                //}
                //else
                //{
                //    encrypted = packet;
                //}

                cpk.Write(Encoding.ASCII.GetBytes(ID));
                cpk.Write((Int32)0xff);
                //cpk.Write((UInt64)encrypted.Length);
                //cpk.Write(encrypted);
            }
        }
    }
}
