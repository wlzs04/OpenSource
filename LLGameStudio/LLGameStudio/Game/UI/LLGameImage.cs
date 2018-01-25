using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LLGameStudio.Game.UI
{
    class LLGameImage : IUINode
    {
        string filePath = "";

        public string FilePath { get => filePath; set => filePath = value; }

        public override XElement ExportContentToXML()
        {
            throw new NotImplementedException();
        }

        public override void LoadContentFromXML(XElement element)
        {
            LoadBaseAttrbuteFromXML(element);

            XAttribute xAttribute = element.Attribute("filePath");
            if (xAttribute != null) { filePath = xAttribute.Value; xAttribute.Remove(); }

        }

        public override void Render()
        {
            throw new NotImplementedException();
        }
    }
}
