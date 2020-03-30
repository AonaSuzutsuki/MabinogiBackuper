using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonStyleLib.Models;
using MabinogiBackuperLib.ExRegistry;

namespace MabinogiBackuper.Models
{
    public class MabinogiLauncherMoverModel : ModelBase
    {
        #region Fields

        private string _launcherPath;
        private string _rootPath;
        private string _iconPath;

        #endregion

        #region Properties

        public string LauncherPath
        {
            get => _launcherPath;
            set => SetProperty(ref _launcherPath, value);
        }

        public string RootPath
        {
            get => _rootPath;
            set => SetProperty(ref _rootPath, value);
        }

        public string IconPath
        {
            get => _iconPath;
            set => SetProperty(ref _iconPath, value);
        }

        #endregion

        public void LoadFromRegistry()
        {
            var editor = new RegistryEditor();
            LauncherPath = editor.GetValue(@"SOFTWARE\Nexon\Mabinogi", "Executable");
            RootPath = editor.GetValue(@"SOFTWARE\Nexon\Mabinogi", "RootPath");
            IconPath = editor.GetValue(@"SOFTWARE\Nexon\Mabinogi", "Icon");
        }

        public void AssignPath(string path)
        {
            LauncherPath = path;
            RootPath = Path.GetDirectoryName(path);
            IconPath = $"{path},0";
        }

        public string Save()
        {
            try
            {
                var editor = new RegistryEditor();
                editor.SetValue(@"SOFTWARE\Nexon\Mabinogi", "Executable", LauncherPath);
                editor.SetValue(@"SOFTWARE\Nexon\Mabinogi", "RootPath", RootPath);
                editor.SetValue(@"SOFTWARE\Nexon\Mabinogi", "Icon", IconPath);
            }
            catch (Exception e)
            {
                return e.Message;
            }

            return string.Empty;
        }
    }
}
