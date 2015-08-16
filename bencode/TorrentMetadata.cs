using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

namespace bencode
{
    public class TorrentMetadata
    {
        private TorrentMetadataInfo Load(Stream stream)
        {
            TorrentMetadataInfo metadata = new TorrentMetadataInfo();
            metadata.AnnounceList = new List<string>();
            metadata.Files = new Dictionary<string, long>();
            metadata.PieceHashes = new List<byte[]>();
            var root = Bencoding.Decode(stream);
            BenDictionary dicroot = (BenDictionary)root;
            if (dicroot == null) return null;
            if (dicroot.ContainsKey(ConstDefinition.ANNOUNCE))
            {
                metadata.Announce = ((BenString)dicroot[ConstDefinition.ANNOUNCE]).Value;
            }
            if (dicroot.ContainsKey(ConstDefinition.ANNOUNCELIST))
            {
                BenList list = (BenList)dicroot[ConstDefinition.ANNOUNCELIST];
                foreach (var type in list)
                {
                    if (type is BenString)
                    {
                        metadata.AnnounceList.Add(((BenString)type).Value);
                    }
                    else
                    {
                        BenList clist = (BenList)type;
                        if (clist != null)
                        {
                            foreach (var btype in clist)
                            {
                                metadata.AnnounceList.Add(((BenString)btype).Value);
                            }
                        }
                    }
                }
            }

            if (dicroot.ContainsKey(ConstDefinition.COMMENT))
            {
                metadata.Comment = ((BenString)dicroot[ConstDefinition.COMMENT]).Value;
            }
            if (dicroot.ContainsKey(ConstDefinition.CREATEDBY))
            {
                metadata.CreatedBy = ((BenString)dicroot[ConstDefinition.CREATEDBY]).Value;
            }
            if (dicroot.ContainsKey(ConstDefinition.CREATIONDATE))
            {
                long second = ((BenInt)dicroot[ConstDefinition.CREATIONDATE]).Value;
                metadata.CreationDate = new DateTime(1970, 1, 1).AddSeconds(second);
            }
            if (dicroot.ContainsKey(ConstDefinition.INFO))
            {
                BenDictionary dictinfo = (BenDictionary)dicroot[ConstDefinition.INFO];
                using (SHA1Managed sha1 = new SHA1Managed())
                {
                    byte[] bytes = Bencoding.EncodeBytes(dictinfo);
                    metadata.Hash = sha1.ComputeHash(bytes);
                }
                if (dictinfo.ContainsKey(ConstDefinition.FILES))
                {
                    BenList filelist = (BenList)dictinfo[ConstDefinition.FILES];
                    foreach (var file in filelist)
                    {
                        BenDictionary dictfile = (BenDictionary)file;
                        string filename = "";
                        long filelength = 0;
                        if (dictfile.ContainsKey(ConstDefinition.PATH))
                        {
                            BenList filenamelist = (BenList)dictfile[ConstDefinition.PATH];
                            foreach (var fn in filenamelist)
                            {
                                filename += ((BenString)fn).Value;
                                filename += "\\";
                            }
                            filename = filename.Trim('\\');
                        }
                        if (dictfile.ContainsKey(ConstDefinition.LENGTH))
                        {
                            filelength = ((BenInt)dictfile[ConstDefinition.LENGTH]).Value;
                        }
                        metadata.Files.Add(filename, filelength);
                    }
                }

                if (dictinfo.ContainsKey(ConstDefinition.NAME))
                {
                    if (metadata.Files.Count == 0 && dictinfo.ContainsKey(ConstDefinition.LENGTH))
                    {
                        metadata.Files.Add(((BenString)dictinfo[ConstDefinition.NAME]).Value, ((BenInt)dictinfo[ConstDefinition.LENGTH]).Value);
                    }
                }

                if (dictinfo.ContainsKey(ConstDefinition.PIECES))
                {
                    BenString pieces = (BenString)dictinfo[ConstDefinition.PIECES];
                    byte[] bytes = Bencoding.EncodeBytes(pieces);
                    for (int i = 0; i < bytes.Length; i += 20)
                    {
                        byte[] hash = bytes.GetBytes(i);
                        metadata.PieceHashes.Add(hash);
                    }
                }

                if (dictinfo.ContainsKey(ConstDefinition.PIECELENGTH))
                {
                    metadata.PieceLength = ((BenInt)dictinfo[ConstDefinition.PIECELENGTH]).Value;
                }
            }
            return metadata;
        }
        public TorrentMetadataInfo FormFile(string filepath)
        {
            using (FileStream fs = File.OpenRead(filepath))
            {
                return Load(fs);
            }
        }

        public TorrentMetadataInfo FormStream(Stream stream)
        {
            return Load(stream);
        }

        public TorrentMetadataInfo FormBytes(byte[] bytes)
        {
            using (MemoryStream ms=new MemoryStream(bytes))
            {
                return Load(ms);
            }
        }

        public TorrentMetadataInfo FormString(string metadataInfo)
        {
            return FormBytes(Bencoding.BEncoding.GetBytes(metadataInfo));
        }
    }
}
