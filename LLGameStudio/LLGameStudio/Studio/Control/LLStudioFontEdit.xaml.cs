using LLGameStudio.Game;
using LLGameStudio.Studio.Window;
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
    /// LLStudioFontEdit.xaml 的交互逻辑
    /// </summary>
    public partial class LLStudioFontEdit : UserControl
    {
        public delegate void NameHandler(string oldName, LLFont font);
        public delegate void AttributeHandler(LLFont font);
        public delegate void DeleteHandler(string name);
        public NameHandler changeNameHandler;
        public AttributeHandler changeAttributeHandler;
        public DeleteHandler deleteHandler;

        string fontName = "";
        LLFont font;

        public LLStudioFontEdit()
        {
            InitializeComponent();
            buttonDelete.SetImage("Resource/最小化.png");
            buttonDelete.ToolTip = "删除";
            buttonDelete.ClickHandler = DeleteFontHandle;
        }

        public void SetContent(LLFont font)
        {
            this.font = font;
            fontName = font.Name;
            textBoxName.Text = font.Name;
            textBoxFamilyName.Text = font.FamilyName;
            textBoxColor.Text = font.Color;
            textBoxSize.Text = font.Size.ToString();
            SetTestText();
        }

        void SetTestText()
        {
            textBoxTest.FontFamily = new FontFamily(font.FamilyName);
            textBoxTest.FontSize = font.Size;
            textBoxTest.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(font.Color));
        }

        private void textBoxAttribute_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                font.FamilyName = textBoxFamilyName.Text;
                font.Color = textBoxColor.Text;
                font.Size = Convert.ToDouble(textBoxSize.Text);
                SetTestText();
                changeAttributeHandler(font);
            }
        }

        private void textBoxAttribute_LostFocus(object sender, RoutedEventArgs e)
        {
            font.FamilyName = textBoxFamilyName.Text;
            font.Color = textBoxColor.Text;
            font.Size = Convert.ToDouble(textBoxSize.Text);
            SetTestText();
            changeAttributeHandler(font);
        }

        private void textBoxName_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.Enter)
            {
                if (fontName != textBoxName.Text)
                {
                    font.Name = textBoxName.Text;
                    changeNameHandler(fontName, font);
                }
            }
        }

        private void textBoxName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (fontName != textBoxName.Text)
            {
                font.Name = textBoxName.Text;
                changeNameHandler(fontName, font);
            }
        }

        void DeleteFontHandle(object sender, MouseButtonEventArgs e)
        {
            deleteHandler(fontName);
        }
    }
}
