using LLGameStudio.Common.XML;
using LLGameStudio.Studio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LLGameStudio.Game.UI
{
    class LLGameCanvas : IUINode
    {
        public override XElement ExportContentToXML()
        {
            XElement element = new XElement("LLGameCanvas");
            ExportAttrbuteToXML(element);
            return element;
        }

        public override void LoadContentFromXML(XElement element)
        {
            LoadAttrbuteFromXML(element);

            foreach (var item in element.Attributes())
            {

            }
        }

        public override void AddUINodeToCanvas(CanvasManager canvasManager)
        {
            
        }
    }
}
