using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommonCoreLib.Bool;
using CommonStyleLib.File;
using MabinogiBackuper.Models.Backup;
using MabinogiBackuper.Models.Restore;
using MabinogiBackuperLib.Backup;
using Prism.Commands;
using Prism.Mvvm;
using Reactive.Bindings;

namespace MabinogiBackuper.ViewModels.Restore
{
    public class DestRestorePageViewModel : NavigationPageViewModelBase
    {
        public DestRestorePageViewModel(NavigationWindowService<RestoreShare> service) : base(service?.NavigationValue)
        {
            _service = service;
            _service.NavigationValue.CanGoNext = false;

            GetSavedPathCommand = new DelegateCommand(GetSavedPath);
            GetDestRestorePathCommand = new DelegateCommand(GetDestRestorePath);
            TextChangedCommand = new DelegateCommand(TextChanged);
        }

        #region Fields

        private readonly NavigationWindowService<RestoreShare> _service;

        private string _savedPath;
        private string _destRestorePath;
        private bool _isManuallyPath;

        #endregion

        #region Properties

        public string SavedPath
        {
            get => _savedPath;
            set
            {
                SetProperty(ref _savedPath, value);
                _service.Share.SavedPath = value;
            }
        }

        public string DestRestorePath
        {
            get => _destRestorePath;
            set
            {
                SetProperty(ref _destRestorePath, value);
                _service.Share.DestRestorePath = value;
            }
        }

        public bool IsManuallyPath
        {
            get => _isManuallyPath;
            set
            {
                SetProperty(ref _isManuallyPath, value);
                _service.Share.DestRestorePath = !value ? MabinogiRestorer.MabinogiLocation : DestRestorePath;
                TextChanged();
            }
        }

        #endregion

        #region Event Properties

        public ICommand GetSavedPathCommand { get; set; }
        public ICommand GetDestRestorePathCommand { get; set; }

        public ICommand TextChangedCommand { get; set; }

        #endregion

        #region Event Methods

        public void GetSavedPath()
        {
            var path = FileSelector.GetFilePath(CommonCoreLib.AppInfo.GetAppPath(), "All Files (*.dat)|*.dat", "MabinogiBackup.dat",
                FileSelector.FileSelectorType.Read);
            if (!string.IsNullOrEmpty(path))
                SavedPath = path;
        }

        public void GetDestRestorePath()
        {
            var path = DirectorySelector.GetDirPath(CommonCoreLib.AppInfo.GetAppPath());
            if (!string.IsNullOrEmpty(path))
                DestRestorePath = path;
        }

        public void TextChanged()
        {
            var collector = new BoolCollector();
            collector.ChangeBool(nameof(SavedPath), !string.IsNullOrEmpty(SavedPath));

            if (IsManuallyPath)
                collector.ChangeBool(nameof(DestRestorePath), !string.IsNullOrEmpty(DestRestorePath));

            _service.NavigationValue.CanGoNext = collector.Value;
        }

        #endregion

        public override void RefreshValues()
        {
            base.RefreshValues();

            TextChanged();
        }
    }
}
