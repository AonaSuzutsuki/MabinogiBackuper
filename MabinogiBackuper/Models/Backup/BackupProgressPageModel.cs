using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MabinogiBackuperLib.Backup;
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

        private readonly Backupper _backupper = new Backupper();
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

            _backupper.BackupProgress.Subscribe(
                onNext: args => ProgressChanged(ProgressMode.Backup, args, 1, 1));
        }

        public void Analyze()
        {
            if (string.IsNullOrEmpty(_share.SavedPath))
                return;

            Task.Factory.StartNew(() =>
            {

                using (var fs = new FileStream(_share.SavedPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    _backupper.Backup(fs);
                }
            });
        }

        public void ProgressChanged(ProgressMode mode, IProgressEventArgs eventArgs, int current, int maxPhase)
        {
            ProgressLabel = $"{mode} {current}/{maxPhase}: {eventArgs.Percentage}% {eventArgs.Name}";
            ProgressValue = eventArgs.Percentage;
        }
    }
}
