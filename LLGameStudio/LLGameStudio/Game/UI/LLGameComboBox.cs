using LLGameStudio.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace LLGameStudio.Game.UI
{ 
    /// <summary>
    /// UI控件，显示按钮。
    /// </summary>
    class LLGameComboBox : IUINode
    {
        public Property.BackImage backImage = new Property.BackImage();
        public Property.ButtonImage buttonImage = new Property.ButtonImage();
        public Property.ButtonSize buttonSize = new Property.ButtonSize();

        private Grid button = new Grid();

        public LLGameComboBox()
        {
            button.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            gridContent.Children.Add(button);

            AddProperty(backImage);
            AddProperty(buttonImage);
            AddProperty(buttonSize);
        }

        public override XElement ExportContentToXML()
        {
            XElement element = new XElement("LLGameComboBox");
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
            if (!backImage.IsDefault)
            {
                gridContent.Background = new ImageBrush(new BitmapImage(new Uri(GameManager.GameResourcePath + @"\" + backImage.Value, UriKind.Relative)));
            }
            else
            {
                gridContent.Background = null;
            }
            if (!buttonImage.IsDefault)
            {
                button.Background = new ImageBrush(new BitmapImage(new Uri(GameManager.GameResourcePath + @"\" + buttonImage.Value, UriKind.Relative)));
            }
            else
            {
                button.Background = null;
            }
            button.Width = LLMath.IsRange1To0(buttonSize.Value)? buttonSize.Value*parentNode.actualWidth: buttonSize.Value;
        }
    }
}
