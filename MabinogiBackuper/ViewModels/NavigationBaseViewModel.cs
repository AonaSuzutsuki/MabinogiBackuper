using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using CommonExtensionLib.Extensions;
using CommonStyleLib.ViewModels;
using CommonStyleLib.Views;
using MabinogiBackuper.Models;
using Prism.Commands;
using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace MabinogiBackuper.ViewModels
{
    public class NavigationBindableValue : BindableBase
    {

        private bool _canGoNext;
        private bool _canGoBack;

        private Visibility _backBtVisibility;
        private Visibility _nextBtVisibility;
        private Visibility _closeBtVisibility;
        private Visibility _cancelBtVisibility;

        private string _nextBtContent;
        private string _windowTitle;

        public string WindowTitle
        {
            get => _windowTitle;
            set => SetProperty(ref _windowTitle, value);
        }

        public string NextBtContent
        {
            get => _nextBtContent;
            set => SetProperty(ref _nextBtContent, value);
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

        public Visibility BackBtVisibility
        {
            get => _backBtVisibility;
            set => SetProperty(ref _backBtVisibility, value);
        }

        public Visibility NextBtVisibility
        {
            get => _nextBtVisibility;
            set => SetProperty(ref _nextBtVisibility, value);
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


        public virtual void InitDefaultValue()
        {
            NextBtContent = "次へ";
            BackBtVisibility = Visibility.Visible;
            NextBtVisibility = Visibility.Visible;
            CancelBtVisibility = Visibility.Visible;
            CloseBtVisibility = Visibility.Collapsed;
        }
    }

    public class NavigationWindowService<T> : WindowService where T : new()
    {
        private int _currentPage = -1;
        private Dictionary<Type, object> cacheDictionary = new Dictionary<Type, object>();

        public IList<Type> Pages { get; set; }
        public NavigationService Navigation { get; set; }

        public NavigationBindableValue NavigationValue { get; set; }

        public T Share { get; set; } = new T();

        public NavigationWindowService()
        {
            NavigationValue = new NavigationBindableValue();
            NavigationValue.InitDefaultValue();
        }

        public void Initialize()
        {
            NavigationValue.CanGoBack = false;
            NavigationValue.CanGoNext = Pages.Count > 1;
        }

        public void GoNext()
        {
            if (_currentPage + 1 >= Pages.Count)
                return;

            var type = Pages[_currentPage + 1];
            var page = cacheDictionary.GetCallback(type, () => Activator.CreateInstance(Pages[_currentPage + 1], this));
            if (!cacheDictionary.ContainsKey(type))
                cacheDictionary.Add(type, page);

            RefreshValues(page);

            _currentPage++;
            if (!Navigation.CanGoForward)
                Navigation.Navigate(page);
            else
                Navigation.GoForward();

            if (_currentPage >= Pages.Count - 1)
                NavigationValue.CanGoNext = false;
            NavigationValue.CanGoBack = _currentPage > 0;
        }

        public void GoBack()
        {
            if (!Navigation.CanGoBack)
                return;

            Navigation.GoBack();
            _currentPage--;

            var type = Pages[_currentPage];
            var page = cacheDictionary.GetCallback(type, null);
            RefreshValues(page);

            if (Pages.Count > 1)
                NavigationValue.CanGoNext = true;

            NavigationValue.CanGoBack = Navigation.CanGoBack;

        }

        private void RefreshValues(object page)
        {
            if (page is Page cPage)
            {
                var refresh = cPage.DataContext as INavigationRefresh;
                refresh?.RefreshValues();
            }
        }
    }

    public class NavigationBaseViewModel<T> : ViewModelBase where T : new()
    {
        public NavigationBaseViewModel(NavigationWindowService<T> service, NavigationBaseModel model) : base(service, model)
        {
            _navigationService = service;

            WindowTitle = service.NavigationValue.ObserveProperty(m => m.WindowTitle).ToReactiveProperty();
            NextBtContent = service.NavigationValue.ObserveProperty(m => m.NextBtContent).ToReactiveProperty();
            BackBtIsEnabled = service.NavigationValue.ObserveProperty(m => m.CanGoBack).ToReactiveProperty();
            NextBtIsEnabled = service.NavigationValue.ObserveProperty(m => m.CanGoNext).ToReactiveProperty();
            BackBtVisibility = service.NavigationValue.ObserveProperty(m => m.BackBtVisibility).ToReactiveProperty();
            NextBtVisibility = service.NavigationValue.ObserveProperty(m => m.NextBtVisibility).ToReactiveProperty();
            CloseBtVisibility = service.NavigationValue.ObserveProperty(m => m.CloseBtVisibility).ToReactiveProperty();
            CancelBtVisibility = service.NavigationValue.ObserveProperty(m => m.CancelBtVisibility).ToReactiveProperty();

            BackBtCommand = new DelegateCommand(GoBack);
            NextBtCommand = new DelegateCommand(GoNext);
            CloseBtCommand = new DelegateCommand(Close);
        }

        #region Fields

        private readonly NavigationWindowService<T> _navigationService;

        #endregion

        #region Properties

        public ReactiveProperty<string> WindowTitle { get; set; }

        public ReactiveProperty<string> NextBtContent { get; set; }

        public ReactiveProperty<bool> BackBtIsEnabled { get; set; }
        public ReactiveProperty<bool> NextBtIsEnabled { get; set; }
        public ReactiveProperty<Visibility> BackBtVisibility { get; set; }
        public ReactiveProperty<Visibility> NextBtVisibility { get; set; }
        public ReactiveProperty<Visibility> CloseBtVisibility { get; set; }
        public ReactiveProperty<Visibility> CancelBtVisibility { get; set; }

        #endregion

        #region Event Properties

        public ICommand BackBtCommand { get; set; }
        public ICommand NextBtCommand { get; set; }
        public ICommand CloseBtCommand { get; set; }

        #endregion


        #region Event Methods

        protected override void MainWindow_Loaded()
        {
            _navigationService.GoNext();
        }

        public void GoBack()
        {
            _navigationService.GoBack();
        }

        public void GoNext()
        {
            _navigationService.GoNext();
        }

        public void Close()
        {
            base.MainWindowCloseBt_Click();
        }

        #endregion
    }
}
