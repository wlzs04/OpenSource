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
        List<IUINode> listNode;

        public LLGameGrid()
        {
            listNode = new List<IUINode>();
        }

        public override XElement ExportContentToXML()
        {
            throw new NotImplementedException();
        }

        public override void LoadContentFromXML(XElement element)
        {
            LoadAttrbuteFromXML(element);

            foreach (var item in element.Elements())
            {
                switch (item.Name.ToString())
                {
                    case "LLGameImage":
                        listNode.Add(new LLGameImage());
                        listNode[listNode.Count - 1].parentNode = this;
                        listNode[listNode.Count - 1].LoadContentFromXML(item);
                        break;
                    default:
                        break;
                }
            }
        }

        public override void AddUINodeToCanvas(CanvasManager canvasManager)
        {
            canvasManager.AddUINode(this);
            foreach (var item in listNode)
            {
                item.AddUINodeToCanvas(canvasManager);
            }
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
