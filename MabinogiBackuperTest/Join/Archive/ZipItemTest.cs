using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MabinogiBackuperLib.Archive;
using NUnit.Framework;

namespace MabinogiBackuperTest.Join.Archive
{
    [TestFixture]
    public class ZipItemTest
    {
        [Test]
        public void ExtractedSizeTest()
        {
            using var fs = new FileStream($"{TestContext.CurrentContext.TestDirectory}/TestData/test.zip", FileMode.Open, FileAccess.Read, FileShare.Read);
            using var zip = new ZipExtractor(fs);
            zip.Initialize();

            var root = zip.Root;

            var exp = 26L;
            var act = root.ExtractedSize();

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ExtractedSizeTest2()
        {
            using var fs = new FileStream($"{TestContext.CurrentContext.TestDirectory}/TestData/test.zip", FileMode.Open, FileAccess.Read, FileShare.Read);
            using var zip = new ZipExtractor(fs);
            zip.Initialize();

            var root = zip.Root.Files[0];

            var exp = 7L;
            var act = root.ExtractedSize();

            Assert.AreEqual(exp, act);
        }
    }
}
