﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;

namespace MabinogiBackuper.ViewModels
{
    public class NavigationPageViewModel : NavigationPageViewModelBase
    {
        public NavigationPageViewModel(NavigationBindableValue bindableValue) : base(bindableValue)
        {
        }
    }

    public abstract class NavigationPageViewModelBase : BindableBase, INavigationRefresh
    {
        protected NavigationPageViewModelBase(NavigationBindableValue bindableValue)
        {
            BindableValue = bindableValue;
        }

        protected NavigationBindableValue BindableValue { get; }


        public virtual void RefreshValues()
        {
            BindableValue.InitDefaultValue();
        }
    }
}
