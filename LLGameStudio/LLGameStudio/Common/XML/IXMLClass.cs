using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LLGameStudio.Common.XML
{
    /// <summary>
    /// 实现用于与xml文件交互的接口，配合LLConvert类使用
    /// </summary>
    interface IXMLClass
    {
        /// <summary>
        /// 从XML节点中加载内容
        /// </summary>
        /// <param name="element"></param>
        void LoadContentFromXML(XElement element);

        /// <summary>
        /// 将内容导出为XML节点。
        /// </summary>
        /// <returns></returns>
        XElement ExportContentToXML();
    }
}
