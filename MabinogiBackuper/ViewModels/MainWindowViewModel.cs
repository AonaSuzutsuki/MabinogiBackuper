using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using CommonStyleLib.ViewModels;
using CommonStyleLib.Views;
using MabinogiBackuper.Models;
using MabinogiBackuper.Views;
using MabinogiBackuper.Views.Pages;
using Prism.Commands;

namespace MabinogiBackuper.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel(WindowService service, MainWindowModel model) : base(service, model)
        {
            this.model = model;

            BackupBtCommand = new DelegateCommand(OpenBackup);
        }

        #region Fields

        private MainWindowModel model;

        #endregion

        #region Properties



        #endregion

        #region Event Properties

        public ICommand BackupBtCommand { get; set; }

        #endregion

        #region Event Methods

        public void OpenBackup()
        {
            var navigationModel = new NavigationBaseModel();
            WindowManageService.Show<NavigationBase>(window =>
            {
                var service = new NavigationWindowService
                {
                    Owner = window,
                    Navigation = window.MainFrame.NavigationService,
                };
                service.NavigationValue.WindowTitle = "Mabinogi Backuper - バックアップ";
                service.Pages = new List<Page>
                {
                    new TestPage(service)
                };
                service.Initialize();
                var vm = new NavigationBaseViewModel(service, navigationModel);
                window.Loaded += (sender, args) => vm.Loaded.Execute(null);
                return vm;
            });
        }

        #endregion
    }
}
