using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace FluentBuild.BuildUI
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private string _buildAssembly;

        public MainWindow()
        {
            InitializeComponent();
            Defaults.SetLogger(BuildProgress);

            Path.Text = SettingHelper.LastPath;
            WorkingDirectory.Text = SettingHelper.LastWorkingDirectory;
            Compile(Path.Text);
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
                    InvokePropertyChanged("Path");
                    Compile(dialog.SelectedPath);
                    
                }
            }
        }

        private void Compile(string selectedPath)
        {
            if (string.IsNullOrEmpty(selectedPath))
                return;

            BuildProgress.Reset();
            Defaults.Logger.Verbosity = VerbosityLevel.Full;
            Defaults.Logger.WriteHeader("Compile Build File");
            if (!Directory.Exists(selectedPath))
            {
                Defaults.Logger.WriteError("Folder Not Found", "Could not find the build folder at " + selectedPath);
                return;
            }

            try
            {
                _buildAssembly = Compiler.BuildAssemblyFromSources(selectedPath);
                Targets.ItemsSource = Compiler.FindBuildClasses(_buildAssembly);
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
            SettingHelper.LastPath = Path.Text;
            SettingHelper.LastWorkingDirectory = WorkingDirectory.Text;

            BuildProgress.Reset();
            Directory.SetCurrentDirectory(WorkingDirectory.Text);
            Environment.CurrentDirectory = WorkingDirectory.Text;

            var runner = new Runner(Targets.SelectedItem.ToString(), BuildProgress, _buildAssembly);
            runner.Run();
        }

        private void Refresh(object sender, RoutedEventArgs e)
        {
            Compile(Path.Text);
        }
    }
}