using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using CommonCoreLib.CommonPath;
using MabinogiBackuperLib.Backup;

namespace MabinogiBackuperLib.Archive
{
    public class ZipConsidateEventArgs : IProgressEventArgs
    {
        public int Total { get; set; }
        public int Current { get; set; }
        public int Percentage => (int)Math.Round((double)Current / Total * 100);
        public string Name { get; set; }

        public long TotalSize { get; set; }
        public long CurrentSize { get; set; }
    }

    public class ZipExtractorEventArgs : IProgressEventArgs
    {
        public int Total { get; set; }
        public int Current { get; set; }
        public int Percentage => (int)Math.Round((double)Current / Total * 100);
        public string Name { get; set; }
    }

    public class ZipConsolidatorItemInfo
    {
        public enum ItemType
        {
            File,
            Binary
        }

        public ItemType Type { get; set; }
        public string EntryName { get; set; }
        public string FilePath { get; set; }
        public byte[] Content { get; set; }
    }

    public class ZipConsolidator : IDisposable
    {

        #region Fields

        private Stream _destinationStream;

        private CompressionLevel _compressionLevel;

        private ZipArchive _archive;

        private Dictionary<string, ZipConsolidatorItemInfo> _items { get; } = new Dictionary<string, ZipConsolidatorItemInfo>();

        #endregion


        public ZipConsolidator(Stream destinationStream, CompressionLevel level = CompressionLevel.Fastest)
        {
            _destinationStream = destinationStream;
            _compressionLevel = level;
            _archive = new ZipArchive(destinationStream, ZipArchiveMode.Create, true);
        }

        public void Add(string entryName, string filePath)
        {
            _items.Add(entryName, new ZipConsolidatorItemInfo
            {
                Type = ZipConsolidatorItemInfo.ItemType.File,
                EntryName = entryName,
                FilePath = filePath
            });
        }

        public void Add(string entryName, byte[] data)
        {
            _items.Add(entryName, new ZipConsolidatorItemInfo
            {
                Type = ZipConsolidatorItemInfo.ItemType.Binary,
                EntryName = entryName,
                Content = data
            });
        }

        public void Consolidate(Action<ZipConsidateEventArgs> callback)
        {
            foreach (var tuple in _items.Values.Select((v, i) => new { Index = i, Value = v }))
            {
                var eventArgs = new ZipConsidateEventArgs
                {
                    Total = _items.Count,
                    Current = tuple.Index + 1,
                    Name = tuple.Value.EntryName
                };

                if (tuple.Value.Type == ZipConsolidatorItemInfo.ItemType.File)
                    Consolidate(tuple.Value.FilePath, tuple.Value.EntryName, callback, eventArgs);
                else
                    Consolidate(tuple.Value.Content, tuple.Value.EntryName);

                callback?.Invoke(eventArgs);
            }
        }

        public void Consolidate(IEnumerable<string> files, string basePath, Action<IProgressEventArgs> callback)
        {
            var filesArray = files as string[] ?? files.ToArray();
            var entries = (from file in filesArray select CreateEntryName(file, basePath)).ToArray();
            var tuples = entries.Zip(filesArray, (entry, file) => new { Entry = entry, File = file }).ToArray();
            foreach (var tuple in tuples.Select((v, i) => new { Index = i, Value = v }))
            {
                Consolidate(tuple.Value.File, tuple.Value.Entry);
                callback?.Invoke(new ZipConsidateEventArgs
                {
                    Total = tuples.Length,
                    Current = tuple.Index + 1,
                    Name = tuple.Value.File
                });
            }
        }

        public void Consolidate(byte[] data, string entryName)
        {
            var entry = _archive.CreateEntry(entryName, _compressionLevel);
            using (var stream = entry.Open())
            {
                stream.Write(data, 0, data.Length);
            }
        }

        public void Consolidate(string file, string entryName, Action<ZipConsidateEventArgs> callback = null, ZipConsidateEventArgs eventArgs = null)
        {
            var fi = new FileInfo(file);
            if (fi.Length >= 128 * 1024 * 1024)
            {
                var entry = _archive.CreateEntry(entryName, _compressionLevel);

                var buffer = new byte[128 * 1024 * 1024];
                using (var fs = new FileStream(file, FileMode.Open))
                {
                    using (var stream = entry.Open())
                    {
                        eventArgs.TotalSize = fi.Length;
                        var copying = true;
                        while (copying)
                        {
                            var bytesRead = fs.Read(buffer, 0, buffer.Length);
                            if (bytesRead > 0)
                            {
                                stream.Write(buffer, 0, bytesRead);
                                eventArgs.CurrentSize += bytesRead;
                                callback(eventArgs);
                            }
                            else
                            {
                                copying = false;
                            }
                        }
                    }
                }
            }
            else
            {
                _archive.CreateEntryFromFile(file, entryName, _compressionLevel);
            }
        }

        public void Dispose()
        {
            _archive.Dispose();
        }



        public static MemoryStream ConsolidateStatic(IEnumerable<string> files, string basePath)
        {
            var ms = new MemoryStream();
            using (var zip = new ZipConsolidator(ms))
            {
                foreach (var file in files)
                {
                    zip.Add(CreateEntryName(file, basePath), file);
                }
                zip.Consolidate(null);
            }

            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }

        public static string CreateEntryName(string filePath, string basePath)
        {
            basePath = basePath.UnifiedSystemPathSeparator();
            var file = filePath.UnifiedSystemPathSeparator();
            if (basePath.Last() != '\\')
                basePath += "\\";
            var entryName = file.Replace(basePath, "").Replace("\\", "/");
            return entryName;
        }

        public static string GetEntryDirectory(string entry)
        {
            var items = new List<string>(entry.Split('/'));
            items.RemoveAt(items.Count - 1);

            var sb = new StringBuilder();
            foreach (var item in items)
            {
                sb.Append($"{item}/");
            }

            return sb.ToString();
        }

        public static IEnumerable<string> GetEntryDirectories(IEnumerable<string> entries)
        {
            var set = new HashSet<string>();
            foreach (var entry in entries)
            {
                var dir = GetEntryDirectory(entry);
                if (!string.IsNullOrEmpty(dir))
                    set.Add(dir);
            }

            var list = new List<string>(set);
            list.Sort();
            return list;
        }
    }

    public class ZipExtractor : IDisposable
    {
        public int Count => _archive.Entries.Count;

        #region Event
        public class ExtractProgressEventArgs : EventArgs
        {
            public int Total { get; set; }
            public int Current { get; set; }
            public string FilePath { get; set; }
        }

        public delegate void ExtractProgressChangedEventHandler(object sender, ExtractProgressEventArgs e);

        public event ExtractProgressChangedEventHandler Extracted;
        protected virtual void OnExtracted(ExtractProgressEventArgs e)
        {
            Extracted?.Invoke(this, e);
        }
        #endregion

        private readonly ZipArchive _archive;
        private readonly ZipItem _root = new ZipItem();

        public ZipItem Root => _root;

        public ZipExtractor(Stream stream)
        {
            _archive = new ZipArchive(stream, ZipArchiveMode.Read, true);
        }

        public ZipExtractor(string zipPath)
        {
            _archive = ZipFile.OpenRead(zipPath);
        }

        public void Initialize(Action<IProgressEventArgs> callBack = null)
        {
            foreach (var item in _archive.Entries.Select((v, i) => new { Value = v, Index = i }))
            {
                var entry = item.Value;
                if (!entry.FullName.EndsWith("/"))
                {
                    var elem = Path.GetFileName(entry.FullName);
                    var dir = Path.GetDirectoryName(entry.FullName).Replace("\\", "/");
                    _root.AppendItem(dir, elem, entry);

                    callBack?.Invoke(new ZipExtractorEventArgs
                    {
                        Name = elem,
                        Current = item.Index + 1,
                        Total = _archive.Entries.Count
                    });
                }
            }
        }

        public void Extract(string extractDirPath)
        {
            foreach (var entry in _archive.Entries)
            {
                var path = Path.Combine(extractDirPath, entry.FullName);
                if (path.Substring(path.Length - 1, 1) == "/")
                {
                    var di = new DirectoryInfo(path);
                    if (!di.Exists)
                        di.Create();
                }
                else
                {
                    entry.ExtractToFile(Path.Combine(extractDirPath, entry.FullName), true);
                }

                OnExtracted(new ExtractProgressEventArgs
                {
                    Total = Count,
                    FilePath = entry.FullName
                });
            }
        }

        public void Extract(string entryName, string extractDirPath)
        {
            var item = _root.Exists(entryName);

            var extractDirInfo = new DirectoryInfo(extractDirPath);
            if (!extractDirInfo.Exists)
                extractDirInfo.Create();

            if (item.ZipItemType == ItemType.File)
            {
                Extract(item.Parent, item, extractDirPath, new ExtractProgressEventArgs
                {
                    Total = item.FileCount()
                });
            }
            else
            {
                Extract(item, item, extractDirPath, new ExtractProgressEventArgs
                {
                    Total = item.FileCount()
                });
            }
        }

        public void Extract(ZipItem root, ZipItem zipItem, string extractDirPath, ExtractProgressEventArgs eventArgs)
        {
            if (zipItem == null)
                return;

            if (zipItem.ZipItemType == ItemType.File)
            {
                var entry = zipItem.ZipEntry;
                var hierarchy = root.Hierarchy();

                var pathArray = zipItem.ToString().Split('/');
                var pathList = (from x in pathArray where !string.IsNullOrEmpty(x) select x).ToList();
                var pathQueue = new Queue(pathList);
                for (int i = 0; i < hierarchy; i++)
                    pathQueue.Dequeue();

                var path = string.Join("/", pathQueue.ToArray()); // /subsub/が/subになる
                var fullPath = $"{extractDirPath}/{path}".UnifiedSystemPathSeparator();
                var di = new DirectoryInfo(Path.GetDirectoryName(fullPath));
                if (!di.Exists)
                    di.Create();
                if (!File.Exists(fullPath))
                {
                    entry.ExtractToFile(fullPath);

                    eventArgs.Current += 1;
                    eventArgs.FilePath += entry.FullName;
                    OnExtracted(eventArgs);
                }
            }
            else
            {
                foreach (var zipItemFile in zipItem.Files)
                {
                    Extract(root, zipItemFile, extractDirPath, eventArgs);
                }

                foreach (var directory in zipItem.Directories)
                {
                    Extract(root, directory, extractDirPath, eventArgs);
                }
            }
        }

        public void Dispose()
        {
            _archive.Dispose();
        }
    }
}
