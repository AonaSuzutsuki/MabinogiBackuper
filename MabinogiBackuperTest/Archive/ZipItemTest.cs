using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MabinogiBackuperLib.Archive;
using NUnit.Framework;

namespace MabinogiBackuperTest.Archive
{
    [TestFixture]
    public class ZipItemTest
    {
        private ZipItem CreateTestData()
        {
            var zipItem = new ZipItem
            {
                ZipItemType = ItemType.Root,
                Name = "root",
                Files = 
                {
                    new ZipItem
                    {
                        Name = "dir"
                    },
                    new ZipItem
                    {
                        Name = "dir2",
                        Files =
                        {
                            new ZipItem
                            {
                                Name = "sub",
                                Files =
                                {
                                    new ZipItem
                                    {
                                        ZipItemType = ItemType.File,
                                        Name = "more.txt"
                                    }
                                }
                            }
                        }
                    }
                }
            };
            SetParent(zipItem);
            return zipItem;
        }

        [Test]
        public void AppendItemTest()
        {
            var zipItem = new ZipItem();
            zipItem.AppendItem("dir2/sub/", "test.zip", null);

            var act = zipItem.Exists("/dir2/sub/test.zip");

            var exp = new ZipItem
            {
                ZipItemType = ItemType.File,
                Parent = zipItem.Exists("/dir2/sub"),
                Name = "test.zip"
            };

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void AppendItemTest2()
        {
            var zipItem = new ZipItem();
            zipItem.AppendItem("", "test.zip", null);

            var act = zipItem.Exists("/test.zip");

            var exp = new ZipItem
            {
                ZipItemType = ItemType.File,
                Parent = zipItem,
                Name = "test.zip"
            };

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ZipItemExistsTest()
        {
            var zipItem = CreateTestData();
            var exp = zipItem.Files.Last().Files.First();

            var item = zipItem.Exists("/dir2/sub/");

            Assert.AreEqual(exp, item);
        }

        [Test]
        public void CreatePathTest()
        {
            var zipItem = CreateTestData();

            var exp = "dir2/sub/more.txt";
            var path = zipItem.Files.Last().Files.First().Files.First().ToString();

            Assert.AreEqual(exp, path);
        }

        [Test]
        public void HierarchyTest()
        {
            var zipItem = CreateTestData();

            var item = zipItem.Files.Last().Files.First().Files.First();
            var value = item.Hierarchy();
            var exp = 3;

            Assert.AreEqual(exp, value);
        }

        private static void SetParent(ZipItem zipItem)
        {
            foreach (var item in zipItem.Files)
            {
                item.Parent = zipItem;
                SetParent(item);
            }
        }
    }
}
