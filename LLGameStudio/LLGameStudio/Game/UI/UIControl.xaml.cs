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
            ContextMenu = new ContextMenu();
            MenuItem mi0 = new MenuItem();
            mi0.Header = "删除";
            mi0.Click += menuItem0_Click;
            ContextMenu.Items.Add(mi0);
        }

        private void menuItem0_Click(object sender, RoutedEventArgs e)
        {
            RemoveThisNode();
        }

        /// <summary>
        /// 移除当期节点
        /// </summary>
        public virtual void RemoveThisNode()
        {

        }

        private void grid_MouseEnter(object sender, MouseEventArgs e)
        {
            border.BorderBrush = ThemeManager.GetBrushByName("borderUIHoverColor");
        }

        private void grid_MouseLeave(object sender, MouseEventArgs e)
        {
            if(!isSelect)
            {
                border.BorderBrush = ThemeManager.GetBrushByName("borderUIColor");
            }
        }

        /// <summary>
        /// 设置选中状态。
        /// </summary>
        public void SetSelectState()
        {
            Keyboard.Focus(this);
            isSelect = true;
            border.BorderBrush = ThemeManager.GetBrushByName("borderUISelectColor");
        }

        /// <summary>
        /// 取消选中状态。
        /// </summary>
        public void CancelSelectState()
        {
            isSelect = false;
            border.BorderBrush = ThemeManager.GetBrushByName("borderUIColor");
        }
    }
}
