using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MabinogiBackuper.Models.Restore;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace MabinogiBackuper.ViewModels.Restore
{
    public class RestoreProgressPageViewModel : NavigationPageViewModelBase
    {
        public RestoreProgressPageViewModel(NavigationWindowService<RestoreShare> service, RestoreProgressPageModel model) : base(service?.NavigationValue)
        {
            _model = model;

            ProgressVisibility = model.ObserveProperty(m => m.ProgressVisibility).ToReactiveProperty();
            MessageVisibility = model.ObserveProperty(m => m.MessageVisibility).ToReactiveProperty();
            ProgressValue = model.ObserveProperty(m => m.ProgressValue).ToReactiveProperty();
            ProgressLabel = model.ObserveProperty(m => m.ProgressLabel).ToReactiveProperty();
            Message = model.ObserveProperty(m => m.Message).ToReactiveProperty();

            _ = Loaded();
        }

        #region Fields

        private readonly RestoreProgressPageModel _model;

        #endregion

        #region Properties

        public ReactiveProperty<Visibility> ProgressVisibility { get; set; }
        public ReactiveProperty<Visibility> MessageVisibility { get; set; }
        public ReactiveProperty<int> ProgressValue { get; set; }
        public ReactiveProperty<string> ProgressLabel { get; set; }
        public ReactiveProperty<string> Message { get; set; }

        #endregion

        #region Event Properties


        #endregion

        #region Methods

        public new async Task Loaded()
        {
            await _model.Restore();

            BindableValue.NextBtVisibility = Visibility.Visible;
        }

        public override void RefreshValues()
        {
            base.RefreshValues();

            BindableValue.BackBtVisibility = Visibility.Collapsed;
            BindableValue.CancelBtVisibility = Visibility.Collapsed;
            BindableValue.CloseBtVisibility = Visibility.Collapsed;
            BindableValue.NextBtVisibility = Visibility.Collapsed;
        }

        #endregion
    }
}
