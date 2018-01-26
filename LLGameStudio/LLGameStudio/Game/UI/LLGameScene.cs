using LLGameStudio.Common;
using LLGameStudio.Common.XML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LLGameStudio.Game.UI
{
    class LLGameScene : IUINode
    {
        LLGameBack back;
        LLGameCanvas canvas;
        List<LLGameLayout> listLayout;

        string filePath;

        public string FilePath { get => filePath; set => filePath = value; }

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
            LoadBaseAttrbuteFromXML(element);

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
                        break;
                    case "LLGameCanvas":
                        canvas = new LLGameCanvas();
                        canvas.LoadContentFromXML(item);
                        break;
                    case "LLGameLayout":
                        listLayout.Add(new LLGameLayout());
                        listLayout[listLayout.Count - 1].LoadContentFromXML(item);
                        break;
                    default:
                        break;
                }
            }
        }

        public override void Render()
        {
            throw new NotImplementedException();
        }
    }
}
