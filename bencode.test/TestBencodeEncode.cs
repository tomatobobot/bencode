using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Base32;

namespace bencode.test
{
    [TestClass]
    public class TestBencodeEncode
    {
        [TestMethod]
        public void TestBenStringEncode()
        {
            BenString bs = new BenString("test");
            string str = Bencoding.EncodeString(bs);
            Assert.AreEqual("4:test", str);
        }
        [TestMethod]
        public void TestBenIntEncode()
        {
            BenInt bint = new BenInt(5);
            string str = Bencoding.EncodeString(bint);
            Assert.AreEqual("i5e", str);
        }
        [TestMethod]
        public void TestBenListEncode()
        {
            BenList list = new BenList();
            BenString bs = new BenString("test");
            BenInt bi = new BenInt(54);
            list.Add(bs);
            list.Add(bi);
            string str = Bencoding.EncodeString(list);
            Assert.AreEqual("l4:testi54ee", str);
        }
        [TestMethod]
        public void TestBenDictonaryEncode()
        {
            BenDictionary dict = new BenDictionary();
            BenInt bi = new BenInt(5);
            dict.Add("test", bi);
            string str = Bencoding.EncodeString(dict);
            Assert.AreEqual("d4:testi5ee", str);
        }

        [TestMethod]
        public void TestDecodeFile()
        {
            string[] files = Directory.GetFiles(".", "*.torrent");
            foreach (var item in files)
            {
                IBencodingType torrent = Bencoding.DecodeFile(item);
                Assert.IsNotNull(torrent);
            }
        }
        [TestMethod]
        public void TestTorrentFileHash()
        {
            string[] files = Directory.GetFiles(".", "*.torrent");
            foreach (var item in files)
            {
                IBencodingType torrent = Bencoding.DecodeFile(item);
                var dict = torrent as BenDictionary;
                var xxoo = (BenDictionary)dict["info"];
                byte[] bytes = Bencoding.CalculateTorrentInfoHash(xxoo);
                string infohash = "";
                foreach (var b in bytes)
                {
                    infohash += String.Format("{0:X2}", b);
                }

                Assert.IsNotNull(infohash);
            }
        }

    }

}
