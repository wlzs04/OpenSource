﻿using LLGameStudio.Common.XML;
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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;
using LLGameStudio.Studio;
using LLGameStudio.Game;
using LLGameStudio.Studio.Control;
using LLGameStudio.Studio.Window;

namespace LLGameStudio
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        StudioManager studioManager;
        Point lastMousePosition;
        bool canvasStartMove = false;

        public MainWindow()
        {
            InitializeComponent();
            InitManager();
            InitControls();
        }

        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControls()
        {
            studioManager.InitControls();

            //添加画布常用缩放比例
            comboBoxScaleCanvas.Items.Add("50%");
            comboBoxScaleCanvas.Items.Add("100%");
            comboBoxScaleCanvas.Items.Add("150%");
            comboBoxScaleCanvas.Items.Add("200%");
            comboBoxScaleCanvas.Items.Add("300%");

            comboBoxScaleCanvas.SelectedIndex = 1;

            comboBoxScaleCanvas.SelectionChanged += comboBoxScaleCanvas_SelectionChanged;
            
            buttonRestoreCanvas.ClickHandler += (object sender, 
                MouseButtonEventArgs e) => { RestoreCanvas(); };
            
            imageMaximizeWindow.ToolTip = studioManager.FullScreen ? "还原" : "最大化";
            labelTitle.Content = Title;

            //imageMinimizeWindow.SetImage(@"\Resource\最小化.png");
            imageMinimizeWindow.ClickHandler += (object sender, MouseButtonEventArgs e) =>
            {
                studioManager.MinimizeStudio();
                e.Handled = true;
            };
            //imageMaximizeWindow.SetImage(@"\Resource\最大化.png");
            imageMaximizeWindow.ClickHandler += (object sender, MouseButtonEventArgs e) =>
            {
                if (studioManager.FullScreen)
                {
                    studioManager.RestoreStudio();
                    imageMaximizeWindow.ToolTip = "最大化";
                    imageMaximizeWindow.SetImage(@"\Resource\最大化.png");
                }
                else
                {
                    studioManager.MaximizeStudio();
                    imageMaximizeWindow.ToolTip = "还原";
                    imageMaximizeWindow.SetImage(@"\Resource\还原.png");
                }
                e.Handled = true;
            };
            //imageExitWindow.SetImage(@"\Resource\退出.png");
            imageExitWindow.ClickHandler += (object sender, MouseButtonEventArgs e) =>
            {
                studioManager.ExitStudio();
                e.Handled = true;
            };
        }

        /// <summary>
        /// 初始化管理编辑器的类
        /// </summary>
        private void InitManager()
        {
            StudioManager.InitStudioManager(this);
            studioManager = StudioManager.GetSingleInstance();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            HwndSource hwndSource = PresentationSource.FromVisual(this) as HwndSource;
            hwndSource.AddHook(new HwndSourceHook(studioManager.WndResizeProc));
        }

        private void window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            studioManager.ResizeStudio();
            borderBackground.Width = Width;
            borderBackground.Height = Height;
        }
        
        /// <summary>
        /// 获得窗体画布对象
        /// </summary>
        /// <returns></returns>
        public Canvas GetCanvas()
        {
            return canvas;
        }

        /// <summary>
        /// 获得窗体菜单容器对象
        /// </summary>
        /// <returns></returns>
        public WrapPanel GetWrapPanelMenuArea()
        {
            return wrapPanelMenuArea;
        }

        /// <summary>
        /// 获得窗体游戏控制菜单容器对象
        /// </summary>
        /// <returns></returns>
        public Grid GetGridGameControlArea()
        {
            return gridGameControlArea;
        }

        /// <summary>
        /// 获得窗体文件容器对象
        /// </summary>
        /// <returns></returns>
        public WrapPanel GetWrapPanelFileArea()
        {
            return wrapPanelFileArea;
        }

        /// <summary>
        /// 获得窗体用来显示状态对象
        /// </summary>
        /// <returns></returns>
        public Label GetLabelStatusInfo()
        {
            return labelStatusInfo;
        }

        /// <summary>
        /// 获得窗体UI层级容器对象
        /// </summary>
        /// <returns></returns>
        public Grid GetGridUILayer()
        {
            return gridUILayer;
        }
        
        /// <summary>
        /// 获得窗体UI控件容器对象
        /// </summary>
        /// <returns></returns>
        public WrapPanel GetWrapPanelUIControlArea()
        {
            return wrapPanelUIControlArea;
        }

        /// <summary>
        /// 获得窗体属性编辑容器对象
        /// </summary>
        /// <returns></returns>
        public Grid GetGridPropertyEditorArea()
        {
            return gridPropertyEditorArea;
        }
        
        private void imageHelp_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            studioManager.ShowStudioHelpInfo();
            FontEditWindow fontEditWindow = new FontEditWindow();
            fontEditWindow.Init();
            fontEditWindow.Show();
        }
        
        /// <summary>
        /// 设置游戏名称
        /// </summary>
        /// <param name="gameName"></param>
        public void SetGameName(string gameName)
        {
            labelGameName.Content = gameName;
        }

        /// <summary>
        /// 返回上目录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imageReturnLastDirectory_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            studioManager.ReturnLastDirectory();
        }

        /// <summary>
        /// 在资源管理器中打开文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imageShowDirectoryInSystem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            studioManager.ShowDirectoryInSystem();
        }

        private void canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            double rate=studioManager.ScaleCanvas(e.GetPosition(canvas), (1 + e.Delta / 3000.0));
            comboBoxScaleCanvas.Text = (int)(rate*100) + "%";
        }

        private void canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.MiddleButton==MouseButtonState.Pressed)
            {
                canvasStartMove = true;
                lastMousePosition = e.GetPosition(canvas);
                canvas.CaptureMouse();
            }
        }

        private void canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.MiddleButton == MouseButtonState.Released)
            {
                canvasStartMove = false;
                lastMousePosition = e.GetPosition(canvas);
                canvas.ReleaseMouseCapture();
            }
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if(canvasStartMove)
            {
                Point currentMousePosition = e.GetPosition(canvas);
                studioManager.MoveCanvas(currentMousePosition.X - lastMousePosition.X, currentMousePosition.Y - lastMousePosition.Y);
                lastMousePosition = currentMousePosition;
            }
        }

        /// <summary>
        /// 恢复画布缩放比例和移动。
        /// </summary>
        public void RestoreCanvas()
        {
            studioManager.RestoreCanvas();
            comboBoxScaleCanvas.SelectionChanged -= comboBoxScaleCanvas_SelectionChanged;
            comboBoxScaleCanvas.SelectedIndex = 1;
            comboBoxScaleCanvas.SelectionChanged += comboBoxScaleCanvas_SelectionChanged;
        }

        private void comboBoxScaleCanvas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(comboBoxScaleCanvas.SelectedItem!=null)
            {
                string rateString = comboBoxScaleCanvas.SelectedItem as string;
                double rate = Convert.ToDouble(rateString.Remove(rateString.Length - 1)) * 0.01;
                studioManager.ScaleCanvas(rate);
            }
        }

        /// <summary>
        /// 移动窗体。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void borderTitleArea_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                studioManager.MoveStudio();
                imageMaximizeWindow.ToolTip = "最大化";
            }
        }

        public void SetCurrentDirectory(string fileAreaDirectory)
        {
            labelCurrentPath.Content = fileAreaDirectory;
        }
    }
}