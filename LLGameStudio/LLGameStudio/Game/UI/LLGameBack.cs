using LLGameStudio.Common.XML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LLGameStudio.Game.UI
{
    class LLGameBack : IXMLClass
    {
        string name;

        public XElement ExportContentToXML()
        {
            throw new NotImplementedException();
        }

        public void LoadContentFromXML(XElement element)
        {
            name = element.Attribute("name").Value;
        }
    }
}
