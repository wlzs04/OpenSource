using LLGameStudio.Common.XML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LLGameStudio.Common.Config
{
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

        public string GetColorValueByName(string name)
        {
            return dictionaryColor[name];
        }
    }
}
