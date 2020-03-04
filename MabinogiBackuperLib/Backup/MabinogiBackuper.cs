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
        public int Percentage => (int)Math.Round((double)Current / Total * 100);
        public string Name { get; set; }
    }

    public class Backupper
    {

        public RegistryCollector RegistryCollector { get; set; } = new RegistryCollector();

        public IObservable<IProgressEventArgs> CreateRegistryBackupProgress => _createRegistryBackupProgress;
        public IObservable<IProgressEventArgs> BackupFileAnalyzeProgress => _backupFileAnalyzeProgress;
        public IObservable<ZipConsidateEventArgs> BackupProgress => _backupProgress;

        private readonly Subject<IProgressEventArgs> _createRegistryBackupProgress = new Subject<IProgressEventArgs>();
        private readonly Subject<IProgressEventArgs> _backupFileAnalyzeProgress = new Subject<IProgressEventArgs>();
        private readonly Subject<ZipConsidateEventArgs> _backupProgress = new Subject<ZipConsidateEventArgs>();
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

        public long CalculatedSize()
        {
            return FileFunctions.FileSize.TotalFileSize(_files) + _registryJson.Length;
        }

        public void Backup(Stream savedStream)
        {
            using var zip = new ZipConsolidator(savedStream);
            foreach (var file in _files)
            {
                zip.Add(ZipConsolidator.CreateEntryName(file, _personalDirectoryPath), file);
            }
            
            if (!string.IsNullOrEmpty(_registryJson))
                zip.Add("Registry.json", Encoding.UTF8.GetBytes(_registryJson));

            zip.Consolidate(_backupProgress.OnNext);
            GC.Collect();
            //zip.Consolidate(_files, _personalDirectoryPath, _backupProgress.OnNext);
        }
    }
}
