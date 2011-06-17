using System;
using System.Collections.Generic;

namespace FluentBuild.Core
{
    ///<summary>
    /// Represents a Build file that can be run
    ///</summary>
    public class BuildFile
    {
        internal Queue<NamedTask> Tasks;

        
        ///<summary>
        /// Invokes the next task in the queue
        ///</summary>
        public void InvokeNextTask()
        {
            while (Tasks.Count > 0)
            {
                NamedTask task = Tasks.Dequeue();
                //do not run another task if a previous task has errored
                if (Environment.ExitCode == 0)
                {
                    MessageLogger.WriteHeader(task.Name);
					try
					{
						task.Task.Invoke();
					}
					catch( Exception ex )
					{
						MessageLogger.WriteError( ex.Message );
						Environment.ExitCode = 1;
					}
                }
            }
			if(Environment.ExitCode == 0)
				MessageLogger.WriteHeader("DONE");
        }

        ///<summary>
        /// Instantiates a build file and initializes the Tasks queue.
        ///</summary>
        public BuildFile()
        {
            Tasks = new Queue<NamedTask>();
        }

        ///<summary>
        /// Clears the task list
        ///</summary>
        public void ClearTasks()
        {
            Tasks.Clear();
        }

        ///<summary>
        /// Adds a task for fb.exe to run in the order that it should be run
        ///</summary>
        ///<param name="task">The method to run</param>
        public void AddTask(Action task)
        {
            Tasks.Enqueue(new NamedTask(task.Method.Name, task));
        }

        ///<summary>
        /// Adds a task for fb.exe to run in the order that it should be run
        ///</summary>
        ///<param name="task">The method to run</param>
        ///<param name="name">The name of the task (will be displayed when the task is run)</param>
        public void AddTask(string name, Action task)
        {
            Tasks.Enqueue(new NamedTask(name, task));
        }

        internal class NamedTask
        {
            public string Name { get; set; }
            public Action Task { get; set; }

            public NamedTask(string name, Action task)
            {
                Name = name;
                Task = task;
            }
        }
        

        ///<summary>
        /// Gets the number of tasks in the queue.
        ///</summary>
        ///<returns>The number of tasks in the queue</returns>
        public int TaskCount
        {
            get { return Tasks.Count; } 
        }
    }
}