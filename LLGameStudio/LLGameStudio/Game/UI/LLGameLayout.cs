using LLGameStudio.Common.XML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LLGameStudio.Game.UI
{
    class LLGameLayout : IXMLClass ,ILLGameUINode
    {
        string name;
        string filePath;
        
        public bool LoadContentFromFile(string path)
        {
            filePath = path;
            LLXMLConverter.LoadContentFromXML(path, this);
            return true;
        }

        public XElement ExportContentToXML()
        {
            throw new NotImplementedException();
        }

        public void LoadContentFromXML(XElement element)
        {
            name = element.Attribute("name").Value;
        }

        public void Render()
        {
            
        }
    }
}
