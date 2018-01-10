﻿using LLGameStudio.Common.Config;
using LLGameStudio.Common.XML;
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

        private void InitControls()
        {
            comboBoxScaleCanvas.Items.Add(50);
            comboBoxScaleCanvas.Items.Add(100);
            comboBoxScaleCanvas.Items.Add(150);
            comboBoxScaleCanvas.Items.Add(200);
            comboBoxScaleCanvas.Items.Add(300);
        }

        private void InitManager()
        {
            studioManager = new StudioManager(this);
            imageMaximizeWindow.ToolTip = studioManager.FullScreen ? "还原" : "最大化";
            labelTitle.Content = Title;
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

        private void borderTitleArea_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            studioManager.MoveStudio();
            imageMaximizeWindow.ToolTip = "最大化";
        }

        public Canvas GetCanvas()
        {
            return canvas;
        }

        private void imageMinimizeWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            studioManager.MinimizeStudio();
            e.Handled = true;
        }

        private void imageMaximizeWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (studioManager.FullScreen)
            {
                studioManager.RestoreStudio();
                imageMaximizeWindow.ToolTip = "最大化";
            }
            else
            {
                studioManager.MaximizeStudio();
                imageMaximizeWindow.ToolTip = "还原";
            }
            e.Handled = true;
        }

        private void imageExitWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            studioManager.ExitStudio();
            e.Handled = true;
        }

        private void imageHelp_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            studioManager.ShowStudioHelpInfo();
        }

        private void imageStartGame_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            studioManager.StartGame();
        }

        private void imageCreateGame_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(studioManager.CreateGame())
            {
                LoadDirectoryToFileArea(studioManager.GameResourcePath);
            }
        }

        private void imageOpenGame_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (studioManager.OpenGame())
            {
                LoadDirectoryToFileArea(studioManager.GameResourcePath);
            }
        }

        private void imageStopGame_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            studioManager.StopGame();
        }

        public void ShowStatusInfo(string info)
        {
            labelStatusInfo.Content = info;
        }

        public void SetGameName(string gameName)
        {
            labelGameName.Content = gameName;
        }

        public void LoadDirectoryToFileArea(string path)
        {
            wrapPanelFileArea.Children.Clear();
            DirectoryInfo di = new DirectoryInfo(path);
            foreach (DirectoryInfo diitem in di.GetDirectories())
            {
                LLStudioFileItem fileItem = new LLStudioFileItem(wrapPanelFileArea, diitem.FullName);
                fileItem.MouseDoubleClick += OpenDirectory;
                wrapPanelFileArea.Children.Add(fileItem);
            }

            foreach (FileInfo fi in di.GetFiles())
            {
                LLStudioFileItem fileItem = new LLStudioFileItem(wrapPanelFileArea, fi.FullName);
                wrapPanelFileArea.Children.Add(fileItem);
            }
        }
        
        private void OpenDirectory(object sender, MouseButtonEventArgs e)
        {
            var v = (LLStudioFileItem)sender;
            studioManager.EnterNextDirectory(v.textBox.Text);
            LoadDirectoryToFileArea(studioManager.FileAreaDirectory);
        }
        
        private void imageReturnLastDirectory_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            studioManager.ReturnLastDirectory();
            LoadDirectoryToFileArea(studioManager.FileAreaDirectory);
        }

        private void canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            studioManager.ScaleCanvas(e.GetPosition(canvas), (1 + e.Delta / 3000.0));
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

        private void buttonRestoreCanvas_Click(object sender, RoutedEventArgs e)
        {
            studioManager.RestoreCanvas();
        }

        private void comboBoxScaleCanvas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            double rate=Convert.ToDouble(comboBoxScaleCanvas.SelectedItem)*0.01;
            studioManager.ScaleCanvas(rate);
        }
    }
}