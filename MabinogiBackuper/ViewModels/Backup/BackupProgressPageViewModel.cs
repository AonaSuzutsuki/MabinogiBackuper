using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MabinogiBackuper.Models.Backup;
using MabinogiBackuperLib.Backup;

namespace MabinogiBackuper.ViewModels.Backup
{
    public class BackupProgressPageViewModel : NavigationPageViewModelBase
    {
        public BackupProgressPageViewModel(NavigationWindowService<BackupShare> bindableValue) : base(bindableValue?.NavigationValue)
        {
            var backuper = new Backuper();
            backuper.CreateBackupData();
        }

        #region Fields



        #endregion

        #region Properties



        #endregion

        #region Event Properties



        #endregion

        #region Event Mehods

        

        #endregion

        public override void RefreshValues()
        {
            BindableValue.BackBtVisibility = Visibility.Collapsed;
            BindableValue.NextBtVisibility = Visibility.Collapsed;
            BindableValue.CancelBtVisibility = Visibility.Collapsed;
            BindableValue.CloseBtVisibility = Visibility.Collapsed;
        }
    }
}
