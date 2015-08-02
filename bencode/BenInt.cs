using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bencode
{
    public class BenInt : IBencodingType
    {
        public long Value { get; set; }

        public BenInt()
        {
            Value = 0;
        }

        public BenInt(long value)
        {
            Value = value;
        }
        public static BenInt Decode(BinaryReader reader)
        {
            reader.Read();
            string benint = "";
            char ch;
            while ((ch = reader.ReadChar()) != 'e')
            {
                benint += ch;
            }
            long bint = 0;
            if (!long.TryParse(benint, out bint))
            {
                throw new InvalidCastException();
            }
            return new BenInt { Value = bint };
        }
        public void Encode(BinaryWriter writer)
        {
            writer.Write('i');
            writer.Write(Value.ToString().ToCharArray());
            writer.Write('e');
        }
    }
}
