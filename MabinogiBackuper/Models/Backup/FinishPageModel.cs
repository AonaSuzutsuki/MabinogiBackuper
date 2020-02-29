using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MabinogiBackuperLib.ExRegistry;
using MabinogiBackuperLib.FileFunctions;
using Prism.Mvvm;

namespace MabinogiBackuper.Models.Backup
{
    public class FinishPageModel : BindableBase
    {

        private readonly BackupShare _share;
        private string _backupSize;
        private string _rootPath;

        public string BackupSize
        {
            get => _backupSize;
            set => SetProperty(ref _backupSize, value);
        }

        public string RootPath
        {
            get => _rootPath;
            set => SetProperty(ref _rootPath, value);
        }



        public FinishPageModel(BackupShare share)
        {
            _share = share;
        }

        public void Load()
        {
            var editor = new RegistryEditor();
            RootPath = editor.GetValue(@"SOFTWARE\Nexon\Mabinogi", "RootPath");

            var fi = new FileInfo(_share.SavedPath);
            BackupSize = FileSize.ConvertToString((ulong)fi.Length).converted;
        }
    }
}
