using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MabinogiBackuper.Models.Backup;
using MabinogiBackuperLib.Archive;
using MabinogiBackuperLib.Backup;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace MabinogiBackuper.ViewModels.Backup
{
    public class BackupProgressPageViewModel : NavigationPageViewModelBase
    {
        public BackupProgressPageViewModel(NavigationWindowService<BackupShare> bindableValue, BackupProgressPageModel model) : base(bindableValue?.NavigationValue)
        {
            _model = model;

            ProgressLabel = model.ObserveProperty(m => m.ProgressLabel).ToReactiveProperty();
            ProgressValue = model.ObserveProperty(m => m.ProgressValue).ToReactiveProperty();

            _ = Loaded();
        }

        #region Fields

        private readonly BackupProgressPageModel _model;

        #endregion

        #region Properties

        public ReactiveProperty<int> ProgressValue { get; set; }
        public ReactiveProperty<string> ProgressLabel { get; set; }

        #endregion

        #region Event Properties



        #endregion

        #region Event Mehods

        private async Task Loaded()
        {
            await _model.Analyze();

            BindableValue.NextBtVisibility = Visibility.Visible;
        }

        #endregion

        public override void RefreshValues()
        {
            base.RefreshValues();

            BindableValue.BackBtVisibility = Visibility.Collapsed;
            BindableValue.NextBtVisibility = Visibility.Collapsed;
            BindableValue.CancelBtVisibility = Visibility.Collapsed;
            BindableValue.CloseBtVisibility = Visibility.Collapsed;
        }
    }
}
