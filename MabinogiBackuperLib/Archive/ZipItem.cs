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

        public ZipArchiveEntry ZipEntry { get; set; }

        public void AppendItem(string dirPath, string name, ZipArchiveEntry entry)
        {
            var array = dirPath.Split('/');
            if (array.Length < 1)
                return;

            var dir = string.IsNullOrEmpty(array[0]) ? this : AppendItem(new Queue<string>(array), this);
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
            list.Remove("");
            var queue = new Queue<string>(list);
            if (type == SearchType.Directory)
                return DirectoryExists(queue, this);
            return FileExists(queue, this);
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

        public ZipItem FileExists(Queue<string> pathQueue, ZipItem zipItem)
        {
            if (zipItem == null)
                return null;

            if (pathQueue.Count > 0)
            {
                var path = pathQueue.Dequeue();
                if (pathQueue.Count > 0)
                {
                    foreach (var item in zipItem.Directories)
                    {
                        if (item.Name == path)
                            return item.DirectoryExists(pathQueue, item);
                    }
                }
                else
                {
                    foreach (var item in zipItem.Files)
                    {
                        if (item.Name == path)
                            return item;
                    }
                }

                return null;
            }

            return zipItem;
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
            boolCollector.ChangeBool(nameof(Directories), Directories.SequenceEqual(zipItem.Directories));
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
