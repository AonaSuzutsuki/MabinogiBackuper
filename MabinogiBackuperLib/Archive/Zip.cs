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

namespace MabinogiBackuperLib.Archive
{
    public class ZipConsolidator : IDisposable
    {

        #region Fields

        private Stream _destinationStream;

        private CompressionLevel _compressionLevel;

        private ZipArchive _archive;

        #endregion

        #region Event
        public class ConsolidateProgressEventArgs : EventArgs
        {
            public int Total { get; set; }
            public int Current { get; set; }
            public int Percentage => (Current / Total) * 100;
            public string FilePath { get; set; }
        }

        public delegate void ConsolidateProgressChangedEventHandler(object sender, ConsolidateProgressEventArgs e);

        public event ConsolidateProgressChangedEventHandler Consolidated;
        protected virtual void OnConsolidated(ConsolidateProgressEventArgs e)
        {
            Consolidated?.Invoke(this, e);
        }
        #endregion

        public ZipConsolidator(Stream destinationStream, CompressionLevel level = CompressionLevel.Fastest)
        {
            _destinationStream = destinationStream;
            _compressionLevel = level;
            _archive = new ZipArchive(destinationStream, ZipArchiveMode.Create, true);
        }

        public void Consolidate(IEnumerable<string> files, string basePath)
        {
            var filesArray = files as string[] ?? files.ToArray();
            var entries = (from file in filesArray select CreateEntryName(file, basePath)).ToArray();
            var tuples = entries.Zip(filesArray, (entry, file) => new { Entry = entry, File = file }).ToArray();
            foreach (var tuple in tuples.Select((v, i) => new { Index = i, Value = v }))
            {
                Consolidate(tuple.Value.File, tuple.Value.Entry);
                OnConsolidated(new ConsolidateProgressEventArgs
                {
                    Total = tuples.Length,
                    Current = tuple.Index + 1,
                    FilePath = tuple.Value.File
                });
            }
        }

        public void Consolidate(string file, string entry)
        {
            _archive.CreateEntryFromFile(file, entry, _compressionLevel);
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
                zip.Consolidate(files, basePath);
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

        public ZipExtractor(Stream stream)
        {
            _archive = new ZipArchive(stream, ZipArchiveMode.Read, true);
            Initialize();
        }

        public ZipExtractor(string zipPath)
        {
            _archive = ZipFile.OpenRead(zipPath);
            Initialize();
        }

        private void Initialize()
        {
            foreach (var entry in _archive.Entries)
            {
                var elem = Path.GetFileName(entry.FullName);
                var dir = Path.GetDirectoryName(entry.FullName).Replace("\\", "/");
                _root.AppendItem(dir, elem, entry);
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
