using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using MabinogiBackuperLib.Archive;

namespace MabinogiBackuperLib.Backup
{
    public class BackupProgressEventArgs : IProgressEventArgs
    {
        public int Total { get; set; }
        public int Current { get; set; }
        public int Percentage => (int)((double)Current / Total * 100);
        public string Name { get; set; }
    }

    public class Backupper
    {

        public RegistryCollector RegistryCollector { get; set; } = new RegistryCollector();

        public IObservable<RegistryEventArgs> CreateRegistryBackupProgress => _createRegistryBackupProgress;
        public IObservable<BackupProgressEventArgs> BackupFileAnalyzeProgress => _backupFileAnalyzeProgress;

        private readonly Subject<RegistryEventArgs> _createRegistryBackupProgress = new Subject<RegistryEventArgs>();
        private readonly Subject<BackupProgressEventArgs> _backupFileAnalyzeProgress = new Subject<BackupProgressEventArgs>();
        private readonly string _personalDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

        private List<string> _files = new List<string>();
        private string _registryJson = string.Empty;

        public void CreateRegistryBackup()
        {
            _registryJson = RegistryCollector.GetJson(_createRegistryBackupProgress.OnNext);
            _createRegistryBackupProgress.OnCompleted();
        }

        public void BackupFilePathItems(IList<string> dirItems)
        {
            var list = new List<string>();
            foreach (var item in dirItems.Select((v, i) => new { Index = i, Value = v }))
            {
                var files = CommonCoreLib.CommonFile.DirectorySearcher.GetAllFiles($"{_personalDirectoryPath}{item.Value}");
                list.AddRange(files);
                _backupFileAnalyzeProgress.OnNext(new BackupProgressEventArgs
                {
                    Total = dirItems.Count,
                    Current = item.Index + 1,
                    Name = item.Value
                });
            }
            _backupFileAnalyzeProgress.OnCompleted();

            _files = list;
        }

        public MemoryStream BackupMemoryStream()
        {
            var ms = new MemoryStream();
            using (var zip = new ZipConsolidator(ms))
            {
                zip.Consolidate(_files, _personalDirectoryPath);
            }

            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }
    }
}
