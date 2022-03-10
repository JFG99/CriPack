using System;
using System.IO;
using System.Text;

namespace CriPakRepository.Writers
{
    public class CpkWriter
    {
        public void Write(BinaryWriter cpk)
        {
            //WritePacket(cpk, "CPK ", 0, CPK_packet);
            //if (position != 0xffffffffffffffff)
            //{
            //    cpk.BaseStream.Seek((long)position, SeekOrigin.Begin);
            //    byte[] encrypted;
            //    if (isUtfEncrypted == true)
            //    {
            //        encrypted = packet.DecryptUTF(); // Yes it says decrypt...
            //    }
            //    else
            //    {
            //        encrypted = packet;
            //    }

            //    cpk.Write(Encoding.ASCII.GetBytes(ID));
            //    cpk.Write((Int32)0xff);
            //    cpk.Write((UInt64)encrypted.Length);
            //    cpk.Write(encrypted);
            //}

            //cpk.BaseStream.Seek(0x800 - 6, SeekOrigin.Begin);
            //cpk.Write(Encoding.ASCII.GetBytes("(c)CRI"));
            //if ((TocOffset > 0x800) && TocOffset < 0x8000)
            //{
            //    //Part of cpk starts TOC from 0x2000, so
            //    //Need to calculate cpk padding
            //    cpk.Write(new byte[TocOffset - 0x800]);
            //}
        }
    }
}
