using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LLGameStudio.Studio.Window
{
    /// <summary>
    /// NewGameWindow.xaml 的交互逻辑
    /// </summary>
    public partial class NewGameWindow : System.Windows.Window
    {
        bool logicSelected = false;
        bool normalSelected = false;
        string gameDirectory = "";
        string gameName = "";
        StudioManager studioManager = null;

        public NewGameWindow(StudioManager studioManager)
        {
            InitializeComponent();
            this.studioManager = studioManager;
        }

        private void gridTitle_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void imageMinimizeWindow_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void imageExitWindow_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            Close();
        }

        private void borderLogic_MouseEnter(object sender, MouseEventArgs e)
        {
            if(!logicSelected)
            {
                borderLogic.BorderBrush = ThemeManager.GetBrushByName("borderSelectColor");
                borderLogic.Background = ThemeManager.GetBrushByName("backgroundHoverColor");
            }
        }

        private void borderLogic_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!logicSelected)
            {
                borderLogic.BorderBrush = null;
                borderLogic.Background = ThemeManager.GetBrushByName("backgroundAlphaColor");
            }
        }

        private void borderLogic_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (normalSelected)
            {
                borderNormal.BorderBrush = null;
                borderNormal.Background = ThemeManager.GetBrushByName("backgroundAlphaColor");
            }
            borderLogic.BorderBrush = ThemeManager.GetBrushByName("borderSelectColor");
            borderLogic.Background = ThemeManager.GetBrushByName("backgroundHoverColor");
            logicSelected = true;
            normalSelected = false;
        }

        private void borderNormal_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!normalSelected)
            {
                borderNormal.BorderBrush = ThemeManager.GetBrushByName("borderSelectColor");
                borderNormal.Background = ThemeManager.GetBrushByName("backgroundHoverColor");
            }
        }

        private void borderNormal_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!normalSelected)
            {
                borderNormal.BorderBrush = null;
                borderNormal.Background = ThemeManager.GetBrushByName("backgroundAlphaColor");
            }
        }

        private void borderNormal_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (logicSelected)
            {
                borderLogic.BorderBrush = null;
                borderLogic.Background = ThemeManager.GetBrushByName("backgroundAlphaColor");
            }
            borderNormal.BorderBrush = ThemeManager.GetBrushByName("borderSelectColor");
            borderNormal.Background = ThemeManager.GetBrushByName("backgroundHoverColor");
            normalSelected = true;
            logicSelected = false;
        }

        private void imageChooseDirectory_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CommonOpenFileDialog folderDialog = new CommonOpenFileDialog();

            folderDialog.IsFolderPicker = true;
            folderDialog.Title = "请选择游戏目录。";
            if (folderDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                gameDirectory = folderDialog.FileName;
                textBoxGameDirectory.Text = gameDirectory;
            }
        }

        private void imageCreateGame_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            gameName = textBoxGameName.Text;
            if (logicSelected==normalSelected)
            {
                MessageBox.Show("请先选择游戏类型！");
                return;
            }
            if (gameDirectory == "")
            {
                MessageBox.Show("请先选择游戏目录！");
                return;
            }
            if (gameName == "")
            {
                MessageBox.Show("请先设置游戏名称！");
                return;
            }
            if (Directory.Exists(gameDirectory +@"\"+gameName))
            {
                MessageBox.Show("游戏路径已存在！");
                return;
            }
            studioManager.CreateGame(gameDirectory + @"\" + gameName, gameName);
            Close();
        }
    }
}
