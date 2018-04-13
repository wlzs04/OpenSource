using LLGameStudio.Common.DataType;
using LLGameStudio.Common.Helper;
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
    public enum TransformType
    {
        Tranlation,//平移
        Rotation,//旋转
        Scale//缩放
    }

    /// <summary>
    /// LLStudioTransformAxis.xaml 的交互逻辑
    /// </summary>
    public partial class LLStudioTransformAxis : UserControl
    {
        public delegate void DragEvent(object sender,Vector2 moveVector);

        public DragEvent DragAxisEvent;

        TransformType transformType= TransformType.Tranlation;

        Point axisCenterPoint;
        Point lastMousePoint;

        bool isXAxisClicked = false;
        bool isYAxisClicked = false;
        bool isXYAxisClicked = false;

        bool isRAxisClicked = false;

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
                Vector2 moveVector = new Vector2(0, currentMousePoint.Y - lastMousePoint.Y);
                DragAxisEvent?.Invoke(this, moveVector);
                lastMousePoint = currentMousePoint;
                SetPosition(axisCenterPoint.X, axisCenterPoint.Y+ moveVector.Y);
            }
        }

        private void rectangleAxis_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            rectangleXYAxis.CaptureMouse();
            lastMousePoint = e.GetPosition(canvas);
            isXYAxisClicked = true;
            rectangleXYAxis.Stroke = new SolidColorBrush(Colors.Blue);
        }

        private void rectangleAxis_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            rectangleXYAxis.ReleaseMouseCapture();
            isXYAxisClicked = false;
            rectangleXYAxis.Stroke = null;
        }

        private void rectangleAxis_MouseMove(object sender, MouseEventArgs e)
        {
            if(isXYAxisClicked)
            {
                Point currentMousePoint = e.GetPosition(canvas);
                Vector2 moveVector = new Vector2(currentMousePoint.X - lastMousePoint.X, currentMousePoint.Y - lastMousePoint.Y);
                DragAxisEvent?.Invoke(this, moveVector);
                lastMousePoint = currentMousePoint;
                SetPosition(axisCenterPoint.X + moveVector.X, axisCenterPoint.Y + moveVector.Y);
            }
        }

        private void pathRAxis_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            pathRAxis.CaptureMouse();
            lastMousePoint = e.GetPosition(canvas);
            isRAxisClicked = true;
            pathRAxis.Stroke = new SolidColorBrush(Colors.Yellow); ;
        }

        private void pathRAxis_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            pathRAxis.ReleaseMouseCapture();
            isRAxisClicked = false;
            pathRAxis.Stroke = null;
        }

        private void pathRAxis_MouseMove(object sender, MouseEventArgs e)
        {
            if (isRAxisClicked)
            {
                Point currentMousePoint = e.GetPosition(canvas);

                Vector2 v1 = LLMath.GetVector2BetweenPoints(axisCenterPoint, lastMousePoint);
                Vector2 v2 = LLMath.GetVector2BetweenPoints(axisCenterPoint, currentMousePoint);
                v1=LLMath.GetNormalVector2(v1);
                v2 = LLMath.GetNormalVector2(v2);
                double angle = LLMath.GetAngleBetweenVectors(v1,v2);

                Vector2 moveVector = new Vector2(angle, angle);
                DragAxisEvent?.Invoke(this, moveVector);
                lastMousePoint = currentMousePoint;
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
            SetPosition(point.X, point.Y);
        }

        /// <summary>
        /// 获得坐标轴类型
        /// </summary>
        /// <returns></returns>
        public TransformType GetTransformType()
        {
            return transformType;
        }

        /// <summary>
        /// 设置坐标轴种类
        /// </summary>
        /// <param name="type"></param>
        public void SetTransformType(TransformType type)
        {
            ClearState();
            transformType = type;
            switch (transformType)
            {
                case TransformType.Tranlation:
                    gridTranslation.Visibility = Visibility.Visible;
                    gridRotation.Visibility = Visibility.Hidden;
                    gridScale.Visibility = Visibility.Hidden;
                    break;
                case TransformType.Rotation:
                    gridTranslation.Visibility = Visibility.Hidden;
                    gridRotation.Visibility = Visibility.Visible;
                    gridScale.Visibility = Visibility.Hidden;
                    break;
                case TransformType.Scale:
                    gridTranslation.Visibility = Visibility.Hidden;
                    gridRotation.Visibility = Visibility.Hidden;
                    gridScale.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 清除坐标轴中已保存的信息
        /// </summary>
        void ClearState()
        {
            gridTranslation.Visibility = Visibility.Hidden;
            gridRotation.Visibility = Visibility.Hidden;
            gridScale.Visibility = Visibility.Hidden;
            gridRotation.Visibility = Visibility.Hidden;

            polygonXAxis.ReleaseMouseCapture();
            polygonYAxis.ReleaseMouseCapture();
            rectangleXYAxis.ReleaseMouseCapture();
            pathRAxis.ReleaseMouseCapture();

            polygonXAxis.Stroke = null;
            polygonYAxis.Stroke = null;
            rectangleXYAxis.Stroke = null;
            pathRAxis.Stroke = null;

            isXAxisClicked = false;
            isYAxisClicked = false;
            isXYAxisClicked = false;
            isRAxisClicked = false;
        }
    }
}
