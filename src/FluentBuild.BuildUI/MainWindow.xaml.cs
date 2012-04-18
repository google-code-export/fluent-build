using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Forms;

using FluentBuild.Core;
using FluentFs.Core;
using Directory = System.IO.Directory;

namespace FluentBuild.BuildUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private string _buildAssembly;

        public MainWindow()
        {
            InitializeComponent();
            //TODO: remember path
            this.Path.Text = @"C:\Users\Kudos\Desktop\fluent-build\src\FluentBuild.Build";
            this.WorkingDirectory.Text = @"C:\Users\Kudos\Desktop\fluent-build";
            Compile(this.Path.Text);
            this.Targets.SelectedIndex = 0;

        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Path.Text = dialog.SelectedPath;
                    InvokePropertyChanged("Path");
                    Compile(dialog.SelectedPath);
                }
            }
        }

        private void Compile(string selectedPath)
        {
            _buildAssembly = Compiler.BuildAssemblyFromSources(selectedPath);
            Targets.ItemsSource = Compiler.FindBuildClasses(_buildAssembly);
            InvokePropertyChanged("Targets");
        }

        public void InvokePropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(name));
        }
        
        private void RunCurrentBuild(object sender, RoutedEventArgs e)
        {
            Directory.SetCurrentDirectory(this.WorkingDirectory.Text);
            Environment.CurrentDirectory = this.WorkingDirectory.Text;

            var runner = new Runner(Targets.SelectedItem.ToString(), BuildProgress, _buildAssembly);
            runner.Run();
        }
    }

    public class Compiler
    {
        /// <summary>
        /// Builds an assembly from a source folder. Currently this only works with .cs files
        /// </summary>
        /// <param name="path">The path to the source files</param>
        /// <returns>returns the path to the compiled assembly</returns>
        public static string BuildAssemblyFromSources(string path)
        {
            Defaults.Logger.WriteDebugMessage("Sources found in: " + path);
            var fileset = new FileSet();
            fileset.Include(path + "\\**\\*.cs");

            string startPath =
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Replace("file:\\", "");

            string dllReference = Path.Combine(startPath, "FluentBuild.dll");
            Defaults.Logger.WriteDebugMessage("Adding in reference to the FluentBuild DLL from: " + dllReference);
            var tempPath = Environment.GetEnvironmentVariable("TEMP") + "\\FluentBuild\\" + DateTime.Now.Ticks.ToString();
            Directory.CreateDirectory(tempPath);
            string outputAssembly = Path.Combine(tempPath, "build.dll");
            Defaults.Logger.WriteDebugMessage("Output Assembly: " + outputAssembly);
            Task.Build(Using.Csc.Target.Library.AddSources(fileset).AddRefences(dllReference).OutputFileTo(outputAssembly).
                IncludeDebugSymbols);
            return outputAssembly;
        }

        public static IEnumerable<Type> FindBuildClasses(string path)
        {
            Defaults.Logger.WriteDebugMessage("Executing DLL build from " + path);

            Defaults.Logger.Write("INFO", "Using framework " + Defaults.FrameworkVersion.ToString());
            Assembly assemblyInstance = Assembly.LoadFile(path);
            Type[] types = assemblyInstance.GetTypes();
            return types.Where(t => t.IsSubclassOf(typeof(BuildFile)));
        }

    }

}