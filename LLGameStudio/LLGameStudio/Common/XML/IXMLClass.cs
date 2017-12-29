using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LLGameStudio.Common.XML
{
    interface IXMLClass
    {
        void LoadContentFromXML(XElement element);
        XElement ExportContentToXML();
    }
}
