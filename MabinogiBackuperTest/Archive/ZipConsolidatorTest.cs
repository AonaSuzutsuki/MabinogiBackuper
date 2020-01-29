using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonCoreLib.CommonPath;
using MabinogiBackuperLib.Archive;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace MabinogiBackuperTest.Archive
{
    [TestFixture]
    public class ZipConsolidatorTest
    {
        [Test]
        public void ConsolidateTest()
        {
            var di = new DirectoryInfo($"{TestContext.CurrentContext.TestDirectory}/TestData/compress".UnifiedSystemPathSeparator());
            var files = di.GetFiles("*", SearchOption.AllDirectories).Select(x => x.FullName);

            using var ms = ZipConsolidator.ConsolidateStatic(files, di.FullName);
            using var fs = new FileStream($"{TestContext.CurrentContext.TestDirectory}/TestData/test.zip", FileMode.Open, FileAccess.Read, FileShare.Read);

            var actData = new byte[ms.Length];
            var expData = new byte[fs.Length];

            ms.Read(actData, 0, actData.Length);
            fs.Read(expData, 0, expData.Length);

            CollectionAssert.AreEqual(expData, actData);

            //using var fs = new FileStream($"{TestContext.CurrentContext.TestDirectory}/test.zip", FileMode.Create, FileAccess.Write, FileShare.None);
            //ms.CopyTo(fs);
        }

        [Test]
        public void CreateEntryNameTest()
        {
            var di = new DirectoryInfo($"{TestContext.CurrentContext.TestDirectory}/TestData/compress".UnifiedSystemPathSeparator());
            var files = di.GetFiles("*", SearchOption.AllDirectories).Select(x => x.FullName);

            var exp = new[]
            {
                "TextFile1.txt",
                "sub/TextFile2.txt",
                "sub/subsub/TextFile3.txt"
            };
            var act = from file in files select ZipConsolidator.CreateEntryName(file, di.FullName);

            CollectionAssert.AreEqual(exp, act);
        }

        [Test]
        public void GetEntryDirectoryTest()
        {
            var values = new[]
            {
                "TextFile1.txt",
                "sub/sub/TextFile3.txt",
                "sub/sub2/TextFile1.txt",
                "sub/TextFile1.txt",
                "sub/TextFile2.txt",
                "sub/sub2/TextFile2.txt"
            };
            var exp = new[]
            {
                "sub/",
                "sub/sub/",
                "sub/sub2/"
            };
            var act = ZipConsolidator.GetEntryDirectories(values);
        }
    }
}
