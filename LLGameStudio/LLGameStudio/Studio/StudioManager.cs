using LLGameStudio.Common.Config;
using LLGameStudio.Common.XML;
using LLGameStudio.Game;
using LLGameStudio.Studio.Control;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LLGameStudio.Studio
{
    class StudioManager
    {
        string studioConfigFilePath = @"Config\Studio.xml";
        string fileAreaDirectory="";
        StudioConfig studioConfig;
        MainWindow window;
        GameManager gameManager;
        CanvasManager canvasManager;

        public bool FullScreen { get => studioConfig.FullScreen;}
        public string GameResourcePath { get => gameManager.GameResourcePath; }
        public string FileAreaDirectory { get => fileAreaDirectory; }

        public StudioManager(MainWindow window)
        {
            this.window = window;
            LoadConfig();
            gameManager = new GameManager(this);
            ThemeManager.LoadTheme(studioConfig.Theme);
        }

        /// <summary>
        /// 从文件中加载编辑器配置。
        /// </summary>
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
            ShowStatusInfo("配置加载完成。");
        }

        /// <summary>
        /// 保存编辑器配置到文件中。
        /// </summary>
        public void SaveConfig()
        {
            ShowStatusInfo("正保存配置。");
            LLXMLConverter converter = new LLXMLConverter();
            converter.ExportContentToXML(studioConfigFilePath, studioConfig);
            ShowStatusInfo("配置保存完成。");
        }

        /// <summary>
        /// 初始化画布
        /// </summary>
        public void InitCanvas()
        {
            canvasManager = new CanvasManager(window.GetCanvas(), gameManager);
            DrawCanvas();
        }

        /// <summary>
        /// 移动画布基础位置
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void MoveCanvas(double x, double y)
        {
            canvasManager.MoveCanvas(x, y);
        }

        /// <summary>
        /// 缩放画布：对变换过的画布进行再变换
        /// </summary>
        /// <param name="centerPosition">中心位置</param>
        /// <param name="rate">缩放比例</param>
        public void ScaleCanvas(Point centerPosition,double rate)
        {
            canvasManager.ScaleCanvas(centerPosition, rate);
        }

        /// <summary>
        /// 缩放画布：直接缩放画布到指定比例
        /// </summary>
        /// <param name="rate"></param>
        public void ScaleCanvas(double rate)
        {
            canvasManager.ScaleCanvas(rate);
        }

        /// <summary>
        /// 清空画布内容
        /// </summary>
        public void ClearCanvas()
        {
            canvasManager.ClearAll();
        }

        /// <summary>
        /// 重新设置编辑器窗体大小。
        /// </summary>
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

        /// <summary>
        /// 移动编辑器窗体位置。
        /// </summary>
        public void MoveStudio()
        {
            window.DragMove();
            studioConfig.Top = (int)window.Top;
            studioConfig.Left = (int)window.Left;
            studioConfig.FullScreen = false;
        }

        /// <summary>
        /// 最大化编辑器。
        /// </summary>
        public void MaximizeStudio()
        {
            studioConfig.FullScreen = true;
            window.Height = SystemParameters.WorkArea.Height;
            window.Width = SystemParameters.WorkArea.Width;
            window.Top = SystemParameters.WorkArea.Top;
            window.Left = SystemParameters.WorkArea.Left;
        }

        /// <summary>
        /// 恢复编辑器大小。
        /// </summary>
        public void RestoreStudio()
        {
            window.Height = studioConfig.Height;
            window.Width = studioConfig.Width;
            window.Top = studioConfig.Top;
            window.Left = studioConfig.Left;
            studioConfig.FullScreen = false;
        }

        /// <summary>
        /// 最小化编辑器。
        /// </summary>
        public void MinimizeStudio()
        {
            window.WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// 退出编辑器。
        /// </summary>
        public void ExitStudio()
        {
            SaveConfig();
            ShowStatusInfo(studioConfig.StudioName + "已退出。");
            Environment.Exit(0);
        }

        /// <summary>
        /// 显示编辑器帮助信息。
        /// </summary>
        public void ShowStudioHelpInfo()
        {
            MessageBox.Show("当前版本：1.0.0。");
        }

        /// <summary>
        /// 开始游戏。
        /// </summary>
        public void StartGame()
        {
            SaveGame();
            gameManager.StartGame();
        }

        /// <summary>
        /// 保存游戏文件。
        /// </summary>
        public void SaveGame()
        {
            ShowStatusInfo("正保存游戏。");
            gameManager.SaveGame();
            ShowStatusInfo("游戏保存完成。");
        }

        /// <summary>
        /// 停止游戏。
        /// </summary>
        public void StopGame()
        {
            gameManager.StopGame();
        }

        /// <summary>
        /// 绘制画布内容
        /// </summary>
        public void DrawCanvas()
        {
            if(gameManager.GameLoaded)
            {
                canvasManager.ReDrawAll();
            }
        }

        /// <summary>
        /// 创建新游戏目录。
        /// </summary>
        /// <returns>返回在当前路径是否创建成功。</returns>
        public bool CreateGame()
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
                }
                else
                {
                    ShowStatusInfo("正新建游戏目录。");
                    gameManager.CreateGame(gamePath,gameName);
                    fileAreaDirectory = GameResourcePath;
                    ShowStatusInfo("游戏目录新建完成。");
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 恢复画布变换
        /// </summary>
        public void RestoreCanvas()
        {
            canvasManager.RestoreCanvas();
        }

        /// <summary>
        /// 打开游戏目录。
        /// </summary>
        /// <returns>是否成功打开</returns>
        public bool OpenGame()
        {
            CommonOpenFileDialog folderDialog = new CommonOpenFileDialog("请选择游戏目录。");
            folderDialog.IsFolderPicker = true;
            if (folderDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                ShowStatusInfo("正打开游戏目录。");
                if (!gameManager.OpenGame(folderDialog.FileName))
                {
                    MessageBox.Show("此文件夹不是有效的游戏路径");
                    ShowStatusInfo("此文件夹不是有效的游戏路径");
                }
                else
                {
                    fileAreaDirectory = GameResourcePath;
                    ShowStatusInfo("打开游戏目录完成。");
                    InitCanvas();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 进入到此文件夹下
        /// </summary>
        /// <param name="folderName"></param>
        public void EnterNextDirectory(string folderName)
        {
            fileAreaDirectory += @"\" + folderName;
        }

        /// <summary>
        /// 返回到上一目录。
        /// </summary>
        public void ReturnLastDirectory()
        {
            if(fileAreaDirectory==GameResourcePath)
            {
                window.ShowStatusInfo("当前已经是根目录。");
            }
            else
            {
                fileAreaDirectory = fileAreaDirectory.Substring(0, fileAreaDirectory.LastIndexOf('\\'));
            }
        }

        /// <summary>
        /// 显示编辑器状态信息。
        /// </summary>
        /// <param name="info">想要显示的文字。</param>
        public void ShowStatusInfo(string info)
        {
            window.ShowStatusInfo(info);
            LogStatusInfo(info);
        }

        /// <summary>
        /// 保存记录到log文件。
        /// </summary>
        /// <param name="info">想要记录的文字。</param>
        public void LogStatusInfo(string info)
        {
            //未完成。
        }

        /// <summary>
        /// 调用Win32窗体处理方法，使用前需要hook到主窗体上。
        /// 当前方法只用于帮助无边框wpf窗体调整边界大小。
        /// </summary>
        /// <param name="hwnd">窗体句柄</param>
        /// <param name="msg">触发的事件名称</param>
        /// <param name="wParam">参数</param>
        /// <param name="lParam">参数</param>
        /// <param name="handled">是否已经处理过</param>
        /// <returns></returns>
        public IntPtr WndResizeProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (FullScreen)
            {
                return hwnd;
            }
            switch ((Window32MessageEnum)msg)
            {
                case Window32MessageEnum.WM_NCHITTEST:
                    Point point = new Point((lParam.ToInt32() & 0xFFFF) - window.Left,
                        (lParam.ToInt32() >> 16) - window.Top);

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
