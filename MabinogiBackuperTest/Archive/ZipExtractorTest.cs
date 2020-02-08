using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MabinogiBackuperLib.Archive;
using NUnit.Framework;

namespace MabinogiBackuperTest.Archive
{
    [TestFixture]
    public class ZipExtractorTest
    {
        [Test]
        public void ExtractTest()
        {
            using var fs = new FileStream($"{TestContext.CurrentContext.TestDirectory}/TestData/test.zip", FileMode.Open, FileAccess.Read, FileShare.Read);
            using var zip = new ZipExtractor(fs);

            zip.Extract("sub/", $"{TestContext.CurrentContext.TestDirectory}/out/sub");

            using var textFileStream = new FileStream($"{TestContext.CurrentContext.TestDirectory}/out/sub/TextFile2.txt", FileMode.Open, FileAccess.Read);
            using var expFileStream = new FileStream($"{TestContext.CurrentContext.TestDirectory}/TestData/compress/sub/TextFile2.txt", 
                FileMode.Open, FileAccess.Read);

            var textBytes = new byte[textFileStream.Length];
            var expBytes = new byte[expFileStream.Length];
            textFileStream.Read(textBytes, 0, textBytes.Length);
            expFileStream.Read(expBytes, 0, expBytes.Length);

            CollectionAssert.AreEqual(expBytes, textBytes);
        }

        [Test]
        public void ExtractSingleTest()
        {
            using var fs = new FileStream($"{TestContext.CurrentContext.TestDirectory}/TestData/test.zip", FileMode.Open, FileAccess.Read, FileShare.Read);
            using var zip = new ZipExtractor(fs);

            zip.Extract("/sub/TextFile2.txt", $"{TestContext.CurrentContext.TestDirectory}/out/single");
        }

        [Test]
        public void ExtractRootTest()
        {
            using var fs = new FileStream($"{TestContext.CurrentContext.TestDirectory}/TestData/test.zip", FileMode.Open, FileAccess.Read, FileShare.Read);
            using var zip = new ZipExtractor(fs);

            zip.Extract("/", $"{TestContext.CurrentContext.TestDirectory}/out/root");
        }
    }
}
