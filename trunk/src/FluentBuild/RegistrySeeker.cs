using System;
using Microsoft.Win32;

namespace FluentBuild
{
    public interface IRegistrySeeker
    {
        ISimpleRegistryKey OpenLocalMachineKey(string key);
    }


    /// <summary>
    /// Wrapper around registry access to provide testability
    /// </summary>
    internal class RegistrySeeker : IRegistrySeeker
    {
        public ISimpleRegistryKey OpenLocalMachineKey(string key)
        {
            RegistryKey subKey = Registry.LocalMachine.OpenSubKey(key);
            if (subKey==null)
                return null;
            return new SimpleRegistryKey(subKey);
        }
    }
}