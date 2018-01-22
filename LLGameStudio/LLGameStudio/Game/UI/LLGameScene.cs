using LLGameStudio.Common.XML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LLGameStudio.Game.UI
{
    class LLGameScene : IXMLClass
    {
        string name;
        LLGameCanvas canvas;
        List<LLGameLayout> listLayout;

        string filePath;

        public LLGameScene(string path)
        {
            filePath = path;
            
            LLXMLConverter converter = new LLXMLConverter();
            converter.LoadContentFromXML(path, this);

        }

        public XElement ExportContentToXML()
        {
            return null;
        }

        public void LoadContentFromXML(XElement element)
        {
            name = element.Attribute("name").Value;

            foreach (var item in element.Elements())
            {
                switch (item.Name.ToString())
                {
                    case "LLGameMap":

                        break;
                    case "LLGameCanvas":
                        break;
                    case "LLGameLayout":
                        listLayout.Add(new LLGameLayout());
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
