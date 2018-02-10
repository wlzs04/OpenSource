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
    /// 游戏配置
    /// </summary>
    class GameConfig : IXMLClass
    {
        int width=800;
        int height=600;
        string gameName="游戏";
        bool fullScreen=false;

        public int Width { get => width; set => width = value; }
        public int Height { get => height; set => height = value; }
        public string GameName { get => gameName; set => gameName = value; }
        public bool FullScreen { get => fullScreen; set => fullScreen = value; }
        
        public XElement ExportContentToXML()
        {
            XElement element = new XElement("Studio");
            element.Add(new XAttribute("width", width));
            element.Add(new XAttribute("height", height));
            element.Add(new XAttribute("fullScreen", fullScreen));
            element.Add(new XAttribute("gameName", gameName));
            return element;
        }

        public void LoadContentFromXML(XElement element)
        {
            width = Convert.ToInt32(element.Attribute("width").Value);
            height = Convert.ToInt32(element.Attribute("height").Value);
            fullScreen = Convert.ToBoolean(element.Attribute("fullScreen").Value);
            gameName = element.Attribute("gameName").Value;
        }
    }
}
