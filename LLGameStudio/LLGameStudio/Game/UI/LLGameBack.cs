using LLGameStudio.Common.XML;
using LLGameStudio.Studio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace LLGameStudio.Game.UI
{
    class LLGameBack : IUINode
    {
        public Property.Image image = new Property.Image();

        public LLGameBack()
        {
            AddProperty(image);
        }

        public override XElement ExportContentToXML()
        {
            XElement element = new XElement("LLGameBack");
            ExportAttrbuteToXML(element);
            return element;
        }

        public override void LoadContentFromXML(XElement element)
        {
            LoadAttrbuteFromXML(element);
        }

        public override void ResetUIProperty()
        {
            base.ResetUIProperty();
            if(!image.IsDefault)
            {
                gridContent.Background = new ImageBrush(new BitmapImage(new Uri(GameManager.GameResourcePath + @"\" + image.Value, UriKind.Relative)));
            }
        }
    }
}
