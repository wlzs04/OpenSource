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
        LLGameBack back;
        LLGameCanvas canvas;
        List<LLGameLayout> listLayout;

        string filePath = "";

        public LLGameScene()
        {
            listLayout = new List<LLGameLayout>();
        }

        public bool LoadContentFromFile(string path)
        {
            filePath = path;
            listLayout = new List<LLGameLayout>();
            LLConvert.LoadContentFromXML(path, this);
            return true;
        }

        public override XElement ExportContentToXML()
        {
            return null;
        }

        public override void LoadContentFromXML(XElement element)
        {
            LoadAttrbuteFromXML(element);

            foreach (var item in element.Attributes())
            {

            }

            foreach (var item in element.Elements())
            {
                switch (item.Name.ToString())
                {
                    case "LLGameBack":
                        back = new LLGameBack();
                        back.LoadContentFromXML(item);
                        AddNode(back);
                        break;
                    case "LLGameCanvas":
                        canvas = new LLGameCanvas();
                        canvas.LoadContentFromXML(item);
                        AddNode(canvas);
                        break;
                    case "LLGameLayout":
                        listLayout.Add(new LLGameLayout());
                        listLayout[listLayout.Count - 1].LoadContentFromXML(item);
                        AddNode(listLayout[listLayout.Count - 1]);
                        break;
                    default:
                        break;
                }
            }
        }

        public override void AddUINodeToCanvas(CanvasManager canvasManager)
        {
            canvasManager.AddUINode(this);
        }

        public override void ResetUIProperty()
        {
            base.ResetUIProperty();
            back.ResetUIProperty();
            canvas.ResetUIProperty();
            foreach (var item in listLayout)
            {
                item.ResetUIProperty();
            }
        }
    }
}
