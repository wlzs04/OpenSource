using LLGameStudio.Studio;
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

namespace LLGameStudio.Game.UI
{
    /// <summary>
    /// UIControl.xaml 的交互逻辑
    /// </summary>
    public partial class UIControl : UserControl
    {
        protected bool isSelect = false;

        public UIControl()
        {
            InitializeComponent();
            border.BorderBrush = ThemeManager.GetBrushByName("borderUIColor");
            grid.Background = ThemeManager.GetBrushByName("backgroundUIAlphaColor");
        }

        private void border_MouseEnter(object sender, MouseEventArgs e)
        {
            border.BorderBrush = ThemeManager.GetBrushByName("borderUIHoverColor");
        }

        private void border_MouseLeave(object sender, MouseEventArgs e)
        {
            if(!isSelect)
            {
                CancelSelectState();
            }
        }

        public void SelectUINode()
        {
            Keyboard.Focus(this);
            isSelect = true;
            border.BorderBrush = ThemeManager.GetBrushByName("borderUISelectColor");
        }

        private void border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SelectUINode();
        }

        private void border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            
        }

        /// <summary>
        /// 取消选中状态。
        /// </summary>
        public void CancelSelectState()
        {
            isSelect = false;
            border.BorderBrush = ThemeManager.GetBrushByName("borderUIColor");
        }

        /// <summary>
        /// 取消所有UI节点的选择状态。
        /// </summary>
        private void CancelAllUINodeSelectState()
        {
            Keyboard.Focus(this);
            if (!Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                foreach (var item in ((Canvas)Parent).Children)
                {
                    if (item != this)
                    {
                        ((UIControl)item).CancelSelectState();
                    }
                }
            }
        }
    }
}
