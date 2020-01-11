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
using MabinogiBackuper.ViewModels.Backup;

namespace MabinogiBackuper.Views.Pages.Backup
{
    /// <summary>
    /// BackupSelectionPage.xaml の相互作用ロジック
    /// </summary>
    public partial class BackupSelectionPage : Page
    {
        public BackupSelectionPage(NavigationWindowService<BackupShare> service)
        {
            InitializeComponent();

            var vm = new BackupSelectionPageViewModel(service);
            DataContext = vm;
        }
    }
}
