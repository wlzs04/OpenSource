using LLGameStudio.Common.Config;
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
using System.Windows.Forms;
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

namespace LLGameStudio
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        StudioManager studioManager;

        public MainWindow()
        {
            InitializeComponent();
            InitManager();
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
            studioManager.CreateGame();
        }

        private void imageOpenGame_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            studioManager.OpenGame();
        }

        private void imageStopGame_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            studioManager.StopGame();
        }

        public void ShowStatusInfo(string info)
        {
            labelStatusInfo.Content = info;
        }
    }
}