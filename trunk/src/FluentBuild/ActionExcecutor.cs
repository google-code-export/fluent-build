using System;

namespace FluentBuild
{
    internal interface IActionExcecutor<T> where T : InternalExecuatable, new()
    {
        void Execute(Action<T> args);
    }

    internal class ActionExcecutor<T> : IActionExcecutor<T> where T:InternalExecuatable, new()
    {
        public void Execute(Action<T> args)
        {
            var concrete = new T();
            args(concrete);
            concrete.InternalExecute();
        }
    }
}