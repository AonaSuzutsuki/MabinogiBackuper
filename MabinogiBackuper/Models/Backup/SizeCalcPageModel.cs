using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MabinogiBackuperLib.Backup;
using Prism.Mvvm;

namespace MabinogiBackuper.Models.Backup
{
    public class SizeCalcPageModel : BindableBase
    {

        private readonly BackupShare _share;
        private readonly Backupper _backupper;

        private Visibility _progressVisibility;
        private Visibility _messageVisibility;
        private int _progressValue;
        private string _progressLabel;
        private string _message;


        public Visibility ProgressVisibility
        {
            get => _progressVisibility;
            set => SetProperty(ref _progressVisibility, value);
        }

        public Visibility MessageVisibility
        {
            get => _messageVisibility;
            set => SetProperty(ref _messageVisibility, value);
        }

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

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }



        public SizeCalcPageModel(BackupShare share)
        {
            _share = share;
            _backupper = share.Backupper;

            var totalPhase = 2;
            totalPhase = share.CheckedCount() > 0 ? totalPhase : totalPhase - 1;

            _backupper.CreateRegistryBackupProgress.Subscribe(
                onNext: args => ProgressChanged(args, 1, totalPhase));
            _backupper.BackupFileAnalyzeProgress.Subscribe(
                onNext: args => ProgressChanged(args, 2, totalPhase));
        }

        public async Task Analyze()
        {
            await Task.Factory.StartNew(() =>
            {
                _backupper.CreateRegistryBackup();

                var targetDirs = _share.CheckedPathList();
                _backupper.BackupFilePathItems(targetDirs);

                _share.AnalyzedSize = _backupper.CalculatedSize();

                Completed();
            });
        }

        private void ProgressChanged(IProgressEventArgs eventArgs, int current, int totalPhase)
        {
            ProgressLabel = $"Analyze {current}/{totalPhase}: {eventArgs.Percentage}% {eventArgs.Name}";
            ProgressValue = eventArgs.Percentage;
        }

        private void Completed()
        {
            var gigaBytes = ((double)_share.AnalyzedSize / 1024 / 1024 / 1024);
            Message = $"バックアップには少なくとも {gigaBytes:0.000} GB の空き容量が必要です。";

            _share.IsAnalyzed = true;
            ProgressVisibility = Visibility.Hidden;
            MessageVisibility = Visibility.Visible;
        }
    }
}
