using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
namespace bencode
{
    public static class Utils
    {
        public static byte[] GetBytes(this byte[] bytes,int start,int length=-1)
        {
            int len = length;
            if (len == -1)
                len = bytes.Length - start;
            byte[] subbyte = new byte[len];
            for (int i = 0; i < bytes.Length; i++)
            {
                subbyte[i] = bytes[start + i];
            }
            return subbyte;
        }
    }
}
