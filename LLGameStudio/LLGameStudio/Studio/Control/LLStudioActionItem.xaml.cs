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
    /// LLStudioActionItem.xaml 的交互逻辑
    /// </summary>
    public partial class LLStudioActionItem : UserControl
    {
        private bool isSelect;
        string name;

        public LLStudioActionItem(string name)
        {
            InitializeComponent();
            this.name = name;
            textBox.IsReadOnly = true;
            textBox.Text = name;
            border.Background = ThemeManager.GetBrushByName("backgroundAlphaColor");
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                
            }
        }

        private void textBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!textBox.IsReadOnly)
            {
                
            }
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

        /// <summary>
        /// 取消选中状态
        /// </summary>
        public void CancelSelectState()
        {
            isSelect = false;
            border.BorderBrush = null;
            border.Background = ThemeManager.GetBrushByName("backgroundAlphaColor");
        }

        /// <summary>
        /// 获得名称
        /// </summary>
        public string GetName()
        {
            return name;
        }
    }
}
