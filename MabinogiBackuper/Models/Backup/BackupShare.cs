using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MabinogiBackuper.Models.Backup
{
    public class BackupShare
    {
        public string SavedPath { get; set; }

        public bool ContainsDrawChat { get; set; }
        public bool ContainsScreenshot { get; set; }
        public bool ContainsPetAi { get; set; }
        public bool ContainsKeyAlerm { get; set; }
    }
}
