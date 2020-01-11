﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MabinogiBackuper.Models.Backup;
using MabinogiBackuper.ViewModels;

namespace MabinogiBackuper.Views.Pages.Backup
{
    /// <summary>
    /// FirstPage.xaml の相互作用ロジック
    /// </summary>
    public partial class FirstPage : Page
    {
        public FirstPage(NavigationWindowService<BackupShare> service)
        {
            InitializeComponent();

            DataContext = new NavigationPageViewModel(service.NavigationValue);
            Loaded += (sender, args) => service.NavigationValue.CloseBtVisibility = Visibility.Collapsed;
        }
    }
}
