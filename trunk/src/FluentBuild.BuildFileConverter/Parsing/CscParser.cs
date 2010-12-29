using System;
using System.Collections.Generic;
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

        public IList<string> References { get; set; }

        public void Parse(XElement data)
        {
            //var references = data.Element("References");
            //if (references != null)
            //{
            //    references.Elements("includes");
            //}
        }

        public string GererateString()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}