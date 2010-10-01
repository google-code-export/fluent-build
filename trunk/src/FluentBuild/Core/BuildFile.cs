using System;
using System.Collections.Generic;

namespace FluentBuild.Core
{
    ///<summary>
    /// Represents a Build file that can be run
    ///</summary>
    public class BuildFile
    {
        internal Queue<Action> Tasks;

        
        ///<summary>
        /// Invokes the next task in the queue
        ///</summary>
        public void InvokeNextTask()
        {
            while (Tasks.Count > 0)
            {
                Action task = Tasks.Dequeue();
                MessageLogger.WriteHeader(task.Method.Name);
                task.Invoke();
            }

           MessageLogger.WriteHeader("DONE");
        }

        ///<summary>
        /// Instantiates a build file and initializes the Tasks queue.
        ///</summary>
        public BuildFile()
        {
            Tasks = new Queue<Action>();
        }


        ///<summary>
        /// Adds a task for fb.exe to run in the order that it should be run
        ///</summary>
        ///<param name="task">The method to run</param>
        public void AddTask(Action task)
        {
            Tasks.Enqueue(task);
        }

        ///<summary>
        /// Gets the number of tasks in the queue.
        ///</summary>
        ///<returns>The number of tasks in the queue</returns>
        public int TaskCount()
        {
            return Tasks.Count;
        }
    }
}