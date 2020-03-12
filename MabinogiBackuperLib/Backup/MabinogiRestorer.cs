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
    public class MabinogiRestorer : AbstractMabinogi, IDisposable
    {

        #region Fields

        private readonly string _backupFile;
        private readonly FileStream _fileStream;
        private readonly ZipExtractor _zip;

        private readonly Subject<IProgressEventArgs> _backupFileAnalyzeProgress = new Subject<IProgressEventArgs>();

        #endregion

        #region Properties

        public IObservable<IProgressEventArgs> BackupFileAnalyzeProgress => _backupFileAnalyzeProgress;

        #endregion

        public MabinogiRestorer(string backupFile)
        {
            if (string.IsNullOrEmpty(backupFile) || !File.Exists(backupFile))
                throw new FileNotFoundException();

            _backupFile = backupFile;

            _fileStream = new FileStream(_backupFile, FileMode.Open, FileAccess.Read, FileShare.None);
            _zip = new ZipExtractor(_fileStream);
        }

        public long Analyze()
        {
            _zip.Initialize(_backupFileAnalyzeProgress.OnNext);
            _backupFileAnalyzeProgress.OnCompleted();

            var zipItem = _zip.Root;

            var size = zipItem.ExtractedSize();
            return size;
        }

        public long GetDriveFreeSize()
        {
            return GetDriveFreeSize(_backupFile);
        }

        public void Dispose()
        {
            _zip?.Dispose();
            _fileStream?.Dispose();
        }
    }
}
