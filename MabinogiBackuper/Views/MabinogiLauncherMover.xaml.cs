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
using System.Windows.Shapes;
using CommonStyleLib.Models;
using CommonStyleLib.Views;
using MabinogiBackuper.Models;
using MabinogiBackuper.ViewModels;

namespace MabinogiBackuper.Views
{
    /// <summary>
    /// MabinogiLauncherMover.xaml の相互作用ロジック
    /// </summary>
    public partial class MabinogiLauncherMover : Window
    {
        public MabinogiLauncherMover()
        {
            InitializeComponent();

            DataContext = new MabinogiLauncherMoverViewModel(new WindowService(this), new MabinogiLauncherMoverModel());
        }
    }
}
