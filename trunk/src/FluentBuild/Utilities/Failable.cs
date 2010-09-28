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

        public Failable()
        {
            OnError = Defaults.OnError;
        }

        public T FailOnError
        {
            get
            {
                OnError = OnError.Fail;
                return (T)(object)this;
            }
        }

        public T ContinueOnError
        {
            get
            {
                OnError = OnError.Continue;
                return (T)(object)this;
            }
        }

    }
}