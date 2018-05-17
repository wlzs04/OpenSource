using LLGameStudio.Studio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace LLGameStudio.Game.UI
{
    /// <summary>
    /// UI控件，显示图像。
    /// </summary>
    class LLGameImage : IUINode
    {
        public Property.Image image = new Property.Image();

        public LLGameImage()
        {
            AddProperty(image);
        }

        public override XElement ExportContentToXML()
        {
            XElement element = new XElement("LLGameImage");
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
            if(image.IsDefault)
            {
                gridContent.Background = null;
            }
            else
            {
                gridContent.Background = new ImageBrush(new BitmapImage(new Uri(GameManager.GameResourcePath + @"\" + image.Value, UriKind.Relative)));
            }
        }
    }
}
