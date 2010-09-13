using System;
using System.Collections.Generic;

namespace FluentBuild
{
    public interface IBuild
    {
        void Execute();
    }

    public class BuildFile
    {
        private Queue<Action> tasks;

        public void InvokeNextTask()
        {
            if (tasks.Count == 0)
                return;
            Action dequeue = tasks.Dequeue();
            MessageLogger.WriteHeader(dequeue.Method.Name);
            dequeue.Invoke();
            InvokeNextTask();
        }

        public BuildFile()
        {
            this.tasks = new Queue<Action>();
        }

        public void AddTask(Action task)
        {
            tasks.Enqueue(task);
        }

    }
}