using LLGameStudio.Common.DataType;
using LLGameStudio.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
    enum TimeState
    {
        Start,//开始
        Pause,//暂停
        Stop//停止
    }

    /// <summary>
    /// LLStudioTimeline.xaml 的交互逻辑
    /// </summary>
    public partial class LLStudioTimeline : UserControl
    {
        double timeLimit = 4;//时间上限（秒）。
        int scaleLimit = 24;//刻度上限。
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
        double scaleSpace = 10;//刻度之间的间隔。

        bool isTimeBlockClicked = false;
        int currentTimeBlackScale = 0;//当前时间滑块在第几个刻度上。
        double currentTime = 0;//当前时间轴代表的时间（秒）。
        double moveLength = 0;//临时保存鼠标移动长度。

        Rectangle timeBlock;//时间滑块。
        Line timeBlockLine;//时间滑块下方的线。
        Brush lineBrush = null;

        Point lastMousePoint;
        TimeState timeState = TimeState.Stop;

        public delegate void DragEvent(int scale);
        public DragEvent DragTimeBlackEvent;

        public delegate void KeyEvent(int scale);
        public KeyEvent KeyFrameEvent;

        public delegate void TimeEvent(double time);
        public TimeEvent TimeRunEvent;

        Timer timer = new Timer(41);//每秒24帧
        DateTime lastDataTime;

        public List<LLStudioKeyItem> listKeyItem=new List<LLStudioKeyItem>();

        public LLStudioTimeline()
        {
            InitializeComponent();
            timeBlock = new Rectangle();
            lineBrush = ThemeManager.GetBrushByName("timeLineScaleColor");
            timeBlock.MouseLeftButtonDown += timeBlock_MouseLeftButtonDown;
            timeBlock.MouseLeftButtonUp += timeBlock_MouseLeftButtonUp;
            timeBlock.MouseMove += timeBlock_MouseMove;

            timer.Elapsed += MoveTimeBlockByTimer;
        }

        private void timeBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (timeState == TimeState.Stop)
            {
                timeBlock.CaptureMouse();
                lastMousePoint = e.GetPosition(canvas);
                isTimeBlockClicked = true;
                timeBlock.Stroke = ThemeManager.GetBrushByName("timeBlockSelectColor");
                moveLength = 0;
            }
        }

        private void timeBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (timeState == TimeState.Stop)
            {
                timeBlock.ReleaseMouseCapture();
                isTimeBlockClicked = false;
                timeBlock.Stroke = null;
                moveLength = 0;
            }
        }

        private void timeBlock_MouseMove(object sender, MouseEventArgs e)
        {
            if (timeState == TimeState.Stop && isTimeBlockClicked)
            {
                Point currentMousePoint = e.GetPosition(canvas);
                currentMousePoint.X = LLMath.Clamp(currentMousePoint.X, 0, canvas.ActualWidth);

                moveLength += currentMousePoint.X - lastMousePoint.X;

                if (Math.Abs(moveLength) > timeBlockWidth)
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
            scale = (int)LLMath.Clamp(scale, 0, scaleLimit);
            if (currentTimeBlackScale != scale)
            {
                currentTimeBlackScale = scale;
                timeBlock.Margin = new Thickness(currentTimeBlackScale * timeBlockWidth, timeBlock.Margin.Top, 0, 0);
                DragTimeBlackEvent?.Invoke(scale);
            }
        }

        public void SetScaleLimit(int scaleLimit)
        {
            this.scaleLimit = scaleLimit;
            secondScaleNumber = scaleLimit / scaleNumber;

        }

        public double GetScaleLimit()
        {
            return scaleLimit;
        }

        public void SetScaleNumber(int scaleNumber)
        {
            this.scaleNumber = scaleNumber;
            secondScaleNumber = scaleLimit / scaleNumber;
        }

        public int GetScaleNumber()
        {
            return scaleNumber;
        }

        public int GetSecondScaleNumber()
        {
            return secondScaleNumber;
        }

        public void SetTimeLimit(double timeLimit)
        {
            this.timeLimit = timeLimit;
        }

        public double GetTimeLimit()
        {
            return timeLimit;
        }

        public double GetScaleSpace()
        {
            return scaleSpace;
        }

        /// <summary>
        /// 重新设置时间轴
        /// </summary>
        public void ResetTimeLine()
        {
            canvas.Children.Clear();

            padLeftRight = canvas.ActualWidth / (scaleLimit + 1) / 2;

            double realWidth = canvas.ActualWidth - 2 * padLeftRight;
            double realHeight = canvas.ActualHeight;

            scaleSpace = realWidth / scaleLimit;

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
            textBlockStart.Margin = new Thickness(padLeftRight, 0, 0, 0);
            canvas.Children.Add(textBlockStart);

            Line endLine = new Line();
            endLine.Stroke = lineBrush;
            endLine.StrokeThickness = lineWidth;
            endLine.X1 = padLeftRight + realWidth; endLine.Y1 = padTop;
            endLine.X2 = padLeftRight + realWidth; endLine.Y2 = padTop + startEndScaleHeight;
            canvas.Children.Add(endLine);

            TextBlock textBlockEnd = new TextBlock();
            textBlockEnd.Text = scaleLimit + "";
            textBlockEnd.FontSize = numberFontSize;
            textBlockEnd.Foreground = lineBrush;
            textBlockEnd.Margin = new Thickness(padLeftRight + realWidth, 0, 0, 0);
            canvas.Children.Add(textBlockEnd);

            Line baseLine = new Line();
            baseLine.Stroke = lineBrush;
            baseLine.StrokeThickness = lineWidth;
            baseLine.X1 = padLeftRight; baseLine.Y1 = padTop + startEndScaleHeight;
            baseLine.X2 = padLeftRight + realWidth; baseLine.Y2 = padTop + startEndScaleHeight;
            canvas.Children.Add(baseLine);
            
            timeBlock.StrokeThickness = 2;
            timeBlock.Fill = ThemeManager.GetBrushByName("timeBlockColor");
            timeBlock.Width = scaleSpace;
            timeBlock.Height = timeBlockHeight;
            timeBlock.Margin = new Thickness(startLine.X1 - timeBlock.Width / 2, padTop + startEndScaleHeight - timeBlockHeight, 0, 0);
            canvas.Children.Add(timeBlock);
            Panel.SetZIndex(timeBlock, 1);

            timeBlockLine = new Line();
            timeBlockLine.Stroke = lineBrush;
            timeBlockLine.StrokeThickness = lineWidth;
            timeBlockLine.X1 = startLine.X1 - timeBlock.Width / 2; timeBlockLine.Y1 = padTop + startEndScaleHeight;
            timeBlockLine.X2 = startLine.X1 - timeBlock.Width / 2; timeBlockLine.Y2 = padTop + startEndScaleHeight;
            canvas.Children.Add(timeBlockLine); 

            timeBlockWidth = timeBlock.Width;

            int allScaleNumber = scaleNumber * secondScaleNumber;
            
            for (int i = 1; i < allScaleNumber; i++)
            {
                Line line = new Line();
                line.Stroke = lineBrush;
                line.StrokeThickness = lineWidth;
                if(i% secondScaleNumber == 0)
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

            SetTimeBlockToScale(0);
        }

        /// <summary>
        /// 获得某一刻度下的X坐标
        /// </summary>
        /// <param name="scale"></param>
        public double GetScalePositionX(int scale)
        {
            return padLeftRight + scale * scaleSpace;
        }
        
        private void imageKey_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Key();
        }

        private void imageStartOrPause_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(timeState!=TimeState.Start)
            {
                Start();
            }
            else
            {
                Pause();
            }
        }

        private void imageStop_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(timeState!= TimeState.Stop)
            {
                Stop();
            }
        }

        /// <summary>
        /// Key帧
        /// </summary>
        public void Key()
        {
            foreach (var item in listKeyItem)
            {
                if(!item.HaveKeyFlag(currentTimeBlackScale))
                {
                    item.AddKeyFlag(currentTimeBlackScale);
                }
            }
            KeyFrameEvent?.Invoke(currentTimeBlackScale);
        }

        public void Start()
        {
            imageStartOrPause.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Resource\暂停.png"));
            imageStartOrPause.ToolTip = "暂停";
            timeState = TimeState.Start;
            timer.Enabled = true;
            lastDataTime = DateTime.Now;
        }

        public void Pause()
        {
            imageStartOrPause.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Resource\开始.png"));
            imageStartOrPause.ToolTip = "继续";
            timeState = TimeState.Pause;
            timer.Enabled = false;
        }

        public void Stop()
        {
            timeState = TimeState.Stop;
            imageStartOrPause.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Resource\开始.png"));
            imageStartOrPause.ToolTip = "开始";
            timer.Enabled = false;
            SetTimeBlockToScale(0);
            currentTime = 0;
        }

        void MoveTimeBlockByTimer(object sender, ElapsedEventArgs e)
        {
            TimeSpan timeSpan = e.SignalTime - lastDataTime;
            currentTime += timeSpan.TotalMilliseconds / 1000;
            if (currentTime > timeLimit)
            {
                currentTime = 0;
            }
            TimeRunEvent?.Invoke(currentTime);
            Dispatcher.Invoke(new Action(() => { SetTimeBlockToScale((int)(scaleLimit * (currentTime / timeLimit))); }));
            lastDataTime = e.SignalTime;
        }

        public void RemoveAllKeyItem()
        {
            stackPanelKeyItems.Children.Clear();
            listKeyItem.Clear();
        }

        public void AddKeyItem(LLStudioKeyItem keyItem)
        {
            stackPanelKeyItems.Children.Add(keyItem);
            listKeyItem.Add(keyItem);
        }

        /// <summary>
        /// 移除所有KeyItem上所有的标记
        /// </summary>
        public void RemoveAllKeyItemFlag()
        {
            foreach (var item in listKeyItem)
            {
                item.RemoveAllFlag();
            }
        }

        public LLStudioKeyItem GetKeyItemByBoneName(string name)
        {
            return listKeyItem.Where(keyItem => keyItem.GetName() == name).First();
        }

        /// <summary>
        /// 获得KeyItem的数量
        /// </summary>
        /// <returns></returns>
        public int GetKeyItemNumber()
        {
            return listKeyItem.Count;
        }
    }
}
