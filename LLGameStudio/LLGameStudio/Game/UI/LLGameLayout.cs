using LLGameStudio.Common.XML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LLGameStudio.Game.UI
{
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
            LLXMLConverter.LoadContentFromXML(path, this);
            return true;
        }

        public override XElement ExportContentToXML()
        {
            throw new NotImplementedException();
        }

        public override void LoadContentFromXML(XElement element)
        {
            LoadBaseAttrbuteFromXML(element);

            XAttribute xAttribute = element.Attribute("filePath");
            if (xAttribute != null) { filePath.Value = xAttribute.Value; xAttribute.Remove(); }

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

        public override void Render()
        {
            throw new NotImplementedException();
        }
    }

    namespace Property
    {
        class FilePath : IUIProperty
        {
            public FilePath() : base("FilePath", typeof(String), UIPropertyEnum.Common, "当前节点文件路径。", "") { }
        }
    }
}
