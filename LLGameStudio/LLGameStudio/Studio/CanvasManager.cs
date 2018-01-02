using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LLGameStudio.Studio
{
    class CanvasManager
    {
        Canvas canvas;
        Brush brush;

        public CanvasManager(Canvas canvas)
        {
            this.canvas = canvas;
            brush = Brushes.White;
        }

        public void DrawStandardGrid()
        {
            SetBrushColor(Brushes.Wheat);
            DrawLine(new Point(0,0),new Point(canvas.ActualWidth,canvas.ActualHeight));
        }

        public void ClearAll()
        {
            canvas.Children.Clear();
        }

        public void SetBrushColor(Brush brush)
        {
            this.brush = brush;
        }

        public void DrawLine(Point startPoint,Point endPoint,double thickness=1)
        {
            LineGeometry lineGeometry = new LineGeometry(startPoint,endPoint);

            Path path = new Path();
            path.Stroke = brush;
            path.StrokeThickness = thickness;
            path.Data = lineGeometry;

            canvas.Children.Add(path);
        }

        public void DrawPath(Path path)
        {
            canvas.Children.Add(path);
        }
    }
}
