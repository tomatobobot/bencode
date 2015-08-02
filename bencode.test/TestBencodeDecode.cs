using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bencode.test
{
    [TestClass]
    public class TestBencodeDecode
    {
        [TestMethod]
        public void TestBenStringDecode()
        {
            string str = "4:test";
            var result = Bencoding.Decode(str);
            BenString bs = new BenString("test");
            Assert.AreEqual(bs, result);
        }
    }
}
