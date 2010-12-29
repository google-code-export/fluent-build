﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using FluentBuild.MessageLoggers;

namespace FluentBuild.BuildUI
{
    /// <summary>
    /// Interaction logic for BuildProgress.xaml
    /// </summary>
    public partial class BuildProgress : UserControl, IMessageLogger
    {
        public BuildProgress()
        {
            InitializeComponent();
            BuildNotices = new ObservableCollection<BuildData>();
            DataContext = this;
        }

        public ObservableCollection<BuildData> BuildNotices { get; set; }

        #region IMessageLogger Members

        public void WriteHeader(string header)
        {
            Dispatcher.BeginInvoke(new Action(delegate
                                                  {
                                                      if (BuildNotices.Count > 0)
                                                      {
                                                          BuildNotices.Last().Completed = true;
                                                      }
                                                      BuildNotices.Add(new BuildData(header));
                                                  }));
        }

        public void WriteDebugMessage(string message)
        {
            Dispatcher.BeginInvoke(new Action(() => BuildNotices.Last().AddItem(message, TaskState.Normal)));
        }

        public void Write(string type, string message)
        {
            Dispatcher.BeginInvoke(new Action(() => BuildNotices.Last().AddItem(message, TaskState.Normal)));
        }

        public void WriteError(string type, string message)
        {
            Dispatcher.BeginInvoke(new Action(delegate { BuildNotices.Last().AddItem(message, TaskState.Error);
                                                           BuildNotices.Last().State = TaskState.Error;
            }));
        }

        public void WriteWarning(string type, string message)
        {
            Dispatcher.BeginInvoke(new Action(delegate { BuildNotices.Last().AddItem(message, TaskState.Warning);
                                                           BuildNotices.Last().State = TaskState.Warning;
            }));
        }

        public ITestSuiteMessageLogger WriteTestSuiteStared(string name)
        {
            Dispatcher.BeginInvoke(new Action(() => BuildNotices.Last().AddItem(name, TaskState.Normal)));
            return new UnitTestSuiteHandler(Dispatcher, BuildNotices.Last());
        }

        #endregion

    }
}