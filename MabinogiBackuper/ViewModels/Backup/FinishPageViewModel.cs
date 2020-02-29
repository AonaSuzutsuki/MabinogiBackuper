using MabinogiBackuper.Models.Backup;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Prism.Commands;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace MabinogiBackuper.ViewModels.Backup
{
    public class FinishPageViewModel : NavigationPageViewModelBase
    {
        public FinishPageViewModel(NavigationWindowService<BackupShare> bindableValue, FinishPageModel model) : base(bindableValue?.NavigationValue)
        {
            _model = model;

            BackupSize = model.ObserveProperty(m => m.BackupSize).ToReactiveProperty();
            MabinogiLocation = model.ObserveProperty(m => m.RootPath).ToReactiveProperty();

            LinkCommand = new DelegateCommand(OpenDirectory);

            model.Load();
        }

        #region Fields

        private readonly FinishPageModel _model;

        #endregion

        #region Properties

        public ReactiveProperty<string> BackupSize { get; set; }
        public ReactiveProperty<string> MabinogiLocation { get; set; }

        #endregion

        #region Event Properties

        public ICommand LinkCommand { get; set; }

        #endregion


        #region Event Methods

        public void OpenDirectory()
        {
            if (!string.IsNullOrEmpty(_model.RootPath) && Directory.Exists(_model.RootPath))
                Process.Start("EXPLORER.EXE", $"/e,/root,\"{_model.RootPath}\"");
        }

        #endregion

        public override void RefreshValues()
        {
            base.RefreshValues();

            BindableValue.NextBtVisibility = Visibility.Collapsed;
            BindableValue.BackBtVisibility = Visibility.Collapsed;
            BindableValue.CancelBtVisibility = Visibility.Collapsed;
            BindableValue.CloseBtVisibility = Visibility.Visible;
        }
    }
}
