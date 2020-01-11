using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonCoreLib.CommonPath;

namespace MabinogiBackuperLib.Archive
{
    public class ZipConsolidator
    {
        public MemoryStream Consolidate(IEnumerable<string> files, string basePath)
        {
            var ms = new MemoryStream();
            using var archive = new ZipArchive(ms, ZipArchiveMode.Create);
            foreach (var file in files)
            {
                var entryName = CreateEntryName(file, basePath);
                //var entry = archive.CreateEntryFromFile(file, entryName);
            }

            return ms;
        }

        public static string CreateEntryName(string filePath, string basePath)
        {
            basePath = basePath.UnifiedSystemPathSeparator();
            var file = filePath.UnifiedSystemPathSeparator();
            if (basePath.Last() != '\\')
                basePath += "\\";
            var entryName = file.Replace(basePath, "").Replace("\\", "/");
            Console.WriteLine(entryName);
            return entryName;
        }
    }

    public class ZipExtractor
    {
        public int Count => archive.Entries.Count;

        #region Event
        public class ExtractProgressEventArgs : EventArgs
        {
            public int Total { get; set; }
            public string FilePath { get; set; }
        }

        public delegate void ExtractProgressChangedEventHandler(object sender, ExtractProgressEventArgs e);

        public event ExtractProgressChangedEventHandler Extracted;
        protected virtual void OnExtracted(ExtractProgressEventArgs e)
        {
            Extracted?.Invoke(this, e);
        }
        #endregion

        private readonly ZipArchive archive;
        private readonly string extractDirPath = string.Empty;

        public ZipExtractor(MemoryStream stream, string extractDirPath)
        {
            if (string.IsNullOrEmpty(extractDirPath))
                throw new ArgumentNullException();

            this.extractDirPath = extractDirPath;
            archive = new ZipArchive(stream, ZipArchiveMode.Read, true);
        }

        public ZipExtractor(string zipPath, string extractDirPath)
        {
            if (string.IsNullOrEmpty(zipPath) || string.IsNullOrEmpty(extractDirPath))
                throw new ArgumentNullException();

            this.extractDirPath = extractDirPath;
            archive = ZipFile.OpenRead(zipPath);
        }

        public void Extract()
        {
            foreach (var entry in archive.Entries)
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

        public void Dispose()
        {
            archive.Dispose();
        }
    }
}
