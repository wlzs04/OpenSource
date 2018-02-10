﻿using LLGameStudio.Common.XML;
using LLGameStudio.Studio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LLGameStudio.Game.UI
{
    class LLGameBack : IUINode
    {
        public override XElement ExportContentToXML()
        {
            XElement element = new XElement("LLGameBack");
            ExportAttrbuteToXML(element);
            return element;
        }

        public override void LoadContentFromXML(XElement element)
        {
            LoadAttrbuteFromXML(element);
        }
    }
}
