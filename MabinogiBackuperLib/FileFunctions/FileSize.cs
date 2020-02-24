using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MabinogiBackuperLib.FileFunctions
{
    public static class FileSize
    {
        public static ulong TotalFileSize(IEnumerable<string> files)
        {
            ulong len = 0L;
            foreach (var file in files)
            {
                var info = new FileInfo(file);
                len += (ulong)info.Length;
            }

            return len;
        }
    }
}
