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
    /// LLStudioKeyItem.xaml 的交互逻辑
    /// </summary>
    public partial class LLStudioKeyItem : UserControl
    {
        bool isSelect = false;
        double keyFlagHeight = 40;
        double keyFlagWidth = 10;
        string name;

        public LLStudioKeyItem(string name,int level)
        {
            InitializeComponent();
            this.name = name;
            for (int i = 0; i < level; i++)
            {
                textBlockKeyName.Text += "    ";
            }
            textBlockKeyName.Text += name;
        }

        public void SetSelectState()
        {
            if(!isSelect)
            {
                isSelect = true;
                border.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF638774"));
            }
        }

        public void CancelSelectState()
        {
            if (isSelect)
            {
                isSelect = false;
                border.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF404E5C"));
            }
        }

        /// <summary>
        /// 添加Key标记
        /// </summary>
        /// <param name="x"></param>
        public void AddKeyFlag(double x)
        {
            Rectangle rectangle = new Rectangle();
            rectangle.Width = keyFlagWidth;
            rectangle.Height = keyFlagHeight;
            rectangle.Margin = new Thickness(x - keyFlagWidth / 2, (canvas.ActualHeight-keyFlagHeight)/2, 0, 0);
            canvas.Children.Add(rectangle);
        }

        /// <summary>
        /// 返回名称
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return name;
        }
    }
}
