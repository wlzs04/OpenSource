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
    /// <summary>
    /// LLStudioTimeline.xaml 的交互逻辑
    /// </summary>
    public partial class LLStudioTimeline : UserControl
    {
        double timeLimit = 24;//时间(刻度)上限。
        int scaleNumber = 4;//一级刻度数量。
        int secondScaleNumber = 6;//二级刻度数量。

        double startEndScaleHeight = 50;//起止刻度的高度。
        double scaleHeight = 40;//一级刻度的高度。
        double secondScaleHeight = 20; //二级刻度的高度。
        double timeBlockHeight = 30; //时间滑块的高度。
        double lineWidth = 2;//绘制线的宽度。
        double numberFontSize = 10;//数字字体大小。
        double timeBlockWidth = 10;
        double padLeftRight = 5;//起止刻度距离控件左右边缘间隔。
        double padTop = 20;//起止刻度距离控件上边缘间隔。

        bool isTimeBlockClicked = false;
        int currentTimeBlackScale = 0;//当前时间滑块在第几个刻度上。

        double moveLength = 0;//临时保存鼠标移动长度。

        Rectangle timeBlock;//时间滑块。
        Brush lineBrush = null;

        Point lastMousePoint;

        public delegate void DragEvent(int scale);
        public DragEvent DragTimeBlackEvent;

        public LLStudioTimeline()
        {
            InitializeComponent();
            timeBlock = new Rectangle();
            lineBrush = ThemeManager.GetBrushByName("timeLineScaleColor");
            timeBlock.MouseLeftButtonDown += timeBlock_MouseLeftButtonDown;
            timeBlock.MouseLeftButtonUp += timeBlock_MouseLeftButtonUp;
            timeBlock.MouseMove += timeBlock_MouseMove;
        }

        private void timeBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            timeBlock.CaptureMouse();
            lastMousePoint = e.GetPosition(canvas);
            isTimeBlockClicked = true;
            timeBlock.Stroke = ThemeManager.GetBrushByName("timeBlockSelectColor");
            moveLength = 0;
        }

        private void timeBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            timeBlock.ReleaseMouseCapture();
            isTimeBlockClicked = false;
            timeBlock.Stroke = null;
            moveLength = 0;
        }

        private void timeBlock_MouseMove(object sender, MouseEventArgs e)
        {
            if (isTimeBlockClicked)
            {
                Point currentMousePoint = e.GetPosition(canvas);
                currentMousePoint.X = LLMath.Clamp(currentMousePoint.X,0,canvas.ActualWidth);
                
                moveLength += currentMousePoint.X - lastMousePoint.X;
                
                if(Math.Abs(moveLength)>timeBlockWidth)
                {
                    int moveScale = (int)(moveLength / timeBlockWidth);
                    moveLength -= moveScale * timeBlockWidth;

                    SetTimeBlockToScale(currentTimeBlackScale + moveScale);
                }
                lastMousePoint = currentMousePoint;
            }
        }

        /// <summary>
        /// 将时间滑块设置第几个刻度上。
        /// </summary>
        public void SetTimeBlockToScale(int scale)
        {
            scale = (int)LLMath.Clamp(scale, 0, timeLimit);
            if (currentTimeBlackScale != scale)
            {
                currentTimeBlackScale = scale;
                timeBlock.Margin = new Thickness(currentTimeBlackScale * timeBlockWidth, timeBlock.Margin.Top, 0, 0);
                DragTimeBlackEvent?.Invoke(scale);
            }
        }

        public void SetTimeLimit(double timeLimit)
        {
            this.timeLimit = timeLimit;
            ResetTimeLine();
        }

        public double GetTimeLimit()
        {
            return timeLimit;
        }

        public void SetScaleNumber(int scaleNumber)
        {
            this.scaleNumber = scaleNumber;
            ResetTimeLine();
        }

        public int GetScaleNumber()
        {
            return scaleNumber;
        }

        public void SetSecondScaleNumber(int secondScaleNumber)
        {
            this.secondScaleNumber = secondScaleNumber;
            ResetTimeLine();
        }

        public int GetSecondScaleNumber()
        {
            return secondScaleNumber;
        }

        public void InitTimeLine()
        {
            padLeftRight = canvas.ActualWidth / (timeLimit + 1)/2;

            double realWidth = canvas.ActualWidth - 2 * padLeftRight;
            double realHeight = canvas.ActualHeight;

            Line startLine = new Line();
            startLine.Stroke = lineBrush;
            startLine.StrokeThickness = lineWidth;
            startLine.X1 = padLeftRight; startLine.Y1 = padTop;
            startLine.X2 = padLeftRight; startLine.Y2 = padTop + startEndScaleHeight;
            canvas.Children.Add(startLine);

            TextBlock textBlockStart = new TextBlock();
            textBlockStart.Text = "0";
            textBlockStart.FontSize = numberFontSize;
            textBlockStart.Foreground = lineBrush;
            textBlockStart.Margin = new Thickness(padLeftRight,0,0,0);
            canvas.Children.Add(textBlockStart);

            Line endLine = new Line();
            endLine.Stroke = lineBrush;
            endLine.StrokeThickness = lineWidth;
            endLine.X1 = padLeftRight + realWidth; endLine.Y1 = padTop;
            endLine.X2 = padLeftRight + realWidth; endLine.Y2 = padTop + startEndScaleHeight;
            canvas.Children.Add(endLine);

            TextBlock textBlockEnd = new TextBlock();
            textBlockEnd.Text = timeLimit+"";
            textBlockEnd.FontSize = numberFontSize;
            textBlockEnd.Foreground = lineBrush;
            textBlockEnd.Margin = new Thickness(padLeftRight + realWidth,0,0,0);
            canvas.Children.Add(textBlockEnd);

            Line baseLine = new Line();
            baseLine.Stroke = lineBrush;
            baseLine.StrokeThickness = lineWidth;
            baseLine.X1 = padLeftRight; baseLine.Y1 = padTop + startEndScaleHeight;
            baseLine.X2 = padLeftRight + realWidth; baseLine.Y2 = padTop + startEndScaleHeight;
            canvas.Children.Add(baseLine);

            timeBlock.StrokeThickness = 2;
            timeBlock.Fill = ThemeManager.GetBrushByName("timeBlockColor");
            timeBlock.Width = realWidth / timeLimit;
            timeBlock.Height = timeBlockHeight;
            timeBlock.Margin = new Thickness(startLine.X1- timeBlock.Width/2, padTop + startEndScaleHeight-timeBlockHeight, 0,0);
            canvas.Children.Add(timeBlock);

            timeBlockWidth = timeBlock.Width;
        }

        public void ResetTimeLine()
        {
            double realWidth = canvas.ActualWidth - 2 * padLeftRight;
            double realHeight = canvas.ActualHeight;

            int allScaleNumber = scaleNumber * secondScaleNumber;
            double scaleSpace = realWidth/ allScaleNumber;

            for (int i = 1; i < allScaleNumber; i++)
            {
                Line line = new Line();
                line.Stroke = lineBrush;
                line.StrokeThickness = lineWidth;
                if(i% scaleNumber==0)
                {
                    line.Y1 = padTop + startEndScaleHeight - scaleHeight;
                }
                else
                {
                    line.Y1 = padTop + startEndScaleHeight - secondScaleHeight;
                }
                line.X1 = padLeftRight + i * scaleSpace;
                line.X2 = line.X1;
                line.Y2 = padTop + startEndScaleHeight;
                canvas.Children.Add(line);

                TextBlock textBlock = new TextBlock();
                textBlock.Text = i + "";
                textBlock.FontSize = numberFontSize;
                textBlock.Foreground = lineBrush;
                textBlock.Margin = new Thickness(padLeftRight + i * scaleSpace, 0, 0, 0);
                canvas.Children.Add(textBlock);
            }
        }
    }
}
