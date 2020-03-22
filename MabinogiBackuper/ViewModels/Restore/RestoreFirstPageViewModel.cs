using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CommonStyleLib.Models;
using CommonStyleLib.Views;
using MabinogiBackuper.Models.Restore;
using MabinogiBackuper.Views;
using Prism.Commands;
using Prism.Mvvm;

namespace MabinogiBackuper.ViewModels.Restore
{
    public class RestoreFirstPageViewModel : NavigationPageViewModelBase
    {
        public RestoreFirstPageViewModel(NavigationWindowService<RestoreShare> service) : base(service?.NavigationValue)
        {
            _service = service;

            LaunchMabinogiCommand = new DelegateCommand(LaunchMabinogi);
        }

        #region Fields

        private readonly NavigationWindowService<RestoreShare> _service;

        #endregion

        #region Properties



        #endregion

        #region Event Properties

        public ICommand LaunchMabinogiCommand { get; set; }

        #endregion

        #region Methods

        public void LaunchMabinogi()
        {
            var parent = _service.Parent;
            var vm = new MabinogiLauncherMoverViewModel(new WindowService(parent.Owner), new ModelBase());
            _service.Close();
            parent.ShowDialog<MabinogiLauncherMover>(vm);
        }

        public override void RefreshValues()
        {
            base.RefreshValues();

            BindableValue.CloseBtVisibility = Visibility.Collapsed;
        }

        #endregion
    }
}
