using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonStyleLib.Models;
using MabinogiBackuperLib.ExRegistry;
using Prism.Mvvm;

namespace MabinogiLauncherMover.Models
{
    public class MainWindowModel : ModelBase
    {

        #region Fields

        private string launcherPath;
        private string rootPath;
        private string iconPath;

        #endregion

        #region Properties

        public string LauncherPath
        {
            get => launcherPath;
            set => SetProperty(ref launcherPath, value);
        }

        public string RootPath
        {
            get => rootPath;
            set => SetProperty(ref rootPath, value);
        }

        public string IconPath
        {
            get => iconPath;
            set => SetProperty(ref iconPath, value);
        }

        #endregion

        public void LoadFromRegistry()
        {
            var editor = new RegistryEditor();
            LauncherPath = editor.GetValue(@"SOFTWARE\Nexon\Mabinogi", "Executable");
            RootPath = editor.GetValue(@"SOFTWARE\Nexon\Mabinogi", "RootPath");
            IconPath = editor.GetValue(@"SOFTWARE\Nexon\Mabinogi", "Icon");
        }

        public void AssignPatth(string path)
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
