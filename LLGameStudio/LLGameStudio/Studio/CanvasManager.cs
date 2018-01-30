using LLGameStudio.Game;
using LLGameStudio.Game.UI;
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
        double canvasShowRate = 1;
        Point rootPosition;
        Point leftTopPostion;
        bool isShowStandard = true;
        GameManager gameManager;
        List<IUINode> uiNodelist;
        Matrix transformMatrix = new Matrix();

        public CanvasManager(Canvas canvas, GameManager gameManager)
        {
            this.canvas = canvas;
            this.gameManager = gameManager;
            brush = Brushes.White;
            leftTopPostion = new Point(0, 0);
            rootPosition = new Point(canvas.ActualWidth / 10, canvas.ActualHeight / 10);
            uiNodelist = new List<IUINode>();
        }

        /// <summary>
        /// 绘制标准网格线
        /// </summary>
        public void DrawStandardGrid()
        {
            //             SetBrushByColor(Color.FromArgb(125,255,255,255));
            //             double spaceBetweenLine = 30 * canvasShowRate;
            //             double width = canvas.ActualWidth;
            //             double height = canvas.ActualHeight;
            //             Point startPosition = new Point(0, 0);
            //             if (rootPosition.X>0)
            //             {
            //                 startPosition.X = rootPosition.X % spaceBetweenLine;
            //             }
            //             else
            //             {
            //                 startPosition.X = spaceBetweenLine-(rootPosition.X % spaceBetweenLine);
            //             }
            //             if (rootPosition.Y > 0)
            //             {
            //                 startPosition.Y = rootPosition.Y % spaceBetweenLine;
            //             }
            //             else
            //             {
            //                 startPosition.Y = spaceBetweenLine - (rootPosition.Y % spaceBetweenLine);
            //             }
            //             //绘制横线
            //             for (int i = 0; startPosition.Y+i*spaceBetweenLine<height; i++)
            //             {
            //                 DrawLine(new Point(0, startPosition.Y + i * spaceBetweenLine), new Point(width, startPosition.Y + i * spaceBetweenLine), 0.2);
            //             }
            //             //绘制竖线
            //             for (int i = 0; startPosition.X + i * spaceBetweenLine < width; i++)
            //             {
            //                 DrawLine(new Point(startPosition.X + i * spaceBetweenLine, 0), new Point(startPosition.X + i * spaceBetweenLine, height), 0.2);
            //             }
            // 
            //             //绘制窗体边缘线
            //             if (rootPosition.Y > 0)
            //             {
            //                 DrawLine(new Point(0, rootPosition.Y), new Point(width, rootPosition.Y), 1);
            //             }
            //             if (rootPosition.X > 0)
            //             {
            //                 DrawLine(new Point(rootPosition.X, 0), new Point(rootPosition.X, height), 1);
            //             }
        }

        /// <summary>
        /// 移动画布基础位置
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void MoveCanvas(double x, double y)
        {
            transformMatrix.OffsetX += x;
            transformMatrix.OffsetY += y;
            ResetCanvasTransform();
        }

        /// <summary>
        /// 缩放画布：对变换过的画布进行再变换，返回当前缩放比例
        /// </summary>
        /// <param name="centerPosition">中心位置</param>
        /// <param name="rate">缩放比例</param>
        public double ScaleCanvas(Point centerPosition, double rate)
        {
            ScaleTransform scaleTransform = new ScaleTransform();
            scaleTransform.CenterX = centerPosition.X;
            scaleTransform.CenterY = centerPosition.Y;
            scaleTransform.ScaleX = rate;
            scaleTransform.ScaleY = rate;
            canvasShowRate *= rate;
            transformMatrix.Append(scaleTransform.Value);
            ResetCanvasTransform();
            return canvasShowRate;
        }

        /// <summary>
        /// 缩放画布：直接缩放画布到指定比例
        /// </summary>
        /// <param name="rate"></param>
        public void ScaleCanvas(double rate)
        {
            ScaleTransform scaleTransform = new ScaleTransform();
            scaleTransform.CenterX = canvas.ActualWidth / 2;
            scaleTransform.CenterY = canvas.ActualHeight / 2;
            scaleTransform.ScaleX = 1/ canvasShowRate*rate;
            scaleTransform.ScaleY = 1 / canvasShowRate * rate;
            canvasShowRate = rate;
            transformMatrix.Append(scaleTransform.Value);
            ResetCanvasTransform();
        }

        /// <summary>
        /// 恢复画布变换
        /// </summary>
        public void RestoreCanvas()
        {
            transformMatrix = Matrix.Identity;
            canvasShowRate = 1;
            ResetCanvasTransform();
        }

        /// <summary>
        /// 重新设置画布变换
        /// </summary>
        public void ResetCanvasTransform()
        {
            MatrixTransform matrixTransform=new MatrixTransform(transformMatrix);
            foreach (UIElement item in canvas.Children)
            {
                item.RenderTransform = matrixTransform;
            }
        }

        /// <summary>
        /// 绘制所有内容
        /// </summary>
        public void DrawAll()
        {
            DrawBackground();
            DrawGame();
        }

        /// <summary>
        /// 绘制游戏内容
        /// </summary>
        public void DrawGame()
        {
            DrawRectangle(0, 0, gameManager.GameWidth,gameManager.GameHeight);
        }

        /// <summary>
        /// 重新绘制所有内容
        /// </summary>
        public void ReDrawAll()
        {
            ClearAll();
            DrawAll();
        }

        /// <summary>
        /// 清空画布内容
        /// </summary>
        public void ClearAll()
        {
            canvas.Children.Clear();
        }

        /// <summary>
        /// 设置画刷
        /// </summary>
        public void SetBrush(Brush brush)
        {
            this.brush = brush;
        }

        /// <summary>
        /// 通过颜色设置画刷
        /// </summary>
        /// <param name="color"></param>
        public void SetBrushByColor(Color color)
        {
            brush = new SolidColorBrush(color);
        }

        /// <summary>
        /// 画线段
        /// </summary>
        /// <param name="startPoint">起点</param>
        /// <param name="endPoint">终点</param>
        /// <param name="thickness">线宽度，默认值是1</param>
        public void DrawLine(Point startPoint,Point endPoint,double thickness=1)
        {
            LineGeometry lineGeometry = new LineGeometry(startPoint,endPoint);

            Path path = new Path();
            path.Stroke = brush;
            path.StrokeThickness = thickness;
            path.Data = lineGeometry;

            AddUINode(path);
        }

        public void DrawRectangle(Point startPoint, Point endPoint, double thickness = 1)
        {
            RectangleGeometry rectangleGeometry = new RectangleGeometry(new Rect(startPoint, endPoint));
            Path path = new Path();
            path.Stroke = brush;
            path.StrokeThickness = thickness;
            path.Data = rectangleGeometry;
            AddUINode(path);
        }

        public void DrawRectangle(double left, double top, double width, double height, double thickness = 1)
        {
            RectangleGeometry rectangleGeometry = new RectangleGeometry(new Rect(left, top, width, height));
            Path path = new Path();
            path.Stroke = brush;
            path.StrokeThickness = thickness;
            path.Data = rectangleGeometry;
            AddUINode(path);
        }
        
        /// <summary>
        /// 添加UI节点
        /// </summary>
        /// <param name="path"></param>
        public void AddUINode(UIElement ui)
        {
            canvas.Children.Add(ui);
        }

        /// <summary>
        /// 画背景
        /// </summary>
        public void DrawBackground()
        {
            if(isShowStandard)
            {
                DrawStandardGrid();
            }
        }
    }
}
