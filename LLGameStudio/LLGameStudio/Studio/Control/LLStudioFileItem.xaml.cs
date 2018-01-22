using System;
using System.Collections.Generic;
using System.IO;
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
    /// FileItem.xaml 的交互逻辑
    /// </summary>
    public partial class LLStudioFileItem : UserControl
    {
        bool isSelect = false;
        FileInfo fileInfo;
        GameUIFileEnum gameUIFileEnum;
        WrapPanel parentPanel;

        public LLStudioFileItem(WrapPanel wp, string path)
        {
            InitializeComponent();

            fileInfo = new FileInfo(path);
            Uri uri;
            if(fileInfo.Attributes==FileAttributes.Directory)
            {
                uri = new Uri(Environment.CurrentDirectory + @"\Resource\文件夹.png");
                gameUIFileEnum = GameUIFileEnum.Folder;
            }
            else
            {
                switch (fileInfo.Extension)
                {
                    case ".png":
                        uri = new Uri(path);
                        break;
                    case ".scene":
                        gameUIFileEnum = GameUIFileEnum.Scene;
                        uri = new Uri(Environment.CurrentDirectory + @"\Resource\未知文件.png");
                        break;
                    case ".layout":
                        gameUIFileEnum = GameUIFileEnum.Layout;
                        uri = new Uri(Environment.CurrentDirectory + @"\Resource\未知文件.png");
                        break;
                    default:
                        gameUIFileEnum = GameUIFileEnum.Unknown;
                        uri = new Uri(Environment.CurrentDirectory + @"\Resource\未知文件.png");
                        break;
                }
            }
            image.Source = new BitmapImage(uri);
            textBox.Text = fileInfo.Name;
            MouseLeftButtonDown += ChangeAllFileItemSelectState;

            KeyDown += FileItemKeyDown;

            textBox.IsReadOnly = true;
            parentPanel = wp;
            ContextMenu = new ContextMenu();
            MenuItem mi0 = new MenuItem();
            mi0.Header = "重命名";
            mi0.Click += menuItem0_Click;
            ContextMenu.Items.Add(mi0);
            MenuItem mi1 = new MenuItem();
            mi1.Header = "删除";
            mi1.Click += menuItem1_Click;
            ContextMenu.Items.Add(mi1);

            border.Background = ThemeManager.GetBrushByName("backgroundAlphaColor");
        }

        /// <summary>
        /// 改变所有文件的选择状态。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeAllFileItemSelectState(object sender, MouseButtonEventArgs e)
        {
            var v = (LLStudioFileItem)sender;
            Keyboard.Focus(v);
            if(Keyboard.GetKeyStates(Key.LeftCtrl)!=KeyStates.Down)
            {
                foreach (var item in parentPanel.Children)
                {
                    if (item != v)
                    {
                        ((LLStudioFileItem)item).CancelSelectState();
                    }
                }
            }
        }

        private void FileItemKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                DeleteFile();
            }
        }

        /// <summary>
        /// 取消选中状态。
        /// </summary>
        public void CancelSelectState()
        {
            isSelect = false;
            border.BorderBrush = null;
            border.Background = ThemeManager.GetBrushByName("backgroundAlphaColor");
        }

        /// <summary>
        /// 删除文件。
        /// </summary>
        public void DeleteFile()
        {
            if(MessageBox.Show("确定要删除文件"+fileInfo.Name+"吗?", "删除文件", MessageBoxButton.OKCancel)==MessageBoxResult.OK)
            {
                parentPanel.Children.Remove(this);
                fileInfo.Delete();
            }
        }

        private void menuItem0_Click(object sender, RoutedEventArgs e)
        {
            if(textBox.IsReadOnly)
            {
                textBox.IsReadOnly = false;
                textBox.BorderBrush = new SolidColorBrush(Colors.DeepSkyBlue);
            }
        }

        public void menuItem1_Click(object sender, RoutedEventArgs e)
        {
            DeleteFile();
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                RenameFile();
            }
        }

        /// <summary>
        /// 重命名文件。
        /// </summary>
        /// <returns>若新路径存在文件，则返回失败。</returns>
        private bool RenameFile()
        {
            if (fileInfo.Name != textBox.Text)
            {
                string newPath = fileInfo.DirectoryName + @"\" + textBox.Text;
                if(fileInfo.Attributes==FileAttributes.Directory)
                {
                    if(Directory.Exists(newPath))
                    {
                        textBox.Text = fileInfo.Name;
                        textBox.IsReadOnly = true;
                        textBox.BorderBrush = null;
                        return false;
                    }
                }
                else
                {
                    if (File.Exists(newPath))
                    {
                        textBox.Text = fileInfo.Name;
                        textBox.IsReadOnly = true;
                        textBox.BorderBrush = null;
                        return false;
                    }
                }
                fileInfo.MoveTo(newPath);
            }
            textBox.IsReadOnly = true;
            return true;
        }

        private void border_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!isSelect) 
            {
                border.BorderBrush = ThemeManager.GetBrushByName("borderSelectColor");
                border.Background = ThemeManager.GetBrushByName("backgroundHoverColor");
            }
        }

        private void border_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!isSelect)
            {
                CancelSelectState();
            }
        }

        private void border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isSelect = true;
            border.Background = ThemeManager.GetBrushByName("backgroundSelectColor");
        }

        private void textBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if(!textBox.IsReadOnly)
            {
                RenameFile();
            }
        }

        public GameUIFileEnum GetFileEnum()
        {
            return gameUIFileEnum;
        }
    }
}
