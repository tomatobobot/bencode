using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bencode
{
    public class TorrentMetadataInfo
    {
        public byte[] Hash { get; set; }
        public string InfoHash { get; set; }
        public string Comment { get; set; }
        public string Announce { get; set; }
        public List<string> AnnounceList { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public string Name { get; set; }

        public Int64 PieceLength { get; set; }
        public List<byte[]> PieceHashes { get; set; }
        public Dictionary<string, Int64> Files { get; set; }
    }
}
