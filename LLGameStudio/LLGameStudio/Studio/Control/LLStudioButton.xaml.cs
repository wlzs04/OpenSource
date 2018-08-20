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
    /// LLStudioButton.xaml 的交互逻辑
    /// </summary>
    public partial class LLStudioButton : UserControl
    {
        bool isSelect = false;

        MouseButtonEventHandler clickHandler;

        public MouseButtonEventHandler ClickHandler { get => clickHandler; set => clickHandler = value; }

        public static readonly DependencyProperty TextProperty =
    DependencyProperty.Register("Text", typeof(string), typeof(LLStudioButton), new PropertyMetadata("Label", new PropertyChangedCallback(OnTextChanged)));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        static void OnTextChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            ((LLStudioButton)sender).OnTextValueChanged(args);
        }
        protected void OnTextValueChanged(DependencyPropertyChangedEventArgs e)
        {
            label.Content = e.NewValue.ToString();
        }

        public LLStudioButton()
        {
            InitializeComponent();
        }

        private void border_MouseEnter(object sender, MouseEventArgs e)
        {
            border.BorderBrush = ThemeManager.GetBrushByName("borderSelectColor");
            border.Background = ThemeManager.GetBrushByName("backgroundHoverColor");
        }

        /// <summary>
        /// 设置图片路径
        /// </summary>
        /// <param name="filePath"></param>
        public void SetImage(string filePath)
        {
            Uri uri = new Uri(Environment.CurrentDirectory + @"\"+ filePath);
            image.Source = new BitmapImage(uri);
        }

        /// <summary>
        /// 设置文字
        /// </summary>
        /// <param name="content"></param>
        public void SetText(string content)
        {
            label.Content = content;
        }

        private void border_MouseLeave(object sender, MouseEventArgs e)
        {
            border.BorderBrush = null;
            border.Background = ThemeManager.GetBrushByName("backgroundAlphaColor");
            isSelect = false;
        }

        private void border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            border.BorderBrush = ThemeManager.GetBrushByName("borderSelectColor");
            border.Background = ThemeManager.GetBrushByName("backgroundHoverColor");
            isSelect = true;
        }

        private void border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(isSelect&& clickHandler!=null)
            {
                clickHandler(sender, e);
            }
            isSelect = false;
        }
    }
}
