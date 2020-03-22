using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MabinogiBackuper.Models.Restore;

namespace MabinogiBackuper.ViewModels.Restore
{
    public class RestoreFinishPageViewModel : NavigationPageViewModelBase
    {
        public RestoreFinishPageViewModel(NavigationWindowService<RestoreShare> service) : base(service?.NavigationValue)
        {
        }


        public override void RefreshValues()
        {
            base.RefreshValues();

            BindableValue.NextBtVisibility = Visibility.Collapsed;
            BindableValue.BackBtVisibility = Visibility.Collapsed;
            BindableValue.CancelBtVisibility = Visibility.Collapsed;
            BindableValue.CloseBtVisibility = Visibility.Visible;
        }
    }
}
