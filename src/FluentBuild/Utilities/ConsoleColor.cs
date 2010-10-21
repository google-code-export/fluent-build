using System;
using System.Runtime.InteropServices;

namespace FluentBuild.Utilities
{
    /// <summary>
    /// Sets the color of the console to support colorized output
    /// </summary>
    internal class ConsoleColor
    {
        [DllImport("kernel32.dll")]
        public static extern bool SetConsoleTextAttribute(IntPtr hConsoleOutput, int wAttributes);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetStdHandle(uint nStdHandle);

        public enum BuildColor { Default=007, Red=12, Yellow=14, Green=10, Aqua=11, Purple=13};
        
        //public static void ShowAllColors()
        //{
        //    //for (int k = 1; k < 130; k++)
        //    //{
        //    //    SetConsoleTextAttribute(hConsole, k);

        //    //    Console.WriteLine("{0:d3} I want to be nice today!", k);
        //    //}

        //    //go back to the default
        //    //SetConsoleTextAttribute(hConsole, 007);
        //}

        internal static void SetColor(BuildColor color)
        {
            //0xfffffff5 is the default handle for reasons unknown
            IntPtr hConsole = GetStdHandle(0xfffffff5);
            SetConsoleTextAttribute(hConsole, (int)color);
        }

        public static IDisposable SetTemporaryColor(BuildColor color)
        {
            SetColor(color);
            return new ReturnColorToDefault();
        }
    }

    internal class ReturnColorToDefault : IDisposable
    {
        public void Dispose()
        {
            ConsoleColor.SetColor(ConsoleColor.BuildColor.Default);
        }
    }
}