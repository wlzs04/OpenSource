using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using LLGameStudio.Common.XML;

namespace LLGameStudio.Common.Config
{
    /// <summary>
    /// 编辑器配置
    /// </summary>
    class StudioConfig : IXMLClass
    {
        int top=0;
        int left=0;
        int width=800;
        int height=600;
        string studioName="LLGameStudio";
        bool fullScreen=true;
        int borderWidth = 3;
        string version = "1.0.0";
        string theme = "Default";
        string lastGamePath = "";

        public int Top { get => top; set => top = value; }
        public int Left { get => left; set => left = value; }
        public int Width { get => width; set => width = value; }
        public int Height { get => height; set => height = value; }
        public string StudioName { get => studioName; set => studioName = value; }
        public bool FullScreen { get => fullScreen; set => fullScreen = value; }
        public int BorderWidth { get => borderWidth; set => borderWidth = value; }
        public string Version { get => version; set => version = value; }
        public string Theme { get => theme; set => theme = value; }
        public string LastGamePath { get => lastGamePath; set => lastGamePath = value; }

        public void LoadContentFromXML(XElement element)
        {
            top = Convert.ToInt32(element.Attribute("top").Value);
            left = Convert.ToInt32(element.Attribute("left").Value);
            width = Convert.ToInt32(element.Attribute("width").Value);
            height = Convert.ToInt32(element.Attribute("height").Value);
            borderWidth = Convert.ToInt32(element.Attribute("borderWidth").Value);
            fullScreen = Convert.ToBoolean(element.Attribute("fullScreen").Value);
            studioName = element.Attribute("studioName").Value;
            version = element.Attribute("version").Value;
            theme = element.Attribute("theme").Value;
            lastGamePath = element.Attribute("lastGamePath").Value;
        }

        public XElement ExportContentToXML()
        {
            XElement element = new XElement("Studio");
            element.Add(new XAttribute("top", top));
            element.Add(new XAttribute("left", left));
            element.Add(new XAttribute("width", width));
            element.Add(new XAttribute("height", height));
            element.Add(new XAttribute("borderWidth", BorderWidth));
            element.Add(new XAttribute("fullScreen", fullScreen));
            element.Add(new XAttribute("studioName", studioName));
            element.Add(new XAttribute("version", version));
            element.Add(new XAttribute("theme", theme));
            element.Add(new XAttribute("lastGamePath", lastGamePath));
            return element;
        }
    }
}

enum Window32MessageEnum
{
    WM_NCHITTEST = 0x0084
}

enum Window32HandleEnum
{
    HTCAPTION = 2,
    HTLEFT = 10,
    HTRIGHT = 11,
    HTTOP = 12,
    HTTOPLEFT = 13,
    HTTOPRIGHT = 14,
    HTBOTTOM = 15,
    HTBOTTOMLEFT = 16,
    HTBOTTOMRIGHT = 17,
}

