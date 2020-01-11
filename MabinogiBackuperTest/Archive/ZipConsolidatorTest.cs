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
        public void CreateEntryNameTest()
        {
            var di = new DirectoryInfo($"{TestContext.CurrentContext.TestDirectory}/TestData".UnifiedSystemPathSeparator());
            var files = di.GetFiles("*", SearchOption.AllDirectories).Select(x => x.FullName);

            foreach (var file in files)
            {
                ZipConsolidator.CreateEntryName(file, di.FullName);
            }
        }
    }
}
