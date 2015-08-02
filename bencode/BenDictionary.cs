using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bencode
{
    public class BenDictionary : Dictionary<string, IBencodingType>, IBencodingType
    {
        public static BenDictionary Decode(BinaryReader reader)
        {
            reader.Read();
            BenDictionary dict = new BenDictionary();
            while (reader.PeekChar()!='e')
            {
                BenString bs = BenString.Decode(reader);
                IBencodingType value = Bencoding.Decode(reader);
                dict[bs.Value] = value;
            }
            reader.Read();
            return dict;
        }

        public void Encode(BinaryWriter writer)
        {
            writer.Write('d');
            foreach (var item in this)
            {
                BenString bs = new BenString();
                bs.Value = item.Key;
                bs.Encode(writer);
                item.Value.Encode(writer);
            }
            writer.Write('e');
        }
    }
}
