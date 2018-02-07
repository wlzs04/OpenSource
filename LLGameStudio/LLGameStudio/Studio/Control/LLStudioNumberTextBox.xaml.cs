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
    /// LLStudioNumberTextBox.xaml 的交互逻辑
    /// </summary>
    public partial class LLStudioNumberTextBox : UserControl
    {
        Color color_TextBox_Default = Color.FromArgb(0xFF, 0x64, 0x64, 0x64);//文本框默认颜色
        public float number = 0;

        public LLStudioNumberTextBox()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 若TextBox只可以添加小数，则只可以输入数字和小数点，在键盘输入时检查，不符合要求时取消操作。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreviewTextInput_CanAppendToDouble(object sender, TextCompositionEventArgs e)
        {
            if (!(char.IsDigit(e.Text[0]) || e.Text[0] == '.' || e.Text[0] == '-'))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// 在键盘输入时检查，输入为空格时取消,输入为回车时尝试将文字转换为数字。
        /// 失败时背景变红。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreviewKeyDown_ExceptSpace(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
            if (e.Key == Key.Enter)
            {
                if (float.TryParse(textBox.Text, out number))
                {
                    textBox.Text = number.ToString();
                    border.Background = new SolidColorBrush(color_TextBox_Default);
                }
                else
                {
                    if (textBox.Text == string.Empty)
                    {
                        number = 0;
                        textBox.Text = "0";
                        border.Background = new SolidColorBrush(color_TextBox_Default);
                    }
                    else
                    {
                        border.Background = new SolidColorBrush(Colors.Red);
                    }
                }
            }
        }
    }
}
