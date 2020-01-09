using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using CommonStyleLib.ViewModels;
using CommonStyleLib.Views;
using MabinogiBackuper.Models;
using Prism.Commands;
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
            private Visibility _cancelBtVisibility;

            private string _windowTitle;

            public string WindowTitle
            {
                get => _windowTitle;
                set => SetProperty(ref _windowTitle, value);
            }

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

            public Visibility CancelBtVisibility
            {
                get => _cancelBtVisibility;
                set => SetProperty(ref _cancelBtVisibility, value);
            }
        }

        private int _currentPage;

        public IList<Page> Pages { get; set; }
        public NavigationService Navigation { get; set; }

        public NavigationBindableValue NavigationValue { get; } = new NavigationBindableValue();

        public void Initialize()
        {
            NavigationValue.CanGoBack = false;
            NavigationValue.CanGoNext = Pages.Count > 1;
        }

        public void GoNext()
        {
            if (_currentPage + 1 >= Pages.Count)
                return;

            var page = Pages[_currentPage + 1];
            _currentPage++;
            if (!Navigation.CanGoForward)
                Navigation.Navigate(page);
            else
                Navigation.GoForward();

            if (_currentPage >= Pages.Count - 1)
                NavigationValue.CanGoNext = false;
            NavigationValue.CanGoBack = true;
        }

        public void GoBack()
        {
            if (!Navigation.CanGoBack)
                return;

            Navigation.GoBack();
            _currentPage--;

            if (Pages.Count > 1)
                NavigationValue.CanGoNext = true;

            NavigationValue.CanGoBack = Navigation.CanGoBack;
        }
    }

    public class NavigationBaseViewModel : ViewModelBase
    {
        public NavigationBaseViewModel(NavigationWindowService service, NavigationBaseModel model) : base(service, model)
        {
            _navigationService = service;

            WindowTitle = service.NavigationValue.ObserveProperty(m => m.WindowTitle).ToReactiveProperty();
            BackBtIsEnabled = service.NavigationValue.ObserveProperty(m => m.CanGoBack).ToReactiveProperty();
            NextBtIsEnabled = service.NavigationValue.ObserveProperty(m => m.CanGoNext).ToReactiveProperty();
            CloseBtVisibility = service.NavigationValue.ObserveProperty(m => m.CloseBtVisibility).ToReactiveProperty();
            CancelBtVisibility = service.NavigationValue.ObserveProperty(m => m.CancelBtVisibility).ToReactiveProperty();

            BackBtCommand = new DelegateCommand(GoBack);
            NextBtCommand = new DelegateCommand(GoNext);
        }

        #region Fields

        private readonly NavigationWindowService _navigationService;

        #endregion

        #region Properties

        public ReactiveProperty<string> WindowTitle { get; set; }

        public ReactiveProperty<bool> BackBtIsEnabled { get; set; }
        public ReactiveProperty<bool> NextBtIsEnabled { get; set; }
        public ReactiveProperty<Visibility> CloseBtVisibility { get; set; }
        public ReactiveProperty<Visibility> CancelBtVisibility { get; set; }

        #endregion

        #region Event Properties

        public ICommand BackBtCommand { get; set; }
        public ICommand NextBtCommand { get; set; }

        #endregion


        #region Event Methods

        protected override void MainWindow_Loaded()
        {
            _navigationService.Navigation.Navigate(_navigationService.Pages.First());
        }

        public void GoBack()
        {
            _navigationService.GoBack();
        }

        public void GoNext()
        {
            _navigationService.GoNext();
        }

        #endregion
    }
}
