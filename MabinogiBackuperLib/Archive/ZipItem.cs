using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonCoreLib.Bool;

namespace MabinogiBackuperLib.Archive
{
    public enum ItemType
    {
        Root,
        Directory,
        File
    }

    public enum SearchType
    {
        Directory,
        File
    }

    public class ZipItem
    {
        public ItemType ZipItemType { get; set; } = ItemType.Directory;
        public ZipItem Parent { get; set; }
        public List<ZipItem> Files { get; set; } = new List<ZipItem>();

        public List<ZipItem> Directories { get; set; } = new List<ZipItem>();

        public string Name { get; set; }

        internal ZipArchiveEntry ZipEntry { get; set; }

        public void AppendItem(string dirPath, string name, ZipArchiveEntry entry)
        {
            var list = new List<string>(dirPath.Split('/'));
            var list2 = (from x in list where !string.IsNullOrEmpty(x) select x).ToList();

            var dir = list2.Count <= 0 || string.IsNullOrEmpty(list2[0]) ? this : AppendItem(new Queue<string>(list2), this);
            dir.Files.Add(new ZipItem
            {
                ZipItemType = ItemType.File,
                Parent = dir,
                Name = name,
                ZipEntry = entry
            });
        }

        public ZipItem AppendItem(Queue<string> queue, ZipItem zipItem)
        {
            if (zipItem == null)
                return null;
            if (queue.Count < 1)
                return zipItem;

            var path = queue.Dequeue();
            foreach (var item in zipItem.Directories)
            {
                if (item.Name == path)
                {
                    return AppendItem(queue, item);
                }
            }

            var newItem = new ZipItem
            {
                ZipItemType = ItemType.Directory,
                Parent = zipItem,
                Name = path
            };
            zipItem.Directories.Add(newItem);
            return AppendItem(queue, newItem);
        }

        public ZipItem Exists(string path)
        {
            if (path == "/")
                return this;

            var type = path.EndsWith("/") ? SearchType.Directory : SearchType.File;

            var list = new List<string>(path.Split('/'));
            var list2 = (from x in list where !string.IsNullOrEmpty(x) select x).ToList();

            if (type == SearchType.Directory)
            {
                var queue = new Queue<string>(list2);
                return DirectoryExists(queue, this);
            }
            else
            {
                var fileName = list2[list2.Count - 1];
                list2.RemoveAt(list2.Count - 1);;
                var queue = new Queue<string>(list2);
                return FileExists(queue, fileName, this);
            }
        }

        public ZipItem DirectoryExists(Queue<string> pathQueue, ZipItem zipItem)
        {
            if (zipItem == null)
                return null;

            if (pathQueue.Count > 0)
            {
                var path = pathQueue.Dequeue();
                foreach (var item in zipItem.Directories)
                {
                    if (item.Name == path)
                        return item.DirectoryExists(pathQueue, item);
                }

                return null;
            }

            return zipItem;
        }

        public ZipItem FileExists(Queue<string> pathQueue, string fileName, ZipItem zipItem)
        {
            if (zipItem == null)
                return null;

            if (pathQueue.Count > 0)
            {
                var path = pathQueue.Dequeue();
                foreach (var item in zipItem.Directories)
                {
                    if (item.Name == path)
                        zipItem = item.DirectoryExists(pathQueue, item);
                }
            }

            foreach (var item in zipItem.Files)
            {
                if (item.Name == fileName)
                    return item;
            }

            return null;
        }

        public long ExtractedSize()
        {
            return ExtractedSize(this);
        }

        public long ExtractedSize(ZipItem zipItem)
        {
            if (zipItem.ZipItemType == ItemType.File)
                return zipItem.ZipEntry.Length;

            return zipItem.Files.Sum(ExtractedSize) + zipItem.Directories.Sum(ExtractedSize);
        }

        public int FileCount(ZipItem zipItem)
        {
            var cnt = zipItem.Files.Count;
            foreach (var directory in zipItem.Directories)
            {
                cnt += FileCount(directory);
            }

            return cnt;
        }
        public int FileCount()
        {
            return FileCount(this);
        }

        public int Hierarchy(ZipItem zipItem, int hierarchy)
        {
            var parent = zipItem.Parent;
            if (parent == null)
            {
                return hierarchy;
            }

            return Hierarchy(parent, hierarchy + 1);
        }

        public int Hierarchy()
        {
            return Hierarchy(this, 0);
        }

        public static Stack<string> CreatePath(ZipItem zipItem, Stack<string> stack = null)
        {
            if (zipItem == null)
                return stack;
            if (stack == null)
                stack = new Stack<string>();

            if (zipItem.ZipItemType != ItemType.Root)
                stack.Push(zipItem.Name);
            return CreatePath(zipItem.Parent, stack);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var zipItem = (ZipItem)obj;
            var boolCollector = new BoolCollector();

            boolCollector.ChangeBool(nameof(ZipItemType), ZipItemType == zipItem.ZipItemType);
            boolCollector.ChangeBool(nameof(Files), Files.SequenceEqual(zipItem.Files));
            //boolCollector.ChangeBool(nameof(Directories), Directories.SequenceEqual(zipItem.Directories));
            boolCollector.ChangeBool(nameof(Parent), Parent == zipItem.Parent);
            boolCollector.ChangeBool(nameof(Name), Name == zipItem.Name);

            return boolCollector.Value;
        }

        public override string ToString()
        {
            var stack = CreatePath(this);
            return string.Join("/", stack);
        }
    }
}
