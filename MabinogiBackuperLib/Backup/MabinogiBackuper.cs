using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MabinogiBackuperLib.Backup
{
    public class Backuper
    {
        public RegistryCollector RegistryCollector { get; set; } = new RegistryCollector();

        public void CreateBackupData()
        {
            var registry = RegistryCollector.GetJson();
        }

        public void GetImageArchive()
        {
        }
    }
}
