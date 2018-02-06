﻿using LLGameStudio.Common;
using LLGameStudio.Common.XML;
using LLGameStudio.Studio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LLGameStudio.Game.UI
{
    /// <summary>
    /// UI布局文件类
    /// </summary>
    class LLGameLayout : IUINode
    {
        public Property.FilePath filePath = new Property.FilePath();
        public LLGameGrid llGameGrid;

        public LLGameLayout()
        {
            AddProperty(filePath);
        }

        public bool LoadContentFromFile(string path)
        {
            if (!Path.IsPathRooted(path))
            {
                filePath.Value = GameManager.GameResourcePath + @"\" + path;
            }
            else
            {
                filePath.Value = path;
            }
            llGameGrid = new LLGameGrid();
            LLConvert.LoadContentFromXML(filePath.Value, llGameGrid);
            AddNode(llGameGrid);
            return true;
        }

        public XElement ExportGridContentToXML()
        {
            return llGameGrid.ExportContentToXML();
        }

        public override XElement ExportContentToXML()
        {
            XElement element = new XElement("LLGameLayout");
            ExportAttrbuteToXML(element);
            return element;
        }

        public override void LoadContentFromXML(XElement element)
        {
            LoadAttrbuteFromXML(element);
            LoadContentFromFile(filePath.Value);
        }

        public override void AddUINodeToCanvas(CanvasManager canvasManager)
        {
            canvasManager.AddRootUINode(this);
        }

        public override void ResetUIProperty()
        {
            base.ResetUIProperty();
            llGameGrid.ResetUIProperty();
        }
    }
}
