using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentBuild.Publishing;

namespace FluentBuild.Core
{
    public class Publish
    {
        public static GoogleCode ToGoogleCode
        {
            get { return new GoogleCode(); }
        }
    }
}
