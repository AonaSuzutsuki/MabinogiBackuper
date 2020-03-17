using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MabinogiBackuperLib.Backup;

namespace MabinogiBackuper.ViewModels.Restore
{
    public class RestoreShare : IDisposable
    {
        private string _savedPath;
        private string _destRestorePath = MabinogiRestorer.MabinogiLocation;

        public string SavedPath
        {
            get => _savedPath;
            set
            {
                _savedPath = value;
                IsChanged = true;
            }
        }

        public string DestRestorePath
        {
            get => _destRestorePath;
            set
            {
                _destRestorePath = value;
                IsChanged = true;
            }
        }

        public bool IsChanged { get; set; }

        public MabinogiRestorer Restorer { get; set; }

        public void Dispose()
        {
            Restorer?.Dispose();
        }
    }
}
