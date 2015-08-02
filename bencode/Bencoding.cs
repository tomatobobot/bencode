using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

namespace bencode
{
    public class Bencoding
    {
        public static Encoding BEncoding { get; set; }

        static Bencoding()
        {
            BEncoding = Encoding.UTF8;
        }
        public static IBencodingType DecodeFile(string filename)
        {
            using (FileStream stream = File.OpenRead(filename))
            {
                return Decode(stream);
            }
        }
        public static IBencodingType Decode(string input)
        {
            byte[] byteArray = BEncoding.GetBytes(input);
            return Decode(new MemoryStream(byteArray));
        }
        public static IBencodingType Decode(Stream stream)
        {
            using (BinaryReader reader = new BinaryReader(stream, BEncoding))
            {
                return Decode(reader);
            }
        }
        internal static IBencodingType Decode(BinaryReader reader)
        {
            char next = (char)reader.PeekChar();
            switch (next)
            {
                case 'i':
                    return BenInt.Decode(reader);
                case 'l':
                    return BenList.Decode(reader);
                case 'd':
                    return BenDictionary.Decode(reader);
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    return BenString.Decode(reader);
            }
            return null;
        }

        public static void Encode(IBencodingType bencode, Stream output)
        {
            BinaryWriter writer = new BinaryWriter(output, BEncoding);
            bencode.Encode(writer);
            writer.Flush();
        }
        public static string EncodeString(IBencodingType bencode)
        {
            MemoryStream ms = new MemoryStream();
            Encode(bencode, ms);
            ms.Position = 0;
            return new StreamReader(ms, BEncoding).ReadToEnd();
        }

        public static byte[] EncodeBytes(IBencodingType bencode)
        {
            MemoryStream ms = new MemoryStream();
            Encode(bencode, ms);
            ms.Position = 0;
            return new BinaryReader(ms, BEncoding).ReadBytes((int)ms.Length);
        }
        public static byte[] CalculateTorrentInfoHash(BenDictionary dict)
        {
            byte[] bytes = EncodeBytes(dict);
            return new SHA1CryptoServiceProvider().ComputeHash(bytes);
        }
    }
}
