using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentBuild
{
    ///<summary>
    /// This is a marker class for items that can be executed by being passed via a lambda (e.g. Task.Run(x=>x.ExecutablePath)
    ///</summary>
    public abstract class InternalExecuatable
    {
        internal abstract void InternalExecute();
    }
}
