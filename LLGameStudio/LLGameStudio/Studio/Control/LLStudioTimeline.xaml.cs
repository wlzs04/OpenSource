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
        double minMoveUnit = 1;//时间轴内最小移动单位。

        double startEndScaleHeight = 50;//起止刻度的高度。
        double scaleHeight = 40;//一级刻度的高度。
        double secondScaleHeight = 20; //二级刻度的高度。
        double lineWidth = 2;//绘制线的宽度。
        double numberFontSize = 10;//数字字体大小。
        double padLeftRight = 5;//起止刻度距离控件左右边缘间隔。
        double padTop = 20;//起止刻度距离控件上边缘间隔。

        Brush lineBrush = null;

        public LLStudioTimeline()
        {
            InitializeComponent();
            lineBrush= ThemeManager.GetBrushByName("timeLineScaleColor");
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
            double realWidth = ActualWidth - 2 * padLeftRight;
            double realHeight = ActualHeight;

            Line startLine = new Line();
            startLine.Stroke = lineBrush;
            startLine.StrokeThickness = lineWidth;
            startLine.X1 = padLeftRight; startLine.Y1 = padTop;
            startLine.X2 = padLeftRight; startLine.Y2 = padTop + startEndScaleHeight;
            canvas.Children.Add(startLine);

            TextBlock textBlockStart = new TextBlock();
            textBlockStart.Text = "0";
            textBlockStart.Margin = new Thickness(0);
            canvas.Children.Add(textBlockStart);

            Line endLine = new Line();
            endLine.Stroke = lineBrush;
            endLine.StrokeThickness = lineWidth;
            endLine.X1 = padLeftRight + realWidth; endLine.Y1 = padTop;
            endLine.X2 = padLeftRight + realWidth; endLine.Y2 = padTop + startEndScaleHeight;
            canvas.Children.Add(endLine);

            TextBlock textBlockEnd = new TextBlock();
            textBlockEnd.Text = timeLimit+"";
            textBlockEnd.Margin = new Thickness(padLeftRight + realWidth,0,0,0);
            canvas.Children.Add(textBlockEnd);

            Line baseLine = new Line();
            baseLine.Stroke = lineBrush;
            baseLine.StrokeThickness = lineWidth;
            baseLine.X1 = padLeftRight; baseLine.Y1 = padTop + startEndScaleHeight;
            baseLine.X2 = padLeftRight + realWidth; baseLine.Y2 = padTop + startEndScaleHeight;
            canvas.Children.Add(baseLine);
        }

        public void ResetTimeLine()
        {
            double realWidth = ActualWidth - 2 * padLeftRight;
            double realHeight = ActualHeight;

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
                textBlock.Margin = new Thickness(padLeftRight + i * scaleSpace, 0, 0, 0);
                canvas.Children.Add(textBlock);
            }
        }
    }
}
