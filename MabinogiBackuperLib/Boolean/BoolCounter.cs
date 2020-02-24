using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MabinogiBackuperLib.Boolean
{
    public class BoolCounter
    {

        private Dictionary<string, bool> _dictionary { get; } = new Dictionary<string, bool>();

        public void Change(string name, bool boolean)
        {
            _dictionary.Add(name, boolean);
        }

        public int Count()
        {
            var cnt = _dictionary.Count(item => item.Value);
            return cnt;
        }
    }
}
