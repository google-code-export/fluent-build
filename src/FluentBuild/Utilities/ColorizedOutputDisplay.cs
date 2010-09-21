using System;
using FluentBuild.Core;

namespace FluentBuild.Utilities
{
    internal interface IColorizedOutputDisplay
    {
        void Display(string prefix, string dataToOutput, string errorDataToOutput, bool isErrorMessage);
    }

    internal class ColorizedOutputDisplay : IColorizedOutputDisplay
    {
        public void Display(string prefix, string dataToOutput, string errorDataToOutput, bool isErrorMessage)
        {
            ConsoleColor.BuildColor textColor = ConsoleColor.BuildColor.Default;

            if (isErrorMessage)
                textColor = ConsoleColor.BuildColor.Red;

            foreach (string line in dataToOutput.Split(Environment.NewLine.ToCharArray()))
            {
                if (line.Trim().Length > 0)
                {
                    ConsoleColor.SetColor(textColor);
                    if (line.Contains("warning") || line.Contains("Warning"))
                        ConsoleColor.SetColor(ConsoleColor.BuildColor.Yellow);
                    MessageLogger.Write(prefix, line);
                }
            }

            ConsoleColor.SetColor(textColor);
            foreach (string line in errorDataToOutput.Split(Environment.NewLine.ToCharArray()))
            {
                if (line.Trim().Length > 0)
                    MessageLogger.Write(prefix, line);
            }
            ConsoleColor.SetColor(ConsoleColor.BuildColor.Default);
        }
    }
}