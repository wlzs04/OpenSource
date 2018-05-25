using LLGameStudio.Common.XML;
using LLGameStudio.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LLGameStudio.Common.Config
{
    /// <summary>
    /// 字体配置
    /// </summary>
    class FontConfig : IXMLClass
    {
        public Dictionary<string, LLFont> dictionaryFont = new Dictionary<string, LLFont>();

        public XElement ExportContentToXML()
        {
            XElement element = new XElement("Fonts");

            foreach (var item in dictionaryFont)
            {
                XElement fontElement = new XElement("Font");
                fontElement.Add(new XAttribute("name", item.Value.Name));
                fontElement.Add(new XAttribute("familyName", item.Value.FamilyName));
                fontElement.Add(new XAttribute("color", item.Value.Color));
                fontElement.Add(new XAttribute("size", item.Value.Size));
                element.Add(fontElement);
            }
            return element;
        }

        public void LoadContentFromXML(XElement element)
        {
            foreach (var item in element.Elements())
            {
                LLFont font = new LLFont
                {
                    Name = item.Attribute("name").Value,
                    FamilyName = item.Attribute("familyName").Value,
                    Color = item.Attribute("color").Value,
                    Size = Convert.ToDouble(item.Attribute("size").Value)
                };
                dictionaryFont.Add(font.Name, font);
            }
        }

        /// <summary>
        /// 通过传入名获得字体
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public LLFont GetFontByName(string name)
        {
            if(dictionaryFont.ContainsKey(name))
            {
                return dictionaryFont[name];
            }
            else
            {
                return new LLFont("font", "Microsoft YaHei","#FF000000",15);
            }
        }

    }
}
