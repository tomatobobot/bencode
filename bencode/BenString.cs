using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bencode
{
    public class BenString : IBencodingType
    {
        public string Value { get; set; }

        public BenString()
        {

        }
        public BenString(string value)
        {
            Value = value;
        }

        public static BenString Decode(BinaryReader reader)
        {
            string benlength = "";
            char ch;
            while ((ch = reader.ReadChar()) != ':')
            {
                benlength += ch;
            }
            int bstringlength = 0;
            if (!int.TryParse(benlength, out bstringlength))
            {
                throw new InvalidCastException();
            }
            byte[] bytes = reader.ReadBytes(bstringlength);
            return new BenString { Value = Bencoding.BEncoding.GetString(bytes) };
        }

        public void Encode(BinaryWriter writer)
        {
            byte[] bytes = Bencoding.BEncoding.GetBytes(Value);
            writer.Write(Bencoding.BEncoding.GetBytes(bytes.Length.ToString()));
            writer.Write(':');
            writer.Write(bytes);
        }

        public override bool Equals(object obj)
        {
            BenString bs = obj as BenString;
            if (bs == null)
                return false;
            return Equals(bs);
        }

        public bool Equals(BenString other)
        {
            if (other == null)
                return false;

            if (other == this)
                return true;

            return Equals(other.Value, Value);
        }
    }
}
