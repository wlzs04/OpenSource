using LLGameStudio.Common.Config;
using LLGameStudio.Common.XML;
using LLGameStudio.Game;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LLGameStudio.Studio
{
    class StudioManager
    {
        string studioConfigFilePath = @"Config\Studio.xml";
        StudioConfig studioConfig;
        MainWindow window;
        GameManager gameManager;

        public StudioManager(MainWindow window)
        {
            this.window = window;
            LoadConfig();
            gameManager = new GameManager(this);
        }

        public bool FullScreen { get => studioConfig.FullScreen;}

        public void LoadConfig()
        {
            ShowStatusInfo("正加载配置。");
            LLXMLConverter converter = new LLXMLConverter();
            studioConfig = new StudioConfig();
            converter.LoadContentFromXML(studioConfigFilePath, studioConfig);

            if (studioConfig.FullScreen)
            {
                window.Height = SystemParameters.WorkArea.Height;
                window.Width = SystemParameters.WorkArea.Width;
                window.Top = SystemParameters.WorkArea.Top;
                window.Left = SystemParameters.WorkArea.Left;
            }
            else
            {
                window.Top = studioConfig.Top;
                window.Left = studioConfig.Left;
                window.Width = studioConfig.Width;
                window.Height = studioConfig.Height;
            }
            window.Title = studioConfig.StudioName;
        }

        public void SaveConfig()
        {
            ShowStatusInfo("正保存配置。");
            LLXMLConverter converter = new LLXMLConverter();
            converter.ExportContentToXML(studioConfigFilePath, studioConfig);
        }

        public void ResizeStudio()
        {
            if (!studioConfig.FullScreen)
            {
                studioConfig.Height = (int)window.Height;
                studioConfig.Width = (int)window.Width;
                studioConfig.Top = (int)window.Top;
                studioConfig.Left = (int)window.Left;
            }
        }

        public void MoveStudio()
        {
            window.DragMove();
            studioConfig.Top = (int)window.Top;
            studioConfig.Left = (int)window.Left;
            studioConfig.FullScreen = false;
        }

        public void MaximizeStudio()
        {
            studioConfig.FullScreen = true;
            window.Height = SystemParameters.WorkArea.Height;
            window.Width = SystemParameters.WorkArea.Width;
            window.Top = SystemParameters.WorkArea.Top;
            window.Left = SystemParameters.WorkArea.Left;
        }

        public void RestoreStudio()
        {
            window.Height = studioConfig.Height;
            window.Width = studioConfig.Width;
            window.Top = studioConfig.Top;
            window.Left = studioConfig.Left;
            studioConfig.FullScreen = false;
        }

        public void MinimizeStudio()
        {
            window.WindowState = WindowState.Minimized;
        }

        public void ExitStudio()
        {
            SaveConfig();
            ShowStatusInfo(studioConfig.StudioName + "已退出。");
            Environment.Exit(0);
        }

        public void ShowStudioHelpInfo()
        {
            MessageBox.Show("当前版本：1.0.0。");
        }

        public void StartGame()
        {
            gameManager.StartGame();
        }

        public void StopGame()
        {
            gameManager.StopGame();
        }

        public void CreateGame()
        {
            CommonOpenFileDialog folderDialog = new CommonOpenFileDialog();
            folderDialog.IsFolderPicker = true;
            folderDialog.Title = "请选择游戏目录。";
            if (folderDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string gameName = "helloworld";
                string gamePath = folderDialog.FileName + @"\" + gameName;

                if (Directory.Exists(gamePath))
                {
                    MessageBox.Show("当前文件路径已存在！");
                    ShowStatusInfo("当前文件路径已存在！");
                    return;
                }
                else
                {
                    gameManager.CreateGame(gamePath,gameName);
                }
            }
        }

        public void OpenGame()
        {
            CommonOpenFileDialog folderDialog = new CommonOpenFileDialog();
            folderDialog.IsFolderPicker = true;
            folderDialog.Title = "请选择游戏目录。";
            if (folderDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                if(!gameManager.OpenGame(folderDialog.FileName))
                {
                    MessageBox.Show("此文件夹不是有效的游戏路径");
                    ShowStatusInfo("此文件夹不是有效的游戏路径");
                }
            }
        }

        public void ShowStatusInfo(string info)
        {
            window.ShowStatusInfo(info);
            LogStatusInfo(info);
        }

        public void LogStatusInfo(string info)
        {
            //未完成。
        }

        public IntPtr WndResizeProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (FullScreen)
            {
                return hwnd;
            }
            switch ((Window32MessageEnum)msg)
            {
                case Window32MessageEnum.WM_NCHITTEST:
                    Point point = new Point();
                    point.X = (lParam.ToInt32() & 0xFFFF) - window.Left;
                    point.Y = (lParam.ToInt32() >> 16) - window.Top;

                    // 窗口左上角
                    if (point.Y < studioConfig.BorderWidth
                       && point.X < studioConfig.BorderWidth)
                    {
                        handled = true;
                        return new IntPtr((int)Window32HandleEnum.HTTOPLEFT);
                    }
                    // 窗口左下角
                    else if (point.X < studioConfig.BorderWidth
                        && point.Y > window.Height - studioConfig.BorderWidth)
                    {
                        handled = true;
                        return new IntPtr((int)Window32HandleEnum.HTBOTTOMLEFT);
                    }
                    // 窗口右上角
                    else if (point.Y < studioConfig.BorderWidth
                       && point.X > window.Width - studioConfig.BorderWidth)
                    {
                        handled = true;
                        return new IntPtr((int)Window32HandleEnum.HTTOPRIGHT);
                    }
                    // 窗口右下角
                    else if (point.X > window.Width - studioConfig.BorderWidth
                       && point.Y > window.Height - studioConfig.BorderWidth)
                    {
                        handled = true;
                        return new IntPtr((int)Window32HandleEnum.HTBOTTOMRIGHT);
                    }
                    // 窗口左侧
                    else if (point.X < studioConfig.BorderWidth)
                    {
                        handled = true;
                        return new IntPtr((int)Window32HandleEnum.HTLEFT);
                    }
                    // 窗口右侧
                    else if (point.X > window.Width - studioConfig.BorderWidth)
                    {
                        handled = true;
                        return new IntPtr((int)Window32HandleEnum.HTRIGHT);
                    }
                    // 窗口上方
                    else if (point.Y < studioConfig.BorderWidth)
                    {
                        handled = true;
                        return new IntPtr((int)Window32HandleEnum.HTTOP);
                    }
                    // 窗口下方
                    else if (point.Y > window.Height - studioConfig.BorderWidth)
                    {
                        handled = true;
                        return new IntPtr((int)Window32HandleEnum.HTBOTTOM);
                    }
                    else
                    {
                        return hwnd;
                    }
            }
            return hwnd;
        }
    }
}
