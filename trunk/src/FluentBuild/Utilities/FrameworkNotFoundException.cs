using System;
using System.Text;

namespace FluentBuild.Utilities
{
    ///<summary>
    /// Raises when the desired .NET framework can not be found
    ///</summary>
    public class FrameworkNotFoundException : Exception
    {
        private readonly string _message;

        public override string Message
        {
            get { return _message; }
        }

        ///<summary>
        /// Creates a new exception with the message populated
        ///</summary>
        ///<param name="frameworkInstallRoot">Paths that were searched to find the install path</param>
        public FrameworkNotFoundException(string[] frameworkInstallRoot)
        {
            var sb = new StringBuilder();
            sb.Append("Could not find the .NET Framework install path by searching paths ");
            foreach (var paths in frameworkInstallRoot)
            {
                sb.Append(paths);
                sb.Append(", ");
            }
            sb.Remove(sb.Length - 2, 2);
            sb.Append(". Please make sure it is installed.");
            _message = sb.ToString();
        }
    }
}