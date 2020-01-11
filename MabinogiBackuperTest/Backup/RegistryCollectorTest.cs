using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MabinogiBackuperLib.Backup;
using MabinogiBackuperLib.ExRegistry;
using Microsoft.Win32;
using Moq;
using NUnit.Framework;

namespace MabinogiBackuperTest.Backup
{
    [TestFixture]
    public class RegistryCollectorTest
    {
        [Test]
        public void GetJsonTest()
        {
            var registryPath = @"Software\Nexon\Mabinogi";
            var exp = "{\"ThemePack\":\"ネオンピンク\",\"ThemePackColorSet\":\"半透明\",\"TpsMode\":\"0\"}";

            var moq = new Mock<IRegistryEditor>();
            moq.Setup(x => x.GetKeyNames(registryPath)).Returns(new []
            {
                "ThemePack", "ThemePackColorSet", "TpsMode"
            });
            moq.Setup(x => x.GetValue(registryPath, "ThemePack", Registry.CurrentUser)).Returns("ネオンピンク");
            moq.Setup(x => x.GetValue(registryPath, "ThemePackColorSet", Registry.CurrentUser)).Returns("半透明");
            moq.Setup(x => x.GetValue(registryPath, "TpsMode", Registry.CurrentUser)).Returns("0");
            var regEdit = moq.Object;

            var collector = new RegistryCollector
            {
                RegistryEditor = regEdit
            };
            var json = collector.GetJson();

            Assert.AreEqual(exp, json);
        }
    }
}
