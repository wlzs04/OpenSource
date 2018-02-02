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
    /// LLStudioSelectBorder.xaml 的交互逻辑
    /// </summary>
    public partial class LLStudioSelectBorder : UserControl
    {
        public LLStudioSelectBorder()
        {
            InitializeComponent();
            border.BorderBrush = ThemeManager.GetBrushByName("borderUISelectColor");
            border.Background = ThemeManager.GetBrushByName("backgroundUIAlphaColor");
            gridLeft.Background = ThemeManager.GetBrushByName("backgroundSelectBorderColor");
            gridTop.Background = ThemeManager.GetBrushByName("backgroundSelectBorderColor");
            gridRight.Background = ThemeManager.GetBrushByName("backgroundSelectBorderColor");
            gridBottom.Background = ThemeManager.GetBrushByName("backgroundSelectBorderColor");
        }

        public void SetRect(Rect rect)
        {
            Margin = new Thickness(rect.Left, rect.Top, 0, 0);
            Width = rect.Width;
            Height = rect.Height;
        }
    }
}
