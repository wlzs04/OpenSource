using LLGameStudio.Common;
using LLGameStudio.Common.XML;
using LLGameStudio.Studio;
using System;
using System.Collections.Generic;
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
        LLGameGrid llGameGrid;

        public LLGameLayout()
        {
        }

        public bool LoadContentFromFile(string path)
        {
            filePath.Value = path;
            llGameGrid = new LLGameGrid();
            LLConvert.LoadContentFromXML(path, llGameGrid);
            return true;
        }

        public override XElement ExportContentToXML()
        {
            throw new NotImplementedException();
        }

        public override void LoadContentFromXML(XElement element)
        {
            LoadAttrbuteFromXML(element);
            
        }

        public override void AddUINodeToCanvas(CanvasManager canvasManager)
        {
            canvasManager.AddUINode(this);
            llGameGrid.AddUINodeToCanvas(canvasManager);
        }

        public override void ResetUIProperty()
        {
            base.ResetUIProperty();
            llGameGrid.ResetUIProperty();
        }
    }
}
