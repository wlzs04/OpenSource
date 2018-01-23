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
        LLGameBack back;
        LLGameCanvas canvas;
        List<LLGameLayout> listLayout;

        string filePath;
        
        public bool LoadContentFromFile(string path)
        {
            filePath = path;
            listLayout = new List<LLGameLayout>();
            LLXMLConverter.LoadContentFromXML(path, this);
            return true;
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
                    case "LLGameBack":
                        back = new LLGameBack();
                        back.LoadContentFromXML(item);
                        break;
                    case "LLGameCanvas":
                        canvas = new LLGameCanvas();
                        canvas.LoadContentFromXML(item);
                        break;
                    case "LLGameLayout":
                        listLayout.Add(new LLGameLayout());
                        listLayout[listLayout.Count - 1].LoadContentFromXML(item);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
