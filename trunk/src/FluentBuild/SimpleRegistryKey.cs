using System.Collections.Generic;
using Microsoft.Win32;

namespace FluentBuild
{
    public interface ISimpleRegistryKey
    {
        IEnumerable<string> GetSubKeyNames();
        ISimpleRegistryKey OpenSubKey(string keyName);
        object GetValue(string name);
        void Close();
    }

    public class SimpleRegistryKey : ISimpleRegistryKey
    {
        private readonly RegistryKey _key;

        public SimpleRegistryKey(RegistryKey key)
        {
            _key = key;
        }

        public SimpleRegistryKey()
        {
            
        }

        public IEnumerable<string> GetSubKeyNames()
        {
            return _key.GetSubKeyNames();
        }

        public ISimpleRegistryKey OpenSubKey(string keyName)
        {
            return new SimpleRegistryKey(_key.OpenSubKey(keyName));
        }

        public object GetValue(string name)
        {
            return _key.GetValue(name);
        }

        public void Close()
        {
            _key.Close();
        }
    }
}