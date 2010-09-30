using System;

namespace FluentBuild.Utilities
{
    public interface IRegistryKeyValueFinder
    {
        string FindFirstValue(params string[] keysToCheck);
    }

    public class RegistryKeyValueFinder : IRegistryKeyValueFinder
    {
        private readonly IRegistryWrapper _registryWrapper;

        public RegistryKeyValueFinder(IRegistryWrapper registryWrapper)
        {
            _registryWrapper = registryWrapper;
        }

        public RegistryKeyValueFinder() : this(new RegistryWrapper())
        {
        }

        public string FindFirstValue(params string[] keysToCheck)
        {
            foreach (var keyToCheck in keysToCheck)
            {
                var parts = keyToCheck.Split(@"\".ToCharArray());
                var key = _registryWrapper.OpenLocalMachineKey(parts[0]);
                for (int i = 1; i < parts.Length - 1; i++)
                {
                    key = key.OpenSubKey(parts[i]);
                    if (key == null) //key is null if it does not exist
                        break;
                }

                if (key!=null) //could open all keys now try to get the value
                    return key.GetValue(parts[parts.Length - 1]).ToString();                
            }
            return string.Empty; //can't find anything so return an emtpy string
        }
    }
}