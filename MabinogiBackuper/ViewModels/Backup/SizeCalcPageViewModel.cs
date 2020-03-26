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
            _share = bindableValue?.Share;
            _model = model;

            ProgressVisibility = model.ObserveProperty(m => m.ProgressVisibility).ToReactiveProperty();
            MessageVisibility = model.ObserveProperty(m => m.MessageVisibility).ToReactiveProperty();
            ProgressValue = model.ObserveProperty(m => m.ProgressValue).ToReactiveProperty();
            ProgressLabel = model.ObserveProperty(m => m.ProgressLabel).ToReactiveProperty();
            Message = model.ObserveProperty(m => m.Message).ToReactiveProperty();
        }

        #region Fields

        private readonly BackupShare _share;
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

        #endregion

        #region Event Methods

        public override void Loaded()
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
            BindableValue.InitDefaultValue();

            BindableValue.NextBtContent = "開始する";
            if (_share.IsChanged)
            {
                BindableValue.BackBtVisibility = Visibility.Collapsed;
                BindableValue.CancelBtVisibility = Visibility.Collapsed;
                BindableValue.NextBtVisibility = Visibility.Collapsed;
            }
        }

        #endregion
    }
}
