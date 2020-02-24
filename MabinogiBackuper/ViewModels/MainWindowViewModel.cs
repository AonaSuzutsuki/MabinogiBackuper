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
using MabinogiBackuper.Models.Backup;
using MabinogiBackuper.Views;
using MabinogiBackuper.Views.Pages;
using MabinogiBackuper.Views.Pages.Backup;
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
            WindowManageService.ShowDialog<NavigationBase>(window =>
            {
                var service = new NavigationWindowService<BackupShare>
                {
                    Owner = window,
                    Navigation = window.MainFrame.NavigationService,
                    Pages = new List<Type>
                    {
                        typeof(FirstPage),
                        typeof(DestSavePage),
                        typeof(BackupSelectionPage),
                        typeof(BackupProgressPage),
                        typeof(FinishPage)
                    }
                };
                service.NavigationValue.WindowTitle = "Mabinogi Backupper - バックアップ";;
                service.Initialize();
                var vm = new NavigationBaseViewModel<BackupShare>(service, navigationModel);
                window.Loaded += (sender, args) => vm.Loaded.Execute(null);
                return vm;
            });
        }

        #endregion
    }
}
