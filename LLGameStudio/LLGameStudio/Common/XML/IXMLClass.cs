using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LLGameStudio.Common.XML
{
    /// <summary>
    /// 实现用于与xml文件交互的接口，配合LLXMLConverter类使用
    /// </summary>
    interface IXMLClass
    {
        void LoadContentFromXML(XElement element);
        XElement ExportContentToXML();
    }
}
