using System;
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
using MabinogiBackuper.ViewModels;
using MabinogiBackuper.ViewModels.Restore;

namespace MabinogiBackuper.Views.Pages.Restore
{
    /// <summary>
    /// DestRestorePage.xaml の相互作用ロジック
    /// </summary>
    public partial class DestRestorePage : Page
    {
        public DestRestorePage(NavigationWindowService<RestoreShare> service)
        {
            InitializeComponent();

            DataContext = new DestRestorePageViewModel(service);
        }
    }
}
