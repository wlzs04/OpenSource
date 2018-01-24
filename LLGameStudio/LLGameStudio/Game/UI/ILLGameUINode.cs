using LLGameStudio.Common.XML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LLGameStudio.Game.UI
{
    /// <summary>
    /// UI节点抽象类
    /// </summary>
    abstract class ILLGameUINode : IXMLClass
    {
        protected double posX = 0;
        protected double posY = 0;
        protected double rotateX = 0;
        protected double rotateY = 0;
        protected double scaleX = 1;
        protected double scaleY = 1;
        protected GameUIAnchorEnum anchorEnum = GameUIAnchorEnum.Left_Top;
        protected double width = 0.2;
        protected double height = 0.2;
        protected string name = "";

        public double RotateX { get => rotateX; set => rotateX = value; }
        public double RotateY { get => rotateY; set => rotateY = value; }
        public double ScaleX { get => scaleX; set => scaleX = value; }
        public double ScaleY { get => scaleY; set => scaleY = value; }
        public GameUIAnchorEnum AnchorEnum { get => anchorEnum; set => anchorEnum = value; }
        public double Width { get => width; set => width = value; }
        public double Height { get => height; set => height = value; }
        public string Name { get => name; set => name = value; }

        public abstract XElement ExportContentToXML();
        public abstract void LoadContentFromXML(XElement element);
        public abstract void Render();

        protected void LoadBaseAttrbuteFromXML(XElement element)
        {
            XAttribute xAttribute;
            xAttribute = element.Attribute("rotateX");
            if (xAttribute != null) { rotateX = Convert.ToDouble(xAttribute.Value); xAttribute.Remove(); }
            xAttribute = element.Attribute("rotateY");
            if (xAttribute != null) { rotateY = Convert.ToDouble(xAttribute.Value); xAttribute.Remove(); }
            xAttribute = element.Attribute("scaleX");
            if (xAttribute != null) { scaleX = Convert.ToDouble(xAttribute.Value); xAttribute.Remove(); }
            xAttribute = element.Attribute("scaleY");
            if (xAttribute != null) { scaleY = Convert.ToDouble(xAttribute.Value); xAttribute.Remove(); }
            xAttribute = element.Attribute("width");
            if (xAttribute != null) { width = Convert.ToDouble(xAttribute.Value); xAttribute.Remove(); }
            xAttribute = element.Attribute("height");
            if (xAttribute != null) { height = Convert.ToDouble(xAttribute.Value); xAttribute.Remove(); }
            xAttribute = element.Attribute("name");
            if (xAttribute != null) { name = xAttribute.Value; xAttribute.Remove(); }
            xAttribute = element.Attribute("anchorEnum");
            if (xAttribute != null) { anchorEnum = (GameUIAnchorEnum)Enum.Parse(typeof(GameUIAnchorEnum), xAttribute.Value); xAttribute.Remove(); }
        }
    }

    /// <summary>
    /// UI节点锚点枚举
    /// </summary>
    enum GameUIAnchorEnum
    {
        Center=0,
        Left =1,
        Top=2,
        Right=4,
        Bottom=8,
        Left_Top = Left| Top,
        Right_Top = Right | Top,
        Right_Bottom = Right | Bottom,
        Left_Bottom = Left | Bottom,
    };
}
