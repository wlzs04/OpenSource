using LLGameStudio.Common;
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
    /// <summary>
    /// UI布局类
    /// </summary>
    class LLGameLayout : IUINode
    {
        public Property.FilePath filePath = new Property.FilePath();
        List<IUINode> listNode;

        public LLGameLayout()
        {
            listNode = new List<IUINode>();
        }

        public bool LoadContentFromFile(string path)
        {
            filePath.Value = path;
            LLConvert.LoadContentFromXML(path, this);
            return true;
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
                        listNode[listNode.Count - 1].LoadContentFromXML(item);
                        break;
                    default:
                        break;
                }
            }
        }

        public override void Render(CanvasManager canvasManager)
        {
            canvasManager.AddPath(path);
            foreach (var item in listNode)
            {
                item.Render(canvasManager);
            }
        }
    }
}
