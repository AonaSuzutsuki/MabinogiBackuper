using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MabinogiBackuper.ViewModels.Restore;
using MabinogiBackuperLib.Backup;
using MabinogiBackuperLib.FileFunctions;
using Prism.Mvvm;

namespace MabinogiBackuper.Models.Restore
{
    public class RestoreSizeCalcPageModel : BindableBase
    {
        #region Fields

        private readonly RestoreShare _share;

        private Visibility _progressVisibility;
        private Visibility _messageVisibility;
        private int _progressValue;
        private string _progressLabel;
        private string _message;

        #endregion

        #region Properties

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

        #endregion


        public RestoreSizeCalcPageModel(RestoreShare share)
        {
            _share = share;
        }

        public async Task Analyze()
        {
            if (_share.Restorer != null)
            {
                _share.Restorer.Dispose();
                _share.Restorer = null;
            }

            var restorer = new MabinogiRestorer(_share.SavedPath);
            restorer.BackupFileAnalyzeProgress.Subscribe(BackupFileAnalyzeProgressChanged, BackupFileAnalyzeCompleted);
            var size = await Task.Factory.StartNew(() => restorer.Analyze());

            var (sizeType, converted) = FileSize.ConvertToString(size);
            var freeSpace = FileSize.ConvertToString(restorer.GetDriveFreeSize()).converted;

            Message = $"復元には {converted} 以上の空き容量が必要です。\n" +
                      $"ドライブの空き容量は {freeSpace} あります。";

            _share.Restorer = restorer;
        }

        public void BackupFileAnalyzeProgressChanged(IProgressEventArgs eventArgs)
        {
            ProgressValue = eventArgs.Percentage;
        }

        public void BackupFileAnalyzeCompleted()
        {
            ProgressVisibility = Visibility.Collapsed;
            MessageVisibility = Visibility.Visible;
        }
    }
}
