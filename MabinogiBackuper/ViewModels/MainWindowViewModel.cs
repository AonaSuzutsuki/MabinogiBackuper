using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using CommonStyleLib.Models;
using CommonStyleLib.ViewModels;
using CommonStyleLib.Views;
using MabinogiBackuper.Models;
using MabinogiBackuper.Models.Backup;
using MabinogiBackuper.Models.Restore;
using MabinogiBackuper.ViewModels.Restore;
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
            RestoreCommand = new DelegateCommand(OpenRestore);
            LauncherMoverCommand = new DelegateCommand(OpenLauncherMover);
        }

        #region Fields

        private MainWindowModel model;

        #endregion

        #region Properties



        #endregion

        #region Event Properties

        public ICommand BackupBtCommand { get; set; }
        public ICommand RestoreCommand { get; set; }
        public ICommand LauncherMoverCommand { get; set; }

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
                    Navigation = window.MainFrame,
                    Pages = new List<Tuple<Type, bool>>
                    {
                        new Tuple<Type, bool>(typeof(FirstPage), true),
                        new Tuple<Type, bool>(typeof(DestSavePage), true),
                        new Tuple<Type, bool>(typeof(BackupSelectionPage), true),
                        new Tuple<Type, bool>(typeof(SizeCalcPage), false),
                        new Tuple<Type, bool>(typeof(BackupProgressPage), false),
                        new Tuple<Type, bool>(typeof(FinishPage), true),
                    }
                };
                service.NavigationValue.WindowTitle = "Mabinogi Backupper - バックアップ";
                service.Initialize();
                var vm = new NavigationBaseViewModel<BackupShare>(service, navigationModel);
                return vm;
            });
        }

        public void OpenRestore()
        {
            var navigationModel = new NavigationBaseModel();
            WindowManageService.ShowDialog<NavigationBase>(window =>
            {
                var service = new NavigationWindowService<RestoreShare>
                {
                    Owner = window,
                    Navigation = window.MainFrame,
                    Pages = new List<Tuple<Type, bool>>
                    {
                        new Tuple<Type, bool>(typeof(Views.Pages.Restore.FirstPage), true),
                        new Tuple<Type, bool>(typeof(Views.Pages.Restore.DestRestorePage), true),
                        new Tuple<Type, bool>(typeof(Views.Pages.Restore.RestoreSizeCalcPage), false),
                        new Tuple<Type, bool>(typeof(Views.Pages.Restore.RestoreProgressPage), false),
                        new Tuple<Type, bool>(typeof(Views.Pages.Restore.RestoreFinishPage), true)
                    }
                };
                service.NavigationValue.WindowTitle = "Mabinogi Backupper - リストア";
                service.Initialize();
                var vm = new NavigationBaseViewModel<RestoreShare>(service, navigationModel);
                return vm;
            });
        }

        public void OpenLauncherMover()
        {
            var modelBase = new ModelBase();
            var vm = new MabinogiLauncherMoverViewModel(new WindowService(), new MabinogiLauncherMoverModel());
            WindowManageService.ShowDialog<MabinogiLauncherMover>(vm);
        }

        #endregion
    }
}
