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
using MabinogiBackuper.Models.Restore;
using MabinogiBackuper.ViewModels;
using MabinogiBackuper.ViewModels.Restore;

namespace MabinogiBackuper.Views.Pages.Restore
{
    /// <summary>
    /// RestoreSelectionPage.xaml の相互作用ロジック
    /// </summary>
    public partial class RestoreSizeCalcPage : UserControl
    {
        public RestoreSizeCalcPage(NavigationWindowService<RestoreShare> service)
        {
            InitializeComponent();

            var model = new RestoreSizeCalcPageModel(service.Share);
            DataContext = new RestoreSizeCalcPageViewModel(service, model);
        }
    }
}
