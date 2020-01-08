using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace MabinogiBackuperLib.Registory
{
    public class RegEdit
    {
        public static string GetValue(string keyPath, string valueName)
        {
            var rKeyName = keyPath;
            var rGetValueName = valueName;

            using var registryKey = Registry.LocalMachine.OpenSubKey(rKeyName);
            var location = registryKey?.GetValue(rGetValueName);
            return location as string;
        }

        public static void SetValue(string keyPath, string valueName, string value)
        {
            using var registryKey = Registry.LocalMachine.CreateSubKey(keyPath);
            registryKey?.SetValue(valueName, value);
        }
    }
}
