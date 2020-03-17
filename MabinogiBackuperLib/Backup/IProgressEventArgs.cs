using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MabinogiBackuperLib.Backup
{
    public interface IProgressEventArgs
    {
        int Total { get; set; }
        int Current { get; set; }
        int Percentage { get; }
        string Name { get; set; }
    }
}
