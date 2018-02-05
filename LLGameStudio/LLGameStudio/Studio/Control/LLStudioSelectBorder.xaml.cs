using LLGameStudio.Common.Helper;
using LLGameStudio.Game.UI;
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
    /// LLStudioSelectBorder.xaml 的交互逻辑
    /// </summary>
    public partial class LLStudioSelectBorder : UserControl
    {
        IUINode uiNode;
        bool scaleLeft = false;
        bool scaleTop = false;
        bool scaleRight = false;
        bool scaleBottom = false;
        Point lastMousePosition;
        CanvasManager canvasManager;

        public LLStudioSelectBorder(CanvasManager canvasManager)
        {
            InitializeComponent();
            this.canvasManager = canvasManager;
            border.BorderBrush = ThemeManager.GetBrushByName("borderUISelectColor");
            gridLeft.Background = ThemeManager.GetBrushByName("backgroundSelectBorderColor");
            gridTop.Background = ThemeManager.GetBrushByName("backgroundSelectBorderColor");
            gridRight.Background = ThemeManager.GetBrushByName("backgroundSelectBorderColor");
            gridBottom.Background = ThemeManager.GetBrushByName("backgroundSelectBorderColor");
        }

        public void SetRect(Rect rect)
        {
            Margin = new Thickness(rect.Left, rect.Top, 0, 0);
            Width = rect.Width;
            Height = rect.Height;
        }

        public void SetSelectUINode(IUINode uiNode)
        {
            this.uiNode = uiNode;
        }

        private void ChangeLeft(double d)
        {
            if ((uiNode.anchorEnum.Value & GameUIAnchorEnum.Left) != 0)
            {
                uiNode.Move(d, 0);
                uiNode.SetWidth(uiNode.width.Value-d);
            }
            else if ((uiNode.anchorEnum.Value & GameUIAnchorEnum.Right) != 0)
            {
                uiNode.SetWidth(uiNode.width.Value - d);
            }
            else
            {
                uiNode.Move(d, 0);
                uiNode.SetWidth(uiNode.width.Value - 2 * d);
            }
            canvasManager.ResetUINodeBorderPosition();
        }

        private void ChangeRight(double d)
        {
            if ((uiNode.anchorEnum.Value & GameUIAnchorEnum.Left) != 0)
            {
                uiNode.SetWidth(uiNode.width.Value + d);
            }
            else if ((uiNode.anchorEnum.Value & GameUIAnchorEnum.Right) != 0)
            {
                uiNode.margin.Value.Right -= d;
                if (LLMath.IsRange1To0(uiNode.margin.Value.Right))
                {
                    uiNode.margin.Value.Right = LLMath.One;
                }
                
                uiNode.actualMargin.Right -= d;
                uiNode.SetWidth(uiNode.width.Value + d);
            }
            else
            {
                uiNode.Move(-d, 0);
                uiNode.SetWidth(uiNode.width.Value + 2 * d);
            }
            canvasManager.ResetUINodeBorderPosition();
        }

        private void ChangeTop(double d)
        {
            if ((uiNode.anchorEnum.Value & GameUIAnchorEnum.Top) != 0)
            {
                uiNode.Move(0, d);
                uiNode.SetHeight(uiNode.height.Value-d);
            }
            else if ((uiNode.anchorEnum.Value & GameUIAnchorEnum.Bottom) != 0)
            {
                uiNode.SetHeight(uiNode.height.Value - d);
            }
            else
            {
                uiNode.Move(0, d);
                uiNode.SetHeight(uiNode.height.Value - 2 * d);
            }
            canvasManager.ResetUINodeBorderPosition();
        }

        private void ChangeBottom(double d)
        {
            if ((uiNode.anchorEnum.Value & GameUIAnchorEnum.Top) != 0)
            {
                uiNode.SetHeight(uiNode.height.Value + d);
            }
            else if ((uiNode.anchorEnum.Value & GameUIAnchorEnum.Bottom) != 0)
            {
                uiNode.Move(0, d);
                uiNode.SetHeight(uiNode.height.Value + d);
            }
            else
            {
                uiNode.Move(0, d);
                uiNode.SetHeight(uiNode.height.Value + 2 * d);
            }
            canvasManager.ResetUINodeBorderPosition();
        }

        private void gridLeft_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            scaleLeft = true;
            lastMousePosition = e.GetPosition((Canvas)Parent);
            ((Grid)sender).CaptureMouse();
            e.Handled = true;
        }

        private void gridLeft_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Point currentMousePosition = e.GetPosition((Canvas)Parent);
            ChangeLeft(currentMousePosition.X - lastMousePosition.X);
            lastMousePosition = currentMousePosition;
            ((Grid)sender).ReleaseMouseCapture();
            scaleLeft = false;
            e.Handled = true;
        }

        private void gridLeft_MouseMove(object sender, MouseEventArgs e)
        {
            if(scaleLeft)
            {
                Point currentMousePosition = e.GetPosition((Canvas)Parent);
                ChangeLeft(currentMousePosition.X - lastMousePosition.X);
                lastMousePosition = currentMousePosition;
            }
        }

        private void gridTop_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            scaleTop = true;
            lastMousePosition = e.GetPosition((Canvas)Parent);
            ((Grid)sender).CaptureMouse();
            e.Handled = true;
        }

        private void gridTop_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            scaleTop = false;
            Point currentMousePosition = e.GetPosition((Canvas)Parent);
            ChangeTop(currentMousePosition.Y- lastMousePosition.Y);
            lastMousePosition = currentMousePosition;
            ((Grid)sender).ReleaseMouseCapture();
            e.Handled = true;
        }

        private void gridTop_MouseMove(object sender, MouseEventArgs e)
        {
            if (scaleTop)
            {
                Point currentMousePosition = e.GetPosition((Canvas)Parent);
                ChangeTop(currentMousePosition.Y - lastMousePosition.Y);
                lastMousePosition = currentMousePosition;
                
            }
        }

        private void gridRight_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            scaleRight = true;
            lastMousePosition = e.GetPosition((Canvas)Parent);
            ((Grid)sender).CaptureMouse();
            e.Handled = true;
        }

        private void gridRight_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            scaleRight = false;
            Point currentMousePosition = e.GetPosition((Canvas)Parent);
            ChangeRight(currentMousePosition.X - lastMousePosition.X);
            lastMousePosition = currentMousePosition;
            ((Grid)sender).ReleaseMouseCapture();
            e.Handled = true;
        }

        private void gridRight_MouseMove(object sender, MouseEventArgs e)
        {
            if (scaleRight)
            {
                Point currentMousePosition = e.GetPosition((Canvas)Parent);
                ChangeRight(currentMousePosition.X - lastMousePosition.X);
                lastMousePosition = currentMousePosition;
            }
        }

        private void gridBottom_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            scaleBottom = true;
            lastMousePosition = e.GetPosition((Canvas)Parent);
            ((Grid)sender).CaptureMouse();
            e.Handled = true;
        }

        private void gridBottom_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            scaleBottom = false;
            Point currentMousePosition = e.GetPosition((Canvas)Parent);
            ChangeBottom(currentMousePosition.Y - lastMousePosition.Y);
            lastMousePosition = currentMousePosition;
            ((Grid)sender).ReleaseMouseCapture();
            e.Handled = true;
        }

        private void gridBottom_MouseMove(object sender, MouseEventArgs e)
        {
            if (scaleBottom)
            {
                Point currentMousePosition = e.GetPosition((Canvas)Parent);
                ChangeBottom(currentMousePosition.Y - lastMousePosition.Y);
                lastMousePosition = currentMousePosition;
            }
        }
    }
}
