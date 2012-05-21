using System;
using System.Reflection;

namespace FluentBuild
{
    public interface IActionExcecutor
    {
        void Execute<T>(Action<T> args) where T : InternalExecuatable, new();
        void Execute<T, TParams>(Action<T> args, TParams constructorParms) where T : InternalExecuatable;
    }

    internal class ActionExcecutor : IActionExcecutor
    {
        public void Execute<T>(Action<T> args) where T : InternalExecuatable, new()
        {
            var concrete = new T();
            args(concrete);
            concrete.InternalExecute();
        }

        public ConstructorInfo FindConstructor<T,TParams>()
        {
            BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
            //try for a concrete implementation
            var constructor = typeof(T).GetConstructor(bindingFlags, null, new[] { typeof(TParams) }, null);
            
            //If that did not work check the base type for a concrete match
            if (constructor == null)
                constructor = typeof (T).GetConstructor(bindingFlags, null, new[] {typeof (TParams).BaseType}, null);
            
            //if that did not work check all interfaces. Return the first matching
            if (constructor == null)
            {
                foreach (var i in typeof(TParams).GetInterfaces())
                {
                    
                    constructor = typeof(T).GetConstructor(bindingFlags , null,  new[] { i }, null);
                    if (constructor != null)
                        return constructor;
                }
            }

            //no matches found
            if (constructor == null)
                throw new ApplicationException("Could not find a matching constructor");
            return constructor;
        }

        public void Execute<T, TParams>(Action<T> args, TParams constructorParms) where T : InternalExecuatable
        {
            var concrete = (T)FindConstructor<T,TParams>().Invoke(new object[] { constructorParms });
            args(concrete);
            concrete.InternalExecute();
        }
    }
}