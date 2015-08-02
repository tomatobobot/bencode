using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bencode
{
    public interface IBencodingType
    {
        void Encode(BinaryWriter writer);
    }
}
