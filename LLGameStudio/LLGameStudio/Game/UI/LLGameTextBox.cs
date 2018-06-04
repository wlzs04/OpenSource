using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;

namespace LLGameStudio.Game.UI
{
    class LLGameTextBox : IUINode
    {
        public Property.Text text = new Property.Text();
        public Property.TextFamily textFamily = new Property.TextFamily();
        TextBlock textBlock = new TextBlock();
        Border textBoxBorder = new Border();

        public LLGameTextBox()
        {
            AddProperty(text);
            AddProperty(textFamily);

            textBoxBorder.BorderBrush = new SolidColorBrush(Colors.Gray);
            textBoxBorder.BorderThickness = new System.Windows.Thickness(1);
            textBoxBorder.Child = textBlock;

            textBlock.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            textBlock.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            gridContent.Children.Add(textBoxBorder);
        }

        public override XElement ExportContentToXML()
        {
            XElement element = new XElement("LLGameTextBox");
            ExportAttrbuteToXML(element);
            return element;
        }

        public override void LoadContentFromXML(XElement element)
        {
            LoadAttrbuteFromXML(element);
        }

        public override void ResetUIProperty()
        {
            base.ResetUIProperty();
            textBlock.Text = text.Value;
            if (!textFamily.IsDefault)
            {
                LLFont font = FontManager.GetFontByName(textFamily.Value);
                textBlock.FontFamily = new FontFamily(font.FamilyName);
                textBlock.FontSize = font.Size;
                textBlock.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(font.Color));
            }
        }
    }
}
