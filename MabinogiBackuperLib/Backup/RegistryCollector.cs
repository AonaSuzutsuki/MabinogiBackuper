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
    public class RegistryCollector
    {
        private const string RegistryPath = @"Software\Nexon\Mabinogi";

        public IRegistryEditor RegistryEditor { get; set; } = new RegistryEditor();

        private static Dictionary<string, string> Analyze(IRegistryEditor registryEditor)
        {
            var keyNames = registryEditor.GetKeyNames(RegistryPath);
            var table = new Dictionary<string, string>(keyNames.Length);
            foreach (var keyName in keyNames)
                table.Put(keyName, registryEditor.GetValue(RegistryPath, keyName, Registry.CurrentUser));

            return table;
        }

        public string GetJson()
        {
            var table = Analyze(RegistryEditor);
            return JsonConvert.SerializeObject(table);
        }
    }
}
