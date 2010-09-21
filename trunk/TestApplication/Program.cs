using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestApplication
{
    public class Program
    {
        static void Main(string[] args)
        {
            var environmentVariables = Environment.GetEnvironmentVariables();
            foreach (var name in environmentVariables.Keys)
            {
                Console.WriteLine(name + "=" + environmentVariables[name]);
            }
        }
    }
}
