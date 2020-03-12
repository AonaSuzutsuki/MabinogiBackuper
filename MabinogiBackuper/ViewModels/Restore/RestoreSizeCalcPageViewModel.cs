using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MabinogiBackuper.Models.Restore;
using Prism.Commands;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace MabinogiBackuper.ViewModels.Restore
{
    public class RestoreSizeCalcPageViewModel : NavigationPageViewModelBase
    {
        public RestoreSizeCalcPageViewModel(NavigationWindowService<RestoreShare> service, RestoreSizeCalcPageModel model) : base(service?.NavigationValue)
        {
            _model = model;
            _share = service?.Share;

            ProgressVisibility = model.ObserveProperty(m => m.ProgressVisibility).ToReactiveProperty();
            MessageVisibility = model.ObserveProperty(m => m.MessageVisibility).ToReactiveProperty();
            ProgressValue = model.ObserveProperty(m => m.ProgressValue).ToReactiveProperty();
            ProgressLabel = model.ObserveProperty(m => m.ProgressLabel).ToReactiveProperty();
            Message = model.ObserveProperty(m => m.Message).ToReactiveProperty();

            LoadedCommand = new DelegateCommand(Loaded);
        }


        #region Fields

        private readonly RestoreShare _share;
        private readonly RestoreSizeCalcPageModel _model;

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

        #region Methods

        public void Loaded()
        {
            if (_share.IsChanged)
                _ = Analyze();
            _share.IsChanged = false;
        }

        public async Task Analyze()
        {
            await _model.Analyze();

            BindableValue.BackBtVisibility = Visibility.Visible;
            BindableValue.CancelBtVisibility = Visibility.Visible;
            BindableValue.NextBtVisibility = Visibility.Visible;
        }

        public override void RefreshValues()
        {
            base.RefreshValues();

            if (_share.IsChanged)
            {
                BindableValue.BackBtVisibility = Visibility.Collapsed;
                BindableValue.CancelBtVisibility = Visibility.Collapsed;
                BindableValue.CloseBtVisibility = Visibility.Collapsed;
                BindableValue.NextBtVisibility = Visibility.Collapsed;
            }
        }

        #endregion
    }
}
