using System;
using System.Xml.Linq;

namespace FluentBuild.BuildFileConverter.Parsing
{
    //call parser
    //exec parser
    //delete parser
    //mkdir parser
    //asminfo parser
    //zipfile parser

    public class CscParser : ITaskParser
    {
        #region ITaskParser Members

        public void Parse(XElement data)
        {
            throw new NotImplementedException();
        }

        public string GererateString()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}