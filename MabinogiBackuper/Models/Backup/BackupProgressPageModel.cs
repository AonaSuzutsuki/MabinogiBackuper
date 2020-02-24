using System;
using System.Collections.Generic;
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
            var totalPhase = 2;
            totalPhase = share.CheckedCount() > 0 ? totalPhase : totalPhase - 1;

            _backupper.CreateRegistryBackupProgress.Subscribe(
                onNext: args => ProgressChanged(ProgressMode.Analyze, args, 1, totalPhase), onCompleted: () => ProgessCompleted("Completed."));
            _backupper.BackupFileAnalyzeProgress.Subscribe(
                onNext: args => ProgressChanged(ProgressMode.Analyze, args, 2, totalPhase));
        }

        public void Analyze()
        {
            Task.Factory.StartNew(() =>
            {
                _backupper.CreateRegistryBackup();

                var targetDirs = _share.CheckedPathList();
                _backupper.BackupFilePathItems(targetDirs);
            });
        }

        public void ProgressChanged(ProgressMode mode, IProgressEventArgs eventArgs, int current, int maxPhase)
        {
            ProgressLabel = $"{mode} {current}/{maxPhase}: {eventArgs.Percentage}%";
            ProgressValue = eventArgs.Percentage;
        }

        public void ProgessCompleted(string message)
        {
            //ProgressLabel = message;
        }
    }
}
