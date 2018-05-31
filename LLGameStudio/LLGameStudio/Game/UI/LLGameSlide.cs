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
    class LLGameSlide : IUINode
    {
        public Property.BackImage backImage = new Property.BackImage();
        public Property.BlockImage blockImage = new Property.BlockImage();
        public Property.Progress progress = new Property.Progress();
        public Property.ButtonSize buttonSize = new Property.ButtonSize();
        public Property.Pad pad = new Property.Pad();

        private Grid block = new Grid();

        public LLGameSlide()
        {
            block.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            gridContent.Children.Add(block);

            AddProperty(backImage);
            AddProperty(blockImage);
            AddProperty(progress);
            AddProperty(buttonSize);
            AddProperty(pad);
        }

        public override XElement ExportContentToXML()
        {
            XElement element = new XElement("LLGameSlide");
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
            if (!blockImage.IsDefault)
            {
                block.Background = new ImageBrush(new BitmapImage(new Uri(GameManager.GameResourcePath + @"\" + blockImage.Value, UriKind.Relative)));
            }
            else
            {
                block.Background = null;
            }

            block.Width = LLMath.IsRange1To0(buttonSize.Value) ? buttonSize.Value * parentNode.actualWidth : buttonSize.Value;
            double padLength= LLMath.IsRange1To0(pad.Value) ? pad.Value * actualWidth : pad.Value;
            block.Margin = new System.Windows.Thickness(padLength - block.Width /2+ progress.Value * (actualWidth-2* padLength),0,0,0);
        }
    }
}
