using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Build
{
    class Program
    {
        static void Main(string[] args)
        {
            var build = new MainBuildTask();
            build.Execute();
        }
    }
}
