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
                        AddNode(new LLGameBack());
                        break;
                    case "LLGameCanvas":
                        AddNode(new LLGameCanvas());
                        break;
                    case "LLGameLayout":
                        AddNode(new LLGameLayout());
                        break;
                    default:
                        break;
                }
                listNode[listNode.Count - 1].LoadContentFromXML(item);
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
