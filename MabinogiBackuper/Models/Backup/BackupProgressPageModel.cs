using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MabinogiBackuperLib.Archive;
using MabinogiBackuperLib.Backup;
using MabinogiBackuperLib.FileFunctions;
using Prism.Mvvm;

namespace MabinogiBackuper.Models.Backup
{
    public class BackupProgressPageModel : BindableBase
    {
        public enum ProgressMode
        {
            Analyze,
            Backup
        }

        private readonly Backupper _backupper;
        private readonly BackupShare _share;

        private int _progressValue;
        private string _progressLabel;


        public int ProgressValue
        {
            get => _progressValue;
            set => SetProperty(ref _progressValue, value);
        }

        public string ProgressLabel
        {
            get => _progressLabel;
            set => SetProperty(ref _progressLabel, value);
        }

        public BackupProgressPageModel(BackupShare share)
        {
            _share = share;
            _backupper = share.Backupper;

            _backupper.BackupProgress.Subscribe(
                onNext: args => ProgressChanged(ProgressMode.Backup, args, 1, 1),
                onCompleted: () => { });
        }

        public async Task Analyze()
        {
            if (string.IsNullOrEmpty(_share.SavedPath))
                return;

            await Task.Factory.StartNew(() =>
            {
                using (var fs = new FileStream(_share.SavedPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    _backupper.Backup(fs);
                }
            });
        }

        public void ProgressChanged(ProgressMode mode, ZipConsidateEventArgs eventArgs, int current, int maxPhase)
        {
            var (sizeType, converted) = FileSize.ConvertToString(eventArgs.TotalSize);
            var currentSize = FileSize.ConvertToString(eventArgs.CurrentSize, sizeType);

            if (eventArgs.TotalSize > 0)
                ProgressLabel = $"{mode} {current}/{maxPhase}: {eventArgs.Percentage}% {currentSize}/{converted} {eventArgs.Name}";
            else
                ProgressLabel = $"{mode} {current}/{maxPhase}: {eventArgs.Percentage}% {eventArgs.Name}";

            ProgressValue = eventArgs.Percentage;
        }
    }
}
