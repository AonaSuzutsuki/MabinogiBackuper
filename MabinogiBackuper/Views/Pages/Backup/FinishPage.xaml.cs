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
    /// Finish.xaml の相互作用ロジック
    /// </summary>
    public partial class FinishPage : Page
    {
        public FinishPage(NavigationWindowService<BackupShare> service)
        {
            InitializeComponent();

            var model = new FinishPageModel(service.Share);
            DataContext = new FinishPageViewModel(service, model);
        }
    }
}
