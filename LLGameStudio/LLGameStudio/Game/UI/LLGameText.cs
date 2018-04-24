﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;

namespace LLGameStudio.Game.UI
{
    /// <summary>
    /// UI控件，显示文字。
    /// </summary>
    class LLGameText : IUINode
    {
        public Property.Text text = new Property.Text();
        TextBlock textBlock = new TextBlock();

        public LLGameText()
        {
            AddProperty(text); 
            textBlock.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            textBlock.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            gridContent.Children.Add(textBlock);
        }

        public override XElement ExportContentToXML()
        {
            XElement element = new XElement("LLGameText");
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
        }
    }
}