using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommonStyleLib.File;
using MabinogiBackuper.Models.Backup;
using Prism.Commands;
using Prism.Mvvm;
using Reactive.Bindings;

namespace MabinogiBackuper.ViewModels.Backup
{
    public class DestSavePageViewModel : NavigationPageViewModelBase
    {
        public DestSavePageViewModel(NavigationWindowService<BackupShare> service) : base(service?.NavigationValue)
        {
            _service = service;
            _service.NavigationValue.CanGoNext = false;

            GetSavedPathCommand = new DelegateCommand(GetSavedPath);
            TextChangedCommand = new DelegateCommand(TextChanged);
        }

        #region Fields

        private readonly NavigationWindowService<BackupShare> _service;

        private string _savedPath;

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

        #endregion

        #region Event Properties

        public ICommand GetSavedPathCommand { get; set; }

        public ICommand TextChangedCommand { get; set; }

        #endregion

        #region Event Methods

        public void GetSavedPath()
        {
            var path = FileSelector.GetFilePath("C:\\", "All Files (*.dat)|*.dat", "MabinogiBackup.dat",
                FileSelector.FileSelectorType.Write);
            if (!string.IsNullOrEmpty(path))
            {
                SavedPath = path;
            }
        }

        public void TextChanged()
        {
            _service.NavigationValue.CanGoNext = !string.IsNullOrEmpty(SavedPath);
        }

        #endregion
    }
}
