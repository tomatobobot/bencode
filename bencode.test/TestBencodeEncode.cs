using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
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
            Assert.AreEqual("d4:testi5ee",str);
        }

        [TestMethod]
        public void TestDecodeFile()
        {
            string[] files = Directory.GetFiles(".",".torrent");
            foreach (var item in files)
            {
                IBencodingType torrent = Bencoding.DecodeFile(item);
                Assert.IsNotNull(torrent);
            }
        }
    }
}
