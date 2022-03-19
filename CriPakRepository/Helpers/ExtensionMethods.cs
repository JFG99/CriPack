using CriPakInterfaces;
using CriPakInterfaces.Models;
using CriPakInterfaces.Models.Components;
using CriPakInterfaces.Models.Components2;
using CriPakRepository.Parsers;
using CriPakRepository.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CriPakRepository.Helpers
{
    //TODO:  refactor the crilayla methods, maybe
    public static class ExtensionMethods
    {             
        public static ushort GetNextBits(byte[] input, ref int offset_p, ref byte bit_pool_p, ref int bits_left_p, int bit_count)
        {
            ushort out_bits = 0;
            int num_bits_produced = 0;
            int bits_this_round;

            while (num_bits_produced < bit_count)
            {
                if (bits_left_p == 0)
                {
                    bit_pool_p = input[offset_p];
                    bits_left_p = 8;
                    offset_p--;
                }

                if (bits_left_p > (bit_count - num_bits_produced))
                    bits_this_round = bit_count - num_bits_produced;
                else
                    bits_this_round = bits_left_p;

                out_bits <<= bits_this_round;

                out_bits |= (ushort)((ushort)(bit_pool_p >> (bits_left_p - bits_this_round)) & ((1 << bits_this_round) - 1));

                bits_left_p -= bits_this_round;
                num_bits_produced += bits_this_round;
            }

            return out_bits;
        }
        unsafe public static int UnsafeCRICompress(byte* dest, int* destLen, byte* src, int srcLen)
        {
            //unsafe pointers seem to be unstable under .NET 4.5,
            //sometimes getting "attempt to read or write protected memory,
            //this usually indicates that other memory is corrupt" error
            //Currently the CRICompress method is moved to the CLR class library for use.
            //TODO:  See how this behaves under the new .NET Core 3.1 and refactor the pointers out.
            int n = srcLen - 1, m = *destLen - 0x1, T = 0, d = 0;

            int p, q = 0, i, j, k;
            byte* odest = dest;
            for (; n >= 0x100; )
            {
                j = n + 3 + 0x2000;
                if (j > srcLen) j = srcLen;
                for (i = n + 3, p = 0; i < j; i++)
                {
                    for (k = 0; k <= n - 0x100; k++)
                    {
                        if (*(src + n - k) != *(src + i - k)) break;
                    }
                    if (k > p)
                    {
                        q = i - n - 3;
                        p = k;
                    }
                }
                if (p < 3)
                {
                    d = (d << 9) | (*(src + n--));
                    T += 9;
                }
                else
                {
                    d = (((d << 1) | 1) << 13) | q;

                    T += 14; n -= p;
                    if (p < 6)
                    {
                        d = (d << 2) | (p - 3); T += 2;
                    }
                    else if (p < 13)
                    {
                        d = (((d << 2) | 3) << 3) | (p - 6); T += 5;
                    }
                    else if (p < 44)
                    {
                        d = (((d << 5) | 0x1f) << 5) | (p - 13); T += 10;
                    }
                    else
                    {
                        d = ((d << 10) | 0x3ff); T += 10; p -= 44;
                        for (; ; )
                        {
                            for (; T >= 8; )
                            {
                                *(dest + m--) = (byte)((d >> (T - 8)) & 0xff);
                                T -= 8; d = d & ((1 << T) - 1);
                            }
                            if (p < 255) break;
                            d = (d << 8) | 0xff; T += 8; p = p - 0xff;
                        }
                        d = (d << 8) | p; T += 8;
                    }
                }
                for (; T >= 8; )
                {
                    *(dest + m--) = (byte)((d >> (T - 8)) & 0xff);
                    T -= 8;
                    d = d & ((1 << T) - 1);
                }
            }
            if (T != 0)
            {
                *(dest + m--) = (byte)(d << (8 - T));
            }

            *(dest + m--) = 0; *(dest + m) = 0;
            for (; ; )
            {
                if (((*destLen - m) & 3) == 0) break;
                *(dest + m--) = 0;
            }

            *destLen = *destLen - m;
            dest += m;

            int[] l = { 0x4c495243, 0x414c5941, srcLen - 0x100, *destLen };

            for (j = 0; j < 4; j++)
            {
                for (i = 0; i < 4; i++)
                {
                    *(odest + i + j * 4) = (byte)(l[j] & 0xff);
                    l[j] >>= 8;
                }
            }
            for (j = 0, odest += 0x10; j < *destLen; j++)
            {
                *(odest++) = *(dest + j);
            }
            for (j = 0; j < 0x100; j++)
            {
                *(odest++) = *(src + j);
            }
            *destLen += 0x110;

            return *destLen;
        }
        unsafe public static byte[] CompressCRILAYLA(this byte[] input)
        {
            unsafe
            {
                fixed (byte* src = input, dst = new byte[input.Length])
                {
                    //Move cricompress to CLR
                    int destLength = (int)input.Length;

                    int result = UnsafeCRICompress(dst, &destLength, src, input.Length);
                    //int result = LibCRIComp.CriCompression.CRIcompress(dst, &destLength, src, input.Length);
                    byte[] arr = new byte[destLength];
                    Marshal.Copy((IntPtr)dst, arr, 0, destLength);
                    return arr;
                }
            }

            /*unsafe
            {

                int destLength = (int)input.Length;
                fixed (byte* src = input)
                fixed (byte* dest = new byte[input.Length])
                {

                    destLength = UnsafeCRICompress(dest, &destLength, src, input.Length);
                    byte[] arr = new byte[destLength];
                    Marshal.Copy((IntPtr)dest, arr, 0, destLength);

                    
                    return arr;
                }
            }*/

        }
        public static byte[] DecompressCRILAYLA(this byte[] input)
        {
            byte[] result;// = new byte[USize];

            MemoryStream ms = new MemoryStream(input);
            var br = new EndianReader<MemoryStream, EndianData>(new MemoryStream(input), new EndianData(true));

            br.BaseStream.Seek(8, SeekOrigin.Begin); // Skip CRILAYLA
            int uncompressed_size = br.ReadInt32();
            int uncompressed_header_offset = br.ReadInt32();

            result = new byte[uncompressed_size + 0x100];

            // do some error checks here.........

            // copy uncompressed 0x100 header to start of file
            Array.Copy(input, uncompressed_header_offset + 0x10, result, 0, 0x100);

            int input_end = input.Length - 0x100 - 1;
            int input_offset = input_end;
            int output_end = 0x100 + uncompressed_size - 1;
            byte bit_pool = 0;
            int bits_left = 0, bytes_output = 0;
            int[] vle_lens = new int[4] { 2, 3, 5, 8 };

            while (bytes_output < uncompressed_size)
            {
                if (GetNextBits(input, ref input_offset, ref bit_pool, ref bits_left, 1) > 0)
                {
                    int backreference_offset = output_end - bytes_output + GetNextBits(input, ref input_offset, ref bit_pool, ref bits_left, 13) + 3;
                    int backreference_length = 3;
                    int vle_level;

                    for (vle_level = 0; vle_level < vle_lens.Length; vle_level++)
                    {
                        int this_level = GetNextBits(input, ref input_offset, ref bit_pool, ref bits_left, vle_lens[vle_level]);
                        backreference_length += this_level;
                        if (this_level != ((1 << vle_lens[vle_level]) - 1)) break;
                    }

                    if (vle_level == vle_lens.Length)
                    {
                        int this_level;
                        do
                        {
                            this_level = GetNextBits(input, ref input_offset, ref bit_pool, ref bits_left, 8);
                            backreference_length += this_level;
                        } while (this_level == 255);
                    }

                    for (int i = 0; i < backreference_length; i++)
                    {
                        result[output_end - bytes_output] = result[backreference_offset--];
                        bytes_output++;
                    }
                }
                else
                {
                    // verbatim byte
                    result[output_end - bytes_output] = (byte)GetNextBits(input, ref input_offset, ref bit_pool, ref bits_left, 8);
                    bytes_output++;
                }
            }

            br.Close();
            ms.Close();

            return result;
        }
        public static byte[] DecompressLegacyCRI(this byte[] input)
        {
            byte[] result;// = new byte[USize];
            var br = new EndianReader<MemoryStream, EndianData>(new MemoryStream(input), new EndianData(true));

            br.BaseStream.Seek(8, SeekOrigin.Begin); // Skip CRILAYLA
            int uncompressed_size = br.ReadInt32();
            int uncompressed_header_offset = br.ReadInt32();

            result = new byte[uncompressed_size + 0x100];

            // do some error checks here.........

            // copy uncompressed 0x100 header to start of file
            Array.Copy(input, uncompressed_header_offset + 0x10, result, 0, 0x100);

            int input_end = input.Length - 0x100 - 1;
            int input_offset = input_end;
            int output_end = 0x100 + uncompressed_size - 1;
            byte bit_pool = 0;
            int bits_left = 0, bytes_output = 0;
            int[] vle_lens = new int[4] { 2, 3, 5, 8 };

            while (bytes_output < uncompressed_size)
            {
                if (GetNextBits(input, ref input_offset, ref bit_pool, ref bits_left, 1) > 0)
                {
                    int backreference_offset = output_end - bytes_output + GetNextBits(input, ref input_offset, ref bit_pool, ref bits_left, 13) + 3;
                    int backreference_length = 3;
                    int vle_level;

                    for (vle_level = 0; vle_level < vle_lens.Length; vle_level++)
                    {
                        int this_level = GetNextBits(input, ref input_offset, ref bit_pool, ref bits_left, vle_lens[vle_level]);
                        backreference_length += this_level;
                        if (this_level != ((1 << vle_lens[vle_level]) - 1)) break;
                    }

                    if (vle_level == vle_lens.Length)
                    {
                        int this_level;
                        do
                        {
                            this_level = GetNextBits(input, ref input_offset, ref bit_pool, ref bits_left, 8);
                            backreference_length += this_level;
                        } while (this_level == 255);
                    }

                    for (int i = 0; i < backreference_length; i++)
                    {
                        result[output_end - bytes_output] = result[backreference_offset--];
                        bytes_output++;
                    }
                }
                else
                {
                    // verbatim byte
                    result[output_end - bytes_output] = (byte)GetNextBits(input, ref input_offset, ref bit_pool, ref bits_left, 8);
                    bytes_output++;
                }
            }

            br.Close();
            return result;
        }
        public static void UpdateCriFile(this ICriPak package, CriFile file)
        {
            if (file.FileType == "FILE" || file.FileType == "HDR")
            {
                byte[] updateMe = null;
                switch (file.TOCName)
                {
                    case "CPK":
                        updateMe = package.CpkPacket;
                        break;
                    case "TOC":
                        updateMe = package.TocPacket;
                        break;
                    case "ITOC":
                        updateMe = package.ItocPacket;
                        break;              
                    case "ETOC":            
                        updateMe = package.EtocPacket;
                        break;              
                    case "GTOC":            
                        updateMe = package.GtocPacket;
                        break;
                    default:
                        throw new Exception("I need to implement this TOC!");

                }


                //Update ExtractSize
                if (file.ExtractSizePos > 0)
                    UpdateValue(ref updateMe, file.ExtractedFileSize, file.ExtractSizePos, file.ExtractSizeType);

                //Update FileSize
                if (file.FileSizePos > 0)
                    UpdateValue(ref updateMe, file.ExtractedFileSize, file.FileSizePos, file.FileSizeType);

                //Update FileOffset
                if (file.FileOffsetPos > 0)
                    if (file.TOCName == "TOC")
                    {
                        UpdateValue(ref updateMe, file.FileOffset - package.TocOffset, file.FileOffsetPos, file.FileOffsetType);
                    }
                    else
                    {
                        UpdateValue(ref updateMe, file.FileOffset, file.FileOffsetPos, file.FileOffsetType);
                    }

                switch (file.TOCName)
                {
                    case "CPK":
                       package.CpkPacket = updateMe;
                        break;
                    case "TOC":
                        package.TocPacket = updateMe;
                        break;
                    case "ITOC":
                        package.ItocPacket = updateMe;
                        break;
                    case "ETOC":
                        updateMe = package.EtocPacket;
                        break;
                    case "GTOC":
                        updateMe = package.GtocPacket;
                        break;
                    default:
                        throw new Exception("I need to implement this TOC!");

                }
            }
        }
        public static void UpdateValue(ref byte[] packet, object value, long pos, Type type)
        {
            MemoryStream temp = new MemoryStream();
            temp.Write(packet, 0, packet.Length);

            EndianWriter toc = new EndianWriter(temp, false);
            toc.Seek((int)pos, SeekOrigin.Begin);

            value = Convert.ChangeType(value, type);

            if (type == typeof(Byte))
            {
                toc.Write((Byte)value);
            }
            else if (type == typeof(UInt16))
            {
                toc.Write((UInt16)value);
            }
            else if (type == typeof(UInt32))
            {
                toc.Write((UInt32)value);
            }
            else if (type == typeof(UInt64))
            {
                toc.Write((UInt64)value);
            }
            else if (type == typeof(Single))
            {
                toc.Write((Single)value);
            }
            else
            {
                throw new Exception("Not supported type!");
            }

            toc.Close();

            MemoryStream myStream = (MemoryStream)toc.BaseStream;
            packet = myStream.ToArray();

        }


        #region Obsolete: for Deletion
        [Obsolete]
        public static byte[] GetData(this IEndianReader br, long offset, int size)
        {
            byte[] result = null;
            long backup = br.BaseStream.Position;
            br.BaseStream.Seek(offset, SeekOrigin.Begin);
            result = br.ReadBytes(size);
            br.BaseStream.Seek(backup, SeekOrigin.Begin);
            return result;
        }
        [Obsolete]
        public static void ReadUTFData(this ICriPak package)
        {
            package.IsUtfEncrypted = false;
            package.Reader.IsLittleEndian = true;

            package.Unk1 = package.Reader.ReadInt32();
            package.UtfSize = package.Reader.ReadInt64();
            package.UtfPacket = package.Reader.ReadBytes((int)package.UtfSize);
            package.OriginalPacket = package.UtfPacket;

            if (package.UtfPacket[0] != 0x40 && package.UtfPacket[1] != 0x55 && package.UtfPacket[2] != 0x54 && package.UtfPacket[3] != 0x46) //@UTF
            {
                package.DecryptedPacket = package.UtfPacket.DecryptUTF();
                package.UtfPacket = package.DecryptedPacket;
                package.IsUtfEncrypted = true;
            }

            package.Reader.IsLittleEndian = false;
        }
        [Obsolete]
        public static byte[] DecryptUTF(this byte[] input)
        {
            var seed = 0x0000655f;
            var decrypted = new List<byte>();
            foreach (var entry in input)
            {
                decrypted.Add((byte)(entry ^ (byte)(seed & 0xff)));
                //seed modifier
                seed *= 0x00004115;
            }
            var test = decrypted.Select(x => string.Join(" ", string.Format("{0:X2}", x)));
            return decrypted.ToArray();
        }
        [Obsolete]
        public static bool ReadDataRows(this ICriPak package)
        {
            package.SubReader = new EndianReader<MemoryStream, EndianData>(new MemoryStream(package.UtfPacket), new EndianData(package.Reader.IsLittleEndian));
            var parser = new UtfParser();
            if (!parser.Parse((CriPak)package))
            {
                package.SubReader.Close();
                return false;
            }
            return true;
        }
        [Obsolete]
        public static bool CheckListRedundant(this List<CriFile> input)
        {

            bool result = false;
            List<string> tmp = new List<string>();
            for (int i = 0; i < input.Count; i++)
            {
                string name = ((input[i].DirName != null) ?
                                        input[i].DirName + "/" : "") + input[i].FileName;
                if (!tmp.Contains(name))
                {
                    tmp.Add(name);
                }
                else
                {
                    return true;
                }
            }
            return result;
        }
        [Obsolete] 
        public static string ReadCString(this IEndianReader br, long offsetLocation = -1, Encoding encoding = null) => br.ReadCString(-1, offsetLocation, encoding);
        [Obsolete]        
        public static string ReadCString(this IEndianReader br, int MaxLength, long offsetLocation = -1, Encoding encoding = null)
        {
            int Max;
            if (MaxLength == -1)
                Max = 255;
            else
                Max = MaxLength;

            long fTemp = br.BaseStream.Position;
            int i = 0;
            string result = "";

            if (offsetLocation > -1)
            {
                br.BaseStream.Seek(offsetLocation, SeekOrigin.Begin);
            }

            do
            {
                var bTemp = br.ReadByte();
                if (bTemp == 0)
                    break;
                i += 1;
            } while (i < Max);

            if (MaxLength == -1)
                Max = i + 1;
            else
                Max = MaxLength;

            if (offsetLocation > -1)
            {
                br.BaseStream.Seek(offsetLocation, SeekOrigin.Begin);

                if (encoding == null)
                    result = Encoding.UTF8.GetString(br.ReadBytes(i));
                else
                    result = encoding.GetString(br.ReadBytes(i));

                br.BaseStream.Seek(fTemp, SeekOrigin.Begin);
            }
            else
            {
                br.BaseStream.Seek(fTemp, SeekOrigin.Begin);
                if (encoding == null)
                    result = Encoding.ASCII.GetString(br.ReadBytes(i));
                else
                    result = encoding.GetString(br.ReadBytes(i));

                br.BaseStream.Seek(fTemp + Max, SeekOrigin.Begin);
            }

            return result;
        }
        #endregion


    }
}
