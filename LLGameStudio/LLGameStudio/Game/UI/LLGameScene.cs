using LLGameStudio.Common;
using LLGameStudio.Common.XML;
using LLGameStudio.Studio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LLGameStudio.Game.UI
{
    /// <summary>
    /// UI场景类
    /// </summary>
    class LLGameScene : IUINode
    {
        string filePath = "";

        public LLGameScene()
        {
        }

        public bool LoadContentFromFile(string path)
        {
            filePath = path;
            LLConvert.LoadContentFromXML(path, this);
            return true;
        }

        public override XElement ExportContentToXML()
        {
            XElement element = new XElement("LLGameScene");
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
                    case "LLGameBack":
                        LLGameBack back = new LLGameBack();
                        back.LoadContentFromXML(item);
                        AddNode(back);
                        break;
                    case "LLGameCanvas":
                        LLGameCanvas canvas = new LLGameCanvas();
                        canvas.LoadContentFromXML(item);
                        AddNode(canvas);
                        break;
                    case "LLGameLayout":
                        AddNode(new LLGameLayout());
                        listNode[listNode.Count - 1].LoadContentFromXML(item);
                        
                        break;
                    default:
                        break;
                }
            }
        }

        public override void AddUINodeToCanvas(CanvasManager canvasManager)
        {
            canvasManager.AddRootUINode(this);
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
