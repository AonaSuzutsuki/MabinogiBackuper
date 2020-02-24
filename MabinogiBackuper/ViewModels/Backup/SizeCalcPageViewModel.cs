using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MabinogiBackuper.Models.Backup;
using Prism.Commands;
using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace MabinogiBackuper.ViewModels.Backup
{
    public class SizeCalcPageViewModel : NavigationPageViewModelBase
    {
        public SizeCalcPageViewModel(NavigationWindowService<BackupShare> bindableValue, SizeCalcPageModel model)
            : base(bindableValue?.NavigationValue)
        {
            _model = model;
            _service = bindableValue;

            _service.NavigationValue.BackBtVisibility = Visibility.Collapsed;
            _service.NavigationValue.CancelBtVisibility = Visibility.Collapsed;
            _service.NavigationValue.NextBtVisibility = Visibility.Collapsed;

            ProgressVisibility = model.ObserveProperty(m => m.ProgressVisibility).ToReactiveProperty();
            MessageVisibility = model.ObserveProperty(m => m.MessageVisibility).ToReactiveProperty();
            ProgressValue = model.ObserveProperty(m => m.ProgressValue).ToReactiveProperty();
            ProgressLabel = model.ObserveProperty(m => m.ProgressLabel).ToReactiveProperty();
            Message = model.ObserveProperty(m => m.Message).ToReactiveProperty();

            LoadedCommand = new DelegateCommand(Loaded);
        }

        #region Fields

        private readonly NavigationWindowService<BackupShare> _service;
        private readonly SizeCalcPageModel _model;

        #endregion

        #region Properties

        public ReactiveProperty<Visibility> ProgressVisibility { get; set; }
        public ReactiveProperty<Visibility> MessageVisibility { get; set; }
        public ReactiveProperty<int> ProgressValue { get; set; }
        public ReactiveProperty<string> ProgressLabel { get; set; }
        public ReactiveProperty<string> Message { get; set; }

        #endregion

        #region Event Properties

        public ICommand LoadedCommand { get; set; }

        #endregion

        #region Event Methods

        public void Loaded()
        {
            if (_service.Share.IsChanged)
                _ = Analyze();
            _service.Share.IsChanged = false;
        }

        public async Task Analyze()
        {
            await _model.Analyze();

            _service.NavigationValue.BackBtVisibility = Visibility.Visible;
            _service.NavigationValue.CancelBtVisibility = Visibility.Visible;
            _service.NavigationValue.NextBtVisibility = Visibility.Visible;
        }

        #endregion
    }
}
