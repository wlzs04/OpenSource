using LLGameStudio.Common.XML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LLGameStudio.Game.UI
{
    class LLGameBack : ILLGameUINode
    {

        public override XElement ExportContentToXML()
        {
            throw new NotImplementedException();
        }

        public override void LoadContentFromXML(XElement element)
        {
            LoadBaseAttrbuteFromXML(element);

            foreach (var item in element.Attributes())
            {

            }
        }

        public override void Render()
        {
            throw new NotImplementedException();
        }
    }
}
