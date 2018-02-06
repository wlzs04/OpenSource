using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using LLGameStudio.Studio;

namespace LLGameStudio.Game.UI
{
    /// <summary>
    /// UI控件，容器
    /// </summary>
    class LLGameGrid : IUINode
    {
        public override XElement ExportContentToXML()
        {
            XElement element = new XElement("LLGameGrid");
            ExportAttrbuteToXML(element);
            foreach (var item in listNode)
            {
                element.Add(item.ExportContentToXML());
            }
            return element;
        }

        public override void LoadContentFromXML(XElement element)
        {
            LoadAttrbuteFromXML(element);

            foreach (var item in element.Elements())
            {
                switch (item.Name.ToString())
                {
                    case "LLGameImage":
                        AddNode(new LLGameImage());
                        listNode[listNode.Count - 1].LoadContentFromXML(item);
                        
                        break;
                    default:
                        break;
                }
            }
        }

        public override void AddUINodeToCanvas(CanvasManager canvasManager)
        {
        }

        public override void ResetUIProperty()
        {
            base.ResetUIProperty();
            foreach (var item in listNode)
            {
                item.ResetUIProperty();
            }
        }
    }
}
