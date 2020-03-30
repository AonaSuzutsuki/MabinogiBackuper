using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommonStyleLib.File;
using CommonStyleLib.Models;
using CommonStyleLib.ViewModels;
using CommonStyleLib.Views;
using MabinogiBackuper.Models;
using Prism.Commands;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace MabinogiBackuper.ViewModels
{
    public class MabinogiLauncherMoverViewModel : ViewModelBase
    {
        public MabinogiLauncherMoverViewModel(IWindowService windowService, MabinogiLauncherMoverModel model) : base(windowService, model)
        {
            _model = model;

            LauncherPath = model.ToReactivePropertyAsSynchronized(m => m.LauncherPath);
            RootPath = model.ObserveProperty(m => m.RootPath).ToReactiveProperty();
            IconPath = model.ObserveProperty(m => m.IconPath).ToReactiveProperty();

            LauncherPathCommand = new DelegateCommand(SetLauncherPath);
            SaveBtCommand = new DelegateCommand(Save);

            model.LoadFromRegistry();
        }

        #region Fields

        private readonly MabinogiLauncherMoverModel _model;

        #endregion

        #region Properties

        public ReactiveProperty<string> LauncherPath { get; set; }
        public ReactiveProperty<string> RootPath { get; set; }
        public ReactiveProperty<string> IconPath { get; set; }

        #endregion

        #region Event Properties

        public ICommand LauncherPathCommand { get; set; }

        public ICommand SaveBtCommand { get; set; }

        #endregion

        #region Methods

        public void SetLauncherPath()
        {
            var path = _model.LauncherPath;
            var dirname = "C:\\";
            if (!string.IsNullOrEmpty(path))
                dirname = Path.GetDirectoryName(path);

            var filename = FileSelector.GetFilePath(dirname, "Executable File (*.exe)|*.exe", "Mabinogi.exe",
                FileSelector.FileSelectorType.Read);
            if (!string.IsNullOrEmpty(filename))
                _model.AssignPath(filename);
        }

        public void Save()
        {
            _model.Save();
        }

        #endregion
    }
}
