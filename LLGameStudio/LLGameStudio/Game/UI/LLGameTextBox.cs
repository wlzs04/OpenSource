using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LLGameStudio.Game.UI
{
    class LLGameTextBox : IUINode
    {
        public LLGameTextBox()
        {
            
        }

        public override XElement ExportContentToXML()
        {
            XElement element = new XElement("LLGameTextBox");
            ExportAttrbuteToXML(element);
            return element;
        }

        public override void LoadContentFromXML(XElement element)
        {
            LoadAttrbuteFromXML(element);
        }
    }
}
