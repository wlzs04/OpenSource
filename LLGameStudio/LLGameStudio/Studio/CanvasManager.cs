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
        float canvasShowRate = 2.5f;
        Point rootPosition;
        Point leftTopPostion;

        public CanvasManager(Canvas canvas)
        {
            this.canvas = canvas;
            brush = Brushes.White;
            rootPosition = new Point(0, 0);
        }

        public void DrawStandardGrid()
        {
            SetBrushColor(Brushes.Wheat);
            double width = canvas.ActualWidth;
            double height = canvas.ActualHeight;
            double spaceBetweenLine = 30 * canvasShowRate;
            int col = (int)Math.Ceiling(width / spaceBetweenLine);
            int row = (int)Math.Ceiling(height / spaceBetweenLine);
            for (int i = 0; i < row; i++)
            {
                DrawLine(new Point(0, i * spaceBetweenLine), new Point(width, i * spaceBetweenLine),0.2);
            }
            for (int i = 0; i < col; i++)
            {
                DrawLine(new Point(i * spaceBetweenLine, 0), new Point(i * spaceBetweenLine, height), 0.2);
            }
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
