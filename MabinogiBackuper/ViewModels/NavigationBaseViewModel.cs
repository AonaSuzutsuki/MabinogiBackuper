using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using CommonStyleLib.ViewModels;
using CommonStyleLib.Views;
using MabinogiBackuper.Models;
using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace MabinogiBackuper.ViewModels
{
    public class NavigationWindowService : WindowService
    {
        public class NavigationBindableValue : BindableBase
        {
            private bool _canGoNext;
            private bool _canGoBack;

            private Visibility _closeBtVisibility;

            public bool CanGoNext
            {
                get => _canGoNext;
                set => SetProperty(ref _canGoNext, value);
            }

            public bool CanGoBack
            {
                get => _canGoBack;
                set => SetProperty(ref _canGoBack, value);
            }

            public Visibility CloseBtVisibility
            {
                get => _closeBtVisibility;
                set => SetProperty(ref _closeBtVisibility, value);
            }
        }

        public IList<Page> Pages { get; set; }
        public NavigationService Navigation { get; set; }

        public NavigationBindableValue NavigationValue { get; } = new NavigationBindableValue();

        public void GoNext()
        {

        }

        public void GoBack()
        {

        }
    }

    public class NavigationBaseViewModel : ViewModelBase
    {
        public NavigationBaseViewModel(NavigationWindowService service, NavigationBaseModel model) : base(service, model)
        {
            _navigationService = service;

            CloseBtVisibility = service.NavigationValue.ObserveProperty(m => m.CloseBtVisibility).ToReactiveProperty();
        }

        #region Fields

        private readonly NavigationWindowService _navigationService;

        #endregion

        #region Properties

        public ReactiveProperty<Visibility> CloseBtVisibility { get; set; }

        #endregion


        #region Event Methods

        protected override void MainWindow_Loaded()
        {
            _navigationService.Navigation.Navigate(_navigationService.Pages.First());
        }

        #endregion
    }
}
