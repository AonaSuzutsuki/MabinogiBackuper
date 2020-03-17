using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MabinogiBackuperLib.Backup
{
    public abstract class AbstractMabinogi
    {
        public const string RegistryFileName = "Registry.json";

        protected long GetDriveFreeSize(string filePath)
        {
            var letter = Path.GetPathRoot(filePath);
            if (string.IsNullOrEmpty(letter))
                return -1;

            var drive = new DriveInfo(letter);
            return drive.AvailableFreeSpace;
        }
    }
}
