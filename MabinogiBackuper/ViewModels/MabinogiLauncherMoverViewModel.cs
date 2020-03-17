using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonStyleLib.Models;
using CommonStyleLib.ViewModels;
using CommonStyleLib.Views;

namespace MabinogiBackuper.ViewModels
{
    public class MabinogiLauncherMoverViewModel : ViewModelBase
    {
        public MabinogiLauncherMoverViewModel(IWindowService windowService, ModelBase modelBase) : base(windowService, modelBase)
        {
        }
    }
}
