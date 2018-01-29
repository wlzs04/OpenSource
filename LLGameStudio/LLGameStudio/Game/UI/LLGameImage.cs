using LLGameStudio.Studio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        public void ResetPath()
        {
            RectangleGeometry rectangleGeometry = new RectangleGeometry(
                new Rect(posX,posY,50, 50));
            
            path.Stroke = ThemeManager.GetBrushByName("borderUIColor");
            path.StrokeThickness = 1;
            path.Fill = new ImageBrush(new BitmapImage(new Uri(filePath.Value, UriKind.Relative)));
            
            path.Data = rectangleGeometry;
        }

        public override void Render(CanvasManager canvasManager)
        {
            ResetPath();
            canvasManager.AddPath(path);
        }
    }
}
