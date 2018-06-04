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
                IUINode uINode = null;
                switch (item.Name.ToString())
                {
                    case "LLGameImage":
                        uINode = new LLGameImage();
                        break;
                    case "LLGameButton":
                        uINode = new LLGameButton();
                        break;
                    case "LLGameText":
                        uINode = new LLGameText();
                        break;
                    case "LLGameComboBox":
                        uINode = new LLGameComboBox();
                        break;
                    case "LLGameSlide":
                        uINode = new LLGameSlide();
                        break;
                    case "LLGameTextBox":
                        uINode = new LLGameTextBox();
                        break;
                    default:
                        break;
                }
                if(uINode!=null)
                {
                    AddNode(uINode);
                    uINode.LoadContentFromXML(item);
                }
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
