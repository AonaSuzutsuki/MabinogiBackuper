using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace MabinogiBackuperLib.ExRegistry
{
    public interface IRegistryEditor
    {
        string GetValue(string keyPath, string valueName);
        string GetValue(string keyPath, string valueName, RegistryKey key);
        string[] GetKeyNames(string keyPath);
        string[] GetKeyNames(string keyPath, RegistryKey key);
        void SetValue(string keyPath, string valueName, string value);
        void SetValue(string keyPath, string valueName, string value, RegistryKey key);
    }

    public class RegistryEditor : IRegistryEditor
    {
        public string GetValue(string keyPath, string valueName) => 
            GetValue(keyPath, valueName, Registry.LocalMachine);

        public string GetValue(string keyPath, string valueName, RegistryKey key)
        {
            var rKeyName = keyPath;
            var rGetValueName = valueName;

            using var registryKey = key.OpenSubKey(rKeyName);
            var location = registryKey?.GetValue(rGetValueName);
            return location as string;
        }

        public string[] GetKeyNames(string keyPath) =>
            GetKeyNames(keyPath, Registry.CurrentUser);

        public string[] GetKeyNames(string keyPath, RegistryKey key)
        {
            using var regKey = key.OpenSubKey(keyPath, false);
            var values = regKey?.GetValueNames();
            return values;
        }


        public void SetValue(string keyPath, string valueName, string value) =>
            SetValue(keyPath, valueName, value, Registry.LocalMachine);

        public void SetValue(string keyPath, string valueName, string value, RegistryKey key)
        {
            using var registryKey = key.CreateSubKey(keyPath);
            registryKey?.SetValue(valueName, value);
        }
    }
}
