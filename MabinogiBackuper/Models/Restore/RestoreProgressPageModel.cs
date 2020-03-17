using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MabinogiBackuper.ViewModels.Restore;
using MabinogiBackuperLib.Backup;
using Prism.Mvvm;

namespace MabinogiBackuper.Models.Restore
{
    public class RestoreProgressPageModel : BindableBase
    {
        #region Fields

        private readonly RestoreShare _share;
        private readonly MabinogiRestorer _mabinogiRestorer;

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

        public RestoreProgressPageModel(RestoreShare share)
        {
            _share = share;
            _mabinogiRestorer = share.Restorer;

            _mabinogiRestorer.RestoreProgress.Subscribe(BackupFileAnalyzeProgressChanged);
        }

        public async Task Restore()
        {
            var dirPath = _share.DestRestorePath;

            await Task.Factory.StartNew(() =>
            {
                _mabinogiRestorer.Restore(dirPath);
            });
        }

        public void BackupFileAnalyzeProgressChanged(IProgressEventArgs eventArgs)
        {
            ProgressValue = eventArgs.Percentage;
        }
    }
}
