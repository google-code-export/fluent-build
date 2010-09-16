using System;
using FluentBuild.FilesAndDirectories;

namespace FluentBuild
{
    public interface IFailable<T>
    {
        T FailOnError { get; }
        T ContinueOnError { get; }
    }

    public abstract class Failable<T> : IFailable<T>
    {
        protected OnError OnError;
        protected abstract T GetSelf { get;  }

        public Failable()
        {
            OnError = Defaults.OnError;
        }

        public T FailOnError
        {
            get
            {
                OnError = OnError.Continue;
                return GetSelf;
            }
        }

        public T ContinueOnError
        {
            get
            {
                OnError = OnError.Continue;
                return GetSelf;
            }
        }

    }
}