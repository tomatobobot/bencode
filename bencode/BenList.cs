using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bencode
{
    public class BenList : List<IBencodingType>, IBencodingType
    {
        
        public static BenList Decode(BinaryReader reader)
        {
            reader.Read();
            BenList list = new BenList();
            while (reader.PeekChar()!='e')
            {
                list.Add(Bencoding.Decode(reader));
            }
            reader.Read();
            return list;
        }

        public void Encode(BinaryWriter writer)
        {
            writer.Write('l');
            foreach (var item in this)
            {
                item.Encode(writer);
            }
            writer.Write('e');
        }
    }
}
