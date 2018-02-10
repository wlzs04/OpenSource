using LLGameStudio.Game;
using LLGameStudio.Game.UI;
using LLGameStudio.Studio.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LLGameStudio.Studio
{
    public class CanvasManager
    {
        Canvas canvas;
        Brush brush;
        double canvasShowRate = 1;
        Point rootPosition;
        bool isShowStandard = true;
        GameManager gameManager;
        List<IUINode> uiNodelist;
        Matrix transformMatrix = new Matrix();
        IUINode currentUINode;
        Point lastMousePosition;
        bool uiNodeStartMove = false;
        LLStudioSelectBorder llStudioSelectBorder;
        bool rootUINodeIsScene = false;

        public double CanvasShowRate { get => canvasShowRate; }

        public CanvasManager(Canvas canvas, GameManager gameManager)
        {
            this.canvas = canvas;
            this.gameManager = gameManager;
            brush = Brushes.White;
            rootPosition = new Point(canvas.ActualWidth / 10, canvas.ActualHeight / 10);
            uiNodelist = new List<IUINode>();

            llStudioSelectBorder = new LLStudioSelectBorder(this);
        }

        /// <summary>
        /// 在画布中对当前UI节点进行位置变换操作后，通过此方法同步属性编辑器相应属性。
        /// </summary>
        public void ReLoadTransformProperty()
        {
            gameManager.ReLoadTransformProperty();
        }

        /// <summary>
        /// 显示用来辅助节点移动的缩放的边框。
        /// </summary>
        public void ShowUINodeBorder()
        {
            llStudioSelectBorder.SetSelectUINode(currentUINode);
            canvas.Children.Remove(llStudioSelectBorder);
            canvas.Children.Add(llStudioSelectBorder);
            ResetUINodeBorderPositionAndSize();
        }

        /// <summary>
        /// 根据当前选中节点重新设置节点边框位置和大小。
        /// </summary>
        public void ResetUINodeBorderPositionAndSize()
        {
            canvas.UpdateLayout();
            Point point = currentUINode.TranslatePoint(new Point(0, 0), canvas);
            llStudioSelectBorder.SetRect(new Rect(point, new Size(currentUINode.actualWidth* canvasShowRate, currentUINode.actualHeight * canvasShowRate)));
        }

        /// <summary>
        /// 绘制标准网格线
        /// </summary>
        public void DrawStandardGrid()
        {
            SetBrushByColor(Color.FromArgb(125, 255, 255, 255));
            double spaceBetweenLine = 30 * canvasShowRate;
            double width = canvas.ActualWidth;
            double height = canvas.ActualHeight;
            Point startPosition = new Point(0, 0);
            if (rootPosition.X > 0)
            {
                startPosition.X = rootPosition.X % spaceBetweenLine;
            }
            else
            {
                startPosition.X = spaceBetweenLine - (rootPosition.X % spaceBetweenLine);
            }
            if (rootPosition.Y > 0)
            {
                startPosition.Y = rootPosition.Y % spaceBetweenLine;
            }
            else
            {
                startPosition.Y = spaceBetweenLine - (rootPosition.Y % spaceBetweenLine);
            }
            //绘制横线
            for (int i = 0; startPosition.Y + i * spaceBetweenLine < height; i++)
            {
                DrawLine(new Point(0, startPosition.Y + i * spaceBetweenLine), new Point(width, startPosition.Y + i * spaceBetweenLine), 0.2);
            }
            //绘制竖线
            for (int i = 0; startPosition.X + i * spaceBetweenLine < width; i++)
            {
                DrawLine(new Point(startPosition.X + i * spaceBetweenLine, 0), new Point(startPosition.X + i * spaceBetweenLine, height), 0.2);
            }

            //绘制窗体边缘线
            if (rootPosition.Y > 0)
            {
                DrawLine(new Point(0, rootPosition.Y), new Point(width, rootPosition.Y), 1);
            }
            if (rootPosition.X > 0)
            {
                DrawLine(new Point(rootPosition.X, 0), new Point(rootPosition.X, height), 1);
            }
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
                if (item != llStudioSelectBorder)
                {
                    item.RenderTransform = matrixTransform;
                }
            }
            if (currentUINode != null)
            {
                ResetUINodeBorderPositionAndSize();
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
            DrawRectangle(0, 0, GameManager.GameWidth,GameManager.GameHeight);
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
            transformMatrix = Matrix.Identity;
            currentUINode = null;
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
        public void DrawLine(Point startPoint, Point endPoint, double thickness = 1)
        {
            LineGeometry lineGeometry = new LineGeometry(startPoint, endPoint);

            Path path = new Path();
            path.Stroke = brush;
            path.StrokeThickness = thickness;
            path.Data = lineGeometry;

            AddPath(path);
        }

        /// <summary>
        /// 画矩形，给定起始点和终点坐标。
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <param name="thickness"></param>
        public void DrawRectangle(Point startPoint, Point endPoint, double thickness = 1)
        {
            RectangleGeometry rectangleGeometry = new RectangleGeometry(new Rect(startPoint, endPoint));
            Path path = new Path();
            path.Stroke = brush;
            path.StrokeThickness = thickness;
            path.Data = rectangleGeometry;
            AddPath(path);
        }

        /// <summary>
        /// 画矩形，给定起始点left、top值和矩形的宽高。
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="thickness"></param>
        public void DrawRectangle(double left, double top, double width, double height, double thickness = 1)
        {
            RectangleGeometry rectangleGeometry = new RectangleGeometry(new Rect(left, top, width, height));
            Path path = new Path();
            path.Stroke = brush;
            path.StrokeThickness = thickness;
            path.Data = rectangleGeometry;
            AddPath(path);
        }

        /// <summary>
        /// 添加形状（Path）到画布上。
        /// </summary>
        /// <param name="path"></param>
        public void AddPath(Path path)
        {
            canvas.Children.Add(path);
        }

        /// <summary>
        /// 添加UI节点
        /// </summary>
        /// <param name="path"></param>
        public void AddRootUINode(UIElement ui)
        {
            canvas.Children.Add(ui);
            rootUINodeIsScene = ui is LLGameScene;
            LoadAllUINodeFromRootUINode(gameManager.rootNode);
        }

        /// <summary>
        /// 从根节点中加载子节点，并为子节点添加鼠标点击等事件。
        /// </summary>
        /// <param name="rootUINode"></param>
        /// <param name="addEvent"></param>
        public void LoadAllUINodeFromRootUINode(IUINode rootUINode,bool addEvent=true)
        {
            foreach (var item in rootUINode.listNode)
            {
                uiNodelist.Add(item);
                if(addEvent)
                {
                    item.MouseLeftButtonDown += UINodeMouseLeftButtonDown;
                    item.MouseMove += UINodeMouseMove;
                    item.MouseLeftButtonUp += UINodeMouseLeftButtonUp;
                }
                LoadAllUINodeFromRootUINode(item, addEvent && !(item is LLGameLayout));
            }
        }

        /// <summary>
        /// UI节点的鼠标左键弹起事件，控制UI节点的移动。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UINodeMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            uiNodeStartMove = false;
            if(currentUINode!=null)
            {
                lastMousePosition = e.GetPosition(canvas);
                ResetUINodeBorderPositionAndSize();
                currentUINode.ReleaseMouseCapture();
                gameManager.ReLoadTransformProperty();
            }
            e.Handled = true;
        }

        /// <summary>
        /// UI节点的鼠标移动事件，控制UI节点的移动。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UINodeMouseMove(object sender, MouseEventArgs e)
        {
            if(uiNodeStartMove)
            {
                Point currentMousePosition = e.GetPosition(canvas);

                if (currentUINode != null)
                {
                    currentUINode.Move((currentMousePosition.X - lastMousePosition.X)/ canvasShowRate, (currentMousePosition.Y - lastMousePosition.Y) / canvasShowRate);
                }

                lastMousePosition = currentMousePosition;
                ResetUINodeBorderPositionAndSize();
            }
        }

        /// <summary>
        /// UI节点的鼠标左键点击事件，控制UI节点的选中状态。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UINodeMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (currentUINode != null)
            {
                currentUINode.CancelSelectState();
            }
            currentUINode = sender as IUINode;
            gameManager.SelectUINode(currentUINode);
            currentUINode.SetSelectState();
            lastMousePosition = e.GetPosition(canvas);
            uiNodeStartMove = true;
            currentUINode.CaptureMouse();
            ShowUINodeBorder();
            e.Handled = true;
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

        /// <summary>
        /// 通过UI节点的Name属性选择UI节点，此方法即将修改。
        /// </summary>
        /// <param name="uiNodeName"></param>
        public void SelectUINodeByName(string uiNodeName)
        {
            foreach (var item in uiNodelist)
            {
                if(item.name.Value== uiNodeName)
                {
                    gameManager.currentSelectUINode = item;
                    SelectUINode(item);
                }
            }
        }

        /// <summary>
        /// 控制UI节点的选中。
        /// </summary>
        /// <param name="uiNode"></param>
        public void SelectUINode(IUINode uiNode)
        {
            if (currentUINode != null)
            {
                if(currentUINode== uiNode)
                {
                    return;
                }
                currentUINode.CancelSelectState();
            }
            currentUINode = uiNode;
            uiNode.SetSelectState();
            ShowUINodeBorder();
        }
    }
}
