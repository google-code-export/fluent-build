using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FluentBuild.UtilitySupport;
using UserControl = System.Windows.Controls.UserControl;

namespace FluentBuild.BuildUI.Controls
{
    /// <summary>
    /// Interaction logic for Header.xaml
    /// </summary>
    public partial class Header : UserControl, INotifyPropertyChanged
    {
        private string _buildAssembly;
        private string _classToRun;

        public Header()
        {
            InitializeComponent();
            Path.Text = SettingHelper.LastPath;
            WorkingDirectory.Text = SettingHelper.LastWorkingDirectory;
            Compile(Path.Text);
            Defaults.Logger.Verbosity = VerbosityLevel.Full;
        }

                
        public event EventHandler Reset;

        public void InvokeReset()
        {
            EventHandler handler = Reset;
            if (handler != null) handler(this, new EventArgs());
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        private void BrowseClick(object sender, RoutedEventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.SelectedPath = Path.Text;
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Path.Text = dialog.SelectedPath;
                    WorkingDirectory.Text = dialog.SelectedPath;
                    InvokePropertyChanged("Path");
                    InvokePropertyChanged("WorkingDirectory");
                    
                    Compile(dialog.SelectedPath);
                }
            }
        }

        private void Compile(string selectedPath)
        {
            if (string.IsNullOrEmpty(selectedPath))
                return;

            InvokeReset();
            
            
            Defaults.Logger.WriteHeader("Compile Build File");
            if (!Directory.Exists(selectedPath))
            {
                Defaults.Logger.WriteError("Folder Not Found", "Could not find the build folder at " + selectedPath);
                return;
            }

            try
            {
                _buildAssembly = CompilerService.BuildAssemblyFromSources(selectedPath);
                //if (output != string.Empty)
                //    Defaults.Logger.WriteError("Compile", output);
                //_buildAssembly = Compiler.BuildAssemblyFromSources(selectedPath);
                Targets.ItemsSource = CompilerService.FindBuildClasses(_buildAssembly);
//                Targets.ItemsSource = Compiler.FindBuildClasses(_buildAssembly);
                Targets.SelectedIndex = 0;
                InvokePropertyChanged("Targets");
            }
            catch (Exception e)
            {
                Defaults.Logger.WriteError("Compile Build File", e.ToString());
            }

            Defaults.Logger.WriteHeader("Done");
        }

        public void InvokePropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(name));
        }

        private void RunCurrentBuild(object sender, RoutedEventArgs e)
        {
            InvokeReset();

            Defaults.Logger.WriteHeader("Start");
            SettingHelper.LastPath = Path.Text;
            SettingHelper.LastWorkingDirectory = WorkingDirectory.Text;
            
            Directory.SetCurrentDirectory(WorkingDirectory.Text);
            Environment.CurrentDirectory = WorkingDirectory.Text;
            _classToRun = Targets.SelectedItem.ToString();
            var th = new Thread(StartCompile);
            th.Start();
           
        }

        private void StartCompile()
        {
            
            var output = CompilerService.ExecuteBuildTask(_buildAssembly, _classToRun, null);
            if (output != string.Empty)
                Defaults.Logger.WriteError("", output);
            //            var runner = new Runner(Targets.SelectedItem.ToString(), _buildAssembly);
            //          runner.Run();
        }

        private void Refresh(object sender, RoutedEventArgs e)
        {
            Compile(Path.Text);
        }

        private void WorkingDirectoryBrowseClick(object sender, RoutedEventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.SelectedPath = WorkingDirectory.Text;
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    WorkingDirectory.Text = dialog.SelectedPath;
                    InvokePropertyChanged("WorkingDirectory");
                }
            }
        }

    }
}
