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
        bool canScale = false;
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
            Margin = new Thickness(rect.Left-2.5, rect.Top-2.5, 0, 0);
            Width = rect.Width+5;
            Height = rect.Height + 5;
        }

        public void SetSelectUINode(IUINode uiNode)
        {
            this.uiNode = uiNode;
        }
        
        private void grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            canScale = true;
            lastMousePosition = e.GetPosition((Canvas)Parent);
            ((Grid)sender).CaptureMouse();
            e.Handled = true;
        }

        private void gridLeft_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Point currentMousePosition = e.GetPosition((Canvas)Parent);
            double dtemp = currentMousePosition.X - lastMousePosition.X;
            uiNode.ChangeLeft(dtemp/ canvasManager.CanvasShowRate);
            canvasManager.ResetUINodeBorderPosition();
            lastMousePosition = currentMousePosition;
            ((Grid)sender).ReleaseMouseCapture();
            canScale = false;
            e.Handled = true;
        }

        private void gridLeft_MouseMove(object sender, MouseEventArgs e)
        {
            if(canScale)
            {
                Point currentMousePosition = e.GetPosition((Canvas)Parent);
                double dtemp = currentMousePosition.X - lastMousePosition.X;
                uiNode.ChangeLeft(dtemp / canvasManager.CanvasShowRate);
                canvasManager.ResetUINodeBorderPosition();
                lastMousePosition = currentMousePosition;
            }
        }

        private void gridTop_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            canScale = false;
            Point currentMousePosition = e.GetPosition((Canvas)Parent);
            double dtemp = currentMousePosition.Y - lastMousePosition.Y;
            uiNode.ChangeTop(dtemp / canvasManager.CanvasShowRate);
            canvasManager.ResetUINodeBorderPosition();
            lastMousePosition = currentMousePosition;
            ((Grid)sender).ReleaseMouseCapture();
            e.Handled = true;
        }

        private void gridTop_MouseMove(object sender, MouseEventArgs e)
        {
            if (canScale)
            {
                Point currentMousePosition = e.GetPosition((Canvas)Parent);
                double dtemp = currentMousePosition.Y - lastMousePosition.Y;
                uiNode.ChangeTop(dtemp / canvasManager.CanvasShowRate);
                canvasManager.ResetUINodeBorderPosition();
                lastMousePosition = currentMousePosition;
                
            }
        }

        private void gridRight_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            canScale = false;
            Point currentMousePosition = e.GetPosition((Canvas)Parent);
            double dtemp = currentMousePosition.X - lastMousePosition.X;
            uiNode.ChangeRight(dtemp / canvasManager.CanvasShowRate);
            canvasManager.ResetUINodeBorderPosition();
            lastMousePosition = currentMousePosition;
            ((Grid)sender).ReleaseMouseCapture();
            e.Handled = true;
        }

        private void gridRight_MouseMove(object sender, MouseEventArgs e)
        {
            if (canScale)
            {
                Point currentMousePosition = e.GetPosition((Canvas)Parent);
                double dtemp = currentMousePosition.X - lastMousePosition.X;
                uiNode.ChangeRight(dtemp / canvasManager.CanvasShowRate);
                canvasManager.ResetUINodeBorderPosition();
                lastMousePosition = currentMousePosition;
            }
        }

        private void gridBottom_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            canScale = false;
            Point currentMousePosition = e.GetPosition((Canvas)Parent);
            double dtemp = currentMousePosition.Y - lastMousePosition.Y;
            uiNode.ChangeBottom(dtemp / canvasManager.CanvasShowRate);
            canvasManager.ResetUINodeBorderPosition();
            lastMousePosition = currentMousePosition;
            ((Grid)sender).ReleaseMouseCapture();
            e.Handled = true;
        }

        private void gridBottom_MouseMove(object sender, MouseEventArgs e)
        {
            if (canScale)
            {
                Point currentMousePosition = e.GetPosition((Canvas)Parent);
                double dtemp = currentMousePosition.Y - lastMousePosition.Y;
                uiNode.ChangeBottom(dtemp / canvasManager.CanvasShowRate);
                canvasManager.ResetUINodeBorderPosition();
                lastMousePosition = currentMousePosition;
            }
        }

        private void gridLeftTop_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Point currentMousePosition = e.GetPosition((Canvas)Parent);
            double dtempX = currentMousePosition.X - lastMousePosition.X;
            double dtempY = currentMousePosition.Y - lastMousePosition.Y;
            uiNode.ChangeLeft(dtempX / canvasManager.CanvasShowRate);
            uiNode.ChangeTop(dtempY / canvasManager.CanvasShowRate);
            canvasManager.ResetUINodeBorderPosition();
            lastMousePosition = currentMousePosition;
            ((Grid)sender).ReleaseMouseCapture();
            canScale = false;
            e.Handled = true;
        }

        private void gridLeftTop_MouseMove(object sender, MouseEventArgs e)
        {
            if (canScale)
            {
                Point currentMousePosition = e.GetPosition((Canvas)Parent);
                double dtempX = currentMousePosition.X - lastMousePosition.X;
                double dtempY = currentMousePosition.Y - lastMousePosition.Y;
                uiNode.ChangeLeft(dtempX / canvasManager.CanvasShowRate);
                uiNode.ChangeTop(dtempY / canvasManager.CanvasShowRate);
                canvasManager.ResetUINodeBorderPosition();
                lastMousePosition = currentMousePosition;
            }
        }

        private void gridRightTop_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Point currentMousePosition = e.GetPosition((Canvas)Parent);
            double dtempX = currentMousePosition.X - lastMousePosition.X;
            double dtempY = currentMousePosition.Y - lastMousePosition.Y;
            uiNode.ChangeRight(dtempX / canvasManager.CanvasShowRate);
            uiNode.ChangeTop(dtempY / canvasManager.CanvasShowRate);
            canvasManager.ResetUINodeBorderPosition();
            lastMousePosition = currentMousePosition;
            ((Grid)sender).ReleaseMouseCapture();
            canScale = false;
            e.Handled = true;
        }

        private void gridRightTop_MouseMove(object sender, MouseEventArgs e)
        {
            if (canScale)
            {
                Point currentMousePosition = e.GetPosition((Canvas)Parent);
                double dtempX = currentMousePosition.X - lastMousePosition.X;
                double dtempY = currentMousePosition.Y - lastMousePosition.Y;
                uiNode.ChangeRight(dtempX / canvasManager.CanvasShowRate);
                uiNode.ChangeTop(dtempY / canvasManager.CanvasShowRate);
                canvasManager.ResetUINodeBorderPosition();
                lastMousePosition = currentMousePosition;
            }
        }

        private void gridLeftBottom_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Point currentMousePosition = e.GetPosition((Canvas)Parent);
            double dtempX = currentMousePosition.X - lastMousePosition.X;
            double dtempY = currentMousePosition.Y - lastMousePosition.Y;
            uiNode.ChangeLeft(dtempX / canvasManager.CanvasShowRate);
            uiNode.ChangeBottom(dtempY / canvasManager.CanvasShowRate);
            canvasManager.ResetUINodeBorderPosition();
            lastMousePosition = currentMousePosition;
            ((Grid)sender).ReleaseMouseCapture();
            canScale = false;
            e.Handled = true;
        }

        private void gridLeftBottom_MouseMove(object sender, MouseEventArgs e)
        {
            if (canScale)
            {
                Point currentMousePosition = e.GetPosition((Canvas)Parent);
                double dtempX = currentMousePosition.X - lastMousePosition.X;
                double dtempY = currentMousePosition.Y - lastMousePosition.Y;
                uiNode.ChangeLeft(dtempX / canvasManager.CanvasShowRate);
                uiNode.ChangeBottom(dtempY / canvasManager.CanvasShowRate);
                canvasManager.ResetUINodeBorderPosition();
                lastMousePosition = currentMousePosition;
            }
        }

        private void gridRightBottom_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Point currentMousePosition = e.GetPosition((Canvas)Parent);
            double dtempX = currentMousePosition.X - lastMousePosition.X;
            double dtempY = currentMousePosition.Y - lastMousePosition.Y;
            uiNode.ChangeRight(dtempX / canvasManager.CanvasShowRate);
            uiNode.ChangeBottom(dtempY / canvasManager.CanvasShowRate);
            canvasManager.ResetUINodeBorderPosition();
            lastMousePosition = currentMousePosition;
            ((Grid)sender).ReleaseMouseCapture();
            canScale = false;
            e.Handled = true;
        }

        private void gridRightBottom_MouseMove(object sender, MouseEventArgs e)
        {
            if (canScale)
            {
                Point currentMousePosition = e.GetPosition((Canvas)Parent);
                double dtempX = currentMousePosition.X - lastMousePosition.X;
                double dtempY = currentMousePosition.Y - lastMousePosition.Y;
                uiNode.ChangeRight(dtempX / canvasManager.CanvasShowRate);
                uiNode.ChangeBottom(dtempY / canvasManager.CanvasShowRate);
                canvasManager.ResetUINodeBorderPosition();
                lastMousePosition = currentMousePosition;
            }
        }
    }
}
