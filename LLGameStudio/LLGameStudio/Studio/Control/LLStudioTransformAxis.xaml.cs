using LLGameStudio.Common.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LLGameStudio.Studio.Control
{
    /// <summary>
    /// LLStudioTransformAxis.xaml 的交互逻辑
    /// </summary>
    public partial class LLStudioTransformAxis : UserControl
    {
        public delegate void DragEvent(object sender,Vector2 moveVector);

        public DragEvent DragAxisEvent;

        Point axisCenterPoint;
        Point lastMousePoint;

        bool isXAxisClicked = false;
        bool isYAxisClicked = false;
        bool isXYAxisClicked = false;
        Canvas canvas;

        public LLStudioTransformAxis(Canvas canvas)
        {
            InitializeComponent();
            this.canvas = canvas;
        }

        private void polygonXAxis_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            polygonXAxis.CaptureMouse();
            lastMousePoint = e.GetPosition(canvas);
            isXAxisClicked = true;
            polygonXAxis.Stroke = new SolidColorBrush(Colors.Blue);
        }

        private void polygonXAxis_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            polygonXAxis.ReleaseMouseCapture();
            isXAxisClicked = false;
            polygonXAxis.Stroke = null;
        }

        private void polygonXAxis_MouseMove(object sender, MouseEventArgs e)
        {
            if(isXAxisClicked)
            {
                Point currentMousePoint = e.GetPosition(canvas);
                if(currentMousePoint.X<=0)
                {
                    //需要判断，让坐标轴不要移动到父容器范围外。
                    //LLStudioBone和LLStudioTransformAxis的SetPosition应统一，并添加是否超过父容器的判断。
                    return;
                }
                Vector2 moveVector = new Vector2(currentMousePoint.X - lastMousePoint.X, 0);
                DragAxisEvent?.Invoke(this, moveVector);
                lastMousePoint = currentMousePoint;
                SetPosition(axisCenterPoint.X + moveVector.X, axisCenterPoint.Y);
            }
        }

        private void polygonYAxis_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            polygonYAxis.CaptureMouse();
            lastMousePoint = e.GetPosition(canvas);
            isYAxisClicked = true;
            polygonYAxis.Stroke = new SolidColorBrush(Colors.Blue);
        }

        private void polygonYAxis_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            polygonYAxis.ReleaseMouseCapture();
            isYAxisClicked = false;
            polygonYAxis.Stroke = null;
        }

        private void polygonYAxis_MouseMove(object sender, MouseEventArgs e)
        {
            if(isYAxisClicked)
            {
                Point currentMousePoint = e.GetPosition(canvas);
                if (currentMousePoint.Y <= 0)
                {
                    return;
                }
                Vector2 moveVector = new Vector2(0, currentMousePoint.Y - lastMousePoint.Y);
                DragAxisEvent?.Invoke(this, moveVector);
                lastMousePoint = currentMousePoint;
                SetPosition(axisCenterPoint.X, axisCenterPoint.Y+ moveVector.Y);
            }
        }

        private void rectangleAxis_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            rectangleAxis.CaptureMouse();
            lastMousePoint = e.GetPosition(canvas);
            isXYAxisClicked = true;
            rectangleAxis.Stroke = new SolidColorBrush(Colors.Blue);
        }

        private void rectangleAxis_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            rectangleAxis.ReleaseMouseCapture();
            isXYAxisClicked = false;
            rectangleAxis.Stroke = null;
        }

        private void rectangleAxis_MouseMove(object sender, MouseEventArgs e)
        {
            if(isXYAxisClicked)
            {
                Point currentMousePoint = e.GetPosition(canvas);
                if (currentMousePoint.Y <= 0 || currentMousePoint.Y <= 0)
                {
                    return;
                }
                Vector2 moveVector = new Vector2(currentMousePoint.X - lastMousePoint.X, currentMousePoint.Y - lastMousePoint.Y);
                DragAxisEvent?.Invoke(this, moveVector);
                lastMousePoint = currentMousePoint;
                SetPosition(axisCenterPoint.X+ moveVector.X, axisCenterPoint.Y + moveVector.Y);
            }
        }

        /// <summary>
        /// 设置坐标轴位置。
        /// </summary>
        public void SetPosition(double x,double y)
        {
            axisCenterPoint.X = x;
            axisCenterPoint.Y = y;
            Margin = new Thickness(x - 0.25 * ActualWidth, y - 0.75 * ActualHeight, 0, 0);
        }

        /// <summary>
        /// 设置坐标轴位置。
        /// </summary>
        public void SetPosition(Point point)
        {
            axisCenterPoint = point;
            Margin = new Thickness(point.X - 0.25 * ActualWidth, point.Y - 0.75 * ActualHeight, 0, 0);
        }
    }
}
