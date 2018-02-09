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
    /// LLStudioTextBox.xaml 的交互逻辑
    /// </summary>
    public partial class LLStudioTextBox : UserControl
    {
        public delegate void ChangTextHandle(string s);
        public ChangTextHandle ChangText;

        public LLStudioTextBox()
        {
            InitializeComponent();
        }

        public void SetText(string s)
        {
            textBox.Text = s;
        }

        /// <summary>
        /// 输入回车时进行内容变换。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreviewKeyDown_SetValue(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ChangText?.Invoke(textBox.Text);
            }
        }
    }
}
