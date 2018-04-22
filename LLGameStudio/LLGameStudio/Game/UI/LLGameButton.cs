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
    /// <summary>
    /// UI控件，显示按钮。
    /// </summary>
    class LLGameButton : IUINode
    {
        public Property.Image image = new Property.Image();

        public LLGameButton()
        {
            AddProperty(image);
        }

        public override XElement ExportContentToXML()
        {
            XElement element = new XElement("LLGameButton");
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
            gridContent.Background = new ImageBrush(new BitmapImage(new Uri(GameManager.GameResourcePath + @"\" + image.Value, UriKind.Relative)));
        }
    }
}
