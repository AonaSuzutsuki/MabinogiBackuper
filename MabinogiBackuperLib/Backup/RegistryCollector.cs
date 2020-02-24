using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonExtensionLib.Extensions;
using MabinogiBackuperLib.ExRegistry;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace MabinogiBackuperLib.Backup
{
    public interface IProgressEventArgs
    {
        int Total { get; set; }
        int Current { get; set; }
        int Percentage { get; }
        string Name { get; set; }
    }
    public class RegistryEventArgs : IProgressEventArgs
    {
        public int Total { get; set; }
        public int Current { get; set; }
        public int Percentage => (int)((double)Current / Total * 100);
        public string Name { get; set; }
    }

    public class RegistryCollector
    {
        private const string RegistryPath = @"Software\Nexon\Mabinogi";

        public IRegistryEditor RegistryEditor { get; set; } = new RegistryEditor();

        private static Dictionary<string, string> Analyze(IRegistryEditor registryEditor, Action<RegistryEventArgs> callBack)
        {
            var keyNames = registryEditor.GetKeyNames(RegistryPath);
            var table = new Dictionary<string, string>(keyNames.Length);
            foreach (var item in keyNames.Select((v, i) => new {Index = i, Value = v}))
            {
                table.Put(item.Value, registryEditor.GetValue(RegistryPath, item.Value, Registry.CurrentUser));
                callBack(new RegistryEventArgs
                {
                    Total = keyNames.Length,
                    Current = item.Index + 1,
                    Name = item.Value
                });
            }

            return table;
        }

        public string GetJson(Action<RegistryEventArgs> callBack)
        {
            var table = Analyze(RegistryEditor, callBack);
            return JsonConvert.SerializeObject(table);
        }
    }
}
