using LLGameStudio.Common.XML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LLGameStudio.Common.Config
{
    /// <summary>
    /// 主题配置
    /// </summary>
    class ThemeConfig : IXMLClass
    {
        Dictionary<string, string> dictionaryColor = new Dictionary<string, string>();

        public XElement ExportContentToXML()
        {
            throw new NotImplementedException();
        }

        public void LoadContentFromXML(XElement element)
        {
            foreach (var item in element.Elements())
            {
                dictionaryColor.Add(item.Name.ToString(), item.Value);
            }
        }

        /// <summary>
        /// 通过传入名获得相应主题颜色例如“#00000000”的字符串
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetColorValueByName(string name)
        {
            return dictionaryColor[name];
        }
    }
}
