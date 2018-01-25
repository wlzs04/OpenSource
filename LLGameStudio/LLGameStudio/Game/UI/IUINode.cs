using LLGameStudio.Common.XML;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace LLGameStudio.Game.UI
{
    /// <summary>
    /// UI节点抽象类
    /// </summary>
    abstract class IUINode : IXMLClass
    {
        protected double posX = 0;
        protected double posY = 0;
        protected double rotateX = 0;
        protected double rotateY = 0;
        protected double scaleX = 1;
        protected double scaleY = 1;
        public Property.GameUIAnchorEnum anchorEnum = new Property.GameUIAnchorEnum();
        public Property.Width width = new Property.Width();
        public Property.Height height = new Property.Height();
        public Property.Name name = new Property.Name();

        public double RotateX { get => rotateX; set => rotateX = value; }
        public double RotateY { get => rotateY; set => rotateY = value; }
        public double ScaleX { get => scaleX; set => scaleX = value; }
        public double ScaleY { get => scaleY; set => scaleY = value; }

        public abstract XElement ExportContentToXML();
        public abstract void LoadContentFromXML(XElement element);
        public abstract void Render();

        protected void LoadBaseAttrbuteFromXML(XElement element)
        {
            //TypeConverter;
            Vector vector = new Vector();
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
            if (xAttribute != null) { width.Value = xAttribute.Value; xAttribute.Remove(); }
            xAttribute = element.Attribute("height");
            if (xAttribute != null) { height.Value = xAttribute.Value; xAttribute.Remove(); }
            xAttribute = element.Attribute("name");
            if (xAttribute != null) { name.Value = xAttribute.Value; xAttribute.Remove(); }
            xAttribute = element.Attribute("anchorEnum");
            if (xAttribute != null) { anchorEnum.Value =xAttribute.Value; xAttribute.Remove(); }
        }
    }
    namespace Property
    {
        class Name : IUIProperty
        {
            public Name() : base("Name", typeof(String), UIPropertyEnum.Common, "当前UI节点的名称。", "node") { }
        }

        class Width : IUIProperty
        {
            public Width() : base("Width", typeof(Double), UIPropertyEnum.Transform, "当前UI节点的宽度。", "0.2") { }
        }

        class Height : IUIProperty
        {
            public Height() : base("Height", typeof(Double), UIPropertyEnum.Transform, "当前UI节点的高度。", "0.2") { }
        }

        class GameUIAnchorEnum : IUIProperty
        {
            public GameUIAnchorEnum() : base("GameUIAnchorEnum", typeof(UI.GameUIAnchorEnum), UIPropertyEnum.Transform, "当前UI节点的节点锚点。", "Left_Top") { }
        }

        class Scale : IUIProperty
        {
            public Scale() : base("Scale", typeof(Vector), UIPropertyEnum.Transform, "当前UI节点的缩放。", "0.2") { }
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
