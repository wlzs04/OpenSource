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
        public Property.FilePath filePath = new Property.FilePath();

        public LLGameImage()
        {
            AddProperty(filePath);
        }

        public override XElement ExportContentToXML()
        {
            throw new NotImplementedException();
        }

        public override void LoadContentFromXML(XElement element)
        {
            LoadAttrbuteFromXML(element);
        }

        public override void AddUINodeToCanvas(CanvasManager canvasManager)
        {
            //canvasManager.AddUINode(this);
        }

        public override void ResetUIProperty()
        {
            base.ResetUIProperty();
            gridContent.Background = new ImageBrush(new BitmapImage(new Uri(GameManager.GameResourcePath + @"\" + filePath.Value, UriKind.Relative)));
        }
    }
}
