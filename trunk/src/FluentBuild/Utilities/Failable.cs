using FluentBuild.Core;

namespace FluentBuild.Utilities
{
    public interface IFailable<T>
    {
        T FailOnError { get; }
        T ContinueOnError { get; }
    }

    public abstract class Failable<T> : IFailable<T>
    {
        protected internal OnError OnError;
        protected internal abstract T GetSelf { get;  }

        public Failable()
        {
            OnError = Defaults.OnError;
        }

        public T FailOnError
        {
            get
            {
                OnError = OnError.Fail;
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