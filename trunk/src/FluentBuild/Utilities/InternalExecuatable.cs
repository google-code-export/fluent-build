namespace FluentBuild.Utilities
{
    ///<summary>
    /// This is a marker class for items that can be executed by being passed via a lambda (e.g. Task.Run(x=>x.ExecutablePath)
    ///</summary>
    public abstract class InternalExecuatable
    {
        internal abstract void InternalExecute();
    }
}
