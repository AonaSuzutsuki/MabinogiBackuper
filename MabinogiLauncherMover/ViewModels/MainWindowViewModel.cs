using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommonStyleLib.ExMessageBox;
using CommonStyleLib.File;
using CommonStyleLib.ViewModels;
using CommonStyleLib.Views;
using MabinogiLauncherMover.Models;
using Prism.Commands;
using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace MabinogiLauncherMover.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        public MainWindowViewModel(WindowService service, MainWindowModel model) : base(service, model)
        {
            this.model = model;

            LauncherPath = model.ToReactivePropertyAsSynchronized(m => m.LauncherPath);
            RootPath = model.ToReactivePropertyAsSynchronized(m => m.RootPath);
            IconPath = model.ToReactivePropertyAsSynchronized(m => m.IconPath);

            LauncherPathCommand = new DelegateCommand(GetLauncherPath);
            SaveBtCommand = new DelegateCommand(Save);
        }

        #region Fields

        private MainWindowModel model;

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

        #region Event Methods

        protected override void MainWindow_Loaded()
        {
            model.LoadFromRegistry();
        }

        public void GetLauncherPath()
        {
            var path = FileSelector.GetFilePath("C:\\", "Executable File (*.exe)|*.exe",
                "Mabinogi.exe", FileSelector.FileSelectorType.Read);
            model.AssignPatth(path);
        }

        public void Save()
        {
            var message = model.Save();
            if (string.IsNullOrEmpty(message))
                WindowManageService.MessageBoxShow("Succeeded saving.", "Succeeded.", ExMessageBoxBase.MessageType.Asterisk);
            else
                WindowManageService.MessageBoxShow(message, "Error", ExMessageBoxBase.MessageType.Asterisk);
        }
        #endregion
    }
}
