using LLGameStudio.Common;
using LLGameStudio.Common.DataType;
using LLGameStudio.Common.XML;
using LLGameStudio.Studio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace LLGameStudio.Game.UI
{
    /// <summary>
    /// UI节点的父类类，作为UI类都必须继承此类，
    /// 有几点注意事项：
    /// 1.继承此类会继承默认属性，如果有新属性并希望新属性可以导出导入，
    /// 需要新添加继承IUIProperty的属性类，重写构造方法并在其中使用
    /// AddProperty()方法添加。如果只作为内部变量出现则不需要上述步骤。
    /// 2.内部统一使用WPF提供的Path进行绘制。
    /// </summary>
    abstract class IUINode : IXMLClass
    {
        protected double posX = 0;
        protected double posY = 0;
        protected Dictionary<string, IUIProperty> propertyDictionary = new Dictionary<string, IUIProperty>();

        public Property.GameUIAnchorEnum anchorEnum = new Property.GameUIAnchorEnum();
        public Property.Width width = new Property.Width();
        public Property.Height height = new Property.Height();
        public Property.Name name = new Property.Name();
        public Property.Rotation rotation = new Property.Rotation();
        public Property.Margin margin = new Property.Margin();
        
        protected Path path;
        protected IUINode parentNode=null;

        public IUINode()
        {
            AddProperty(anchorEnum);
            AddProperty(width);
            AddProperty(height);
            AddProperty(name);
            AddProperty(rotation);
            AddProperty(margin);

            path = new Path();
        }

        /// <summary>
        /// 添加属性
        /// </summary>
        /// <param name="property"></param>
        public void AddProperty(IUIProperty property)
        {
            propertyDictionary.Add(property.Name, property);
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetProperty(string name,string value)
        {
            propertyDictionary[name].Value = value;
        }

        public abstract XElement ExportContentToXML();
        public abstract void LoadContentFromXML(XElement element);
        public abstract void Render(CanvasManager canvasManager);

        public virtual void AddNode(IUINode node)
        {
            node.parentNode = this;
        }

        protected void LoadAttrbuteFromXML(XElement element)
        {
            XAttribute xAttribute;
            foreach (var item in propertyDictionary)
            {
                xAttribute = element.Attribute(item.Key);
                if (xAttribute != null) { item.Value.Value = xAttribute.Value; xAttribute.Remove(); }
            }
        }
    }
    namespace Property
    {
        class Name : IUIProperty
        {
            public Name() : base("name", typeof(String), UIPropertyEnum.Common, "当前UI节点的名称。", "node") { }
        }

        class Width : IUIProperty
        {
            public Width() : base("width", typeof(Double), UIPropertyEnum.Transform, "当前UI节点的宽度。", "0.2") { }
        }

        class Height : IUIProperty
        {
            public Height() : base("height", typeof(Double), UIPropertyEnum.Transform, "当前UI节点的高度。", "0.2") { }
        }

        class GameUIAnchorEnum : IUIProperty
        {
            public GameUIAnchorEnum() : base("anchorEnum", typeof(UI.GameUIAnchorEnum), UIPropertyEnum.Transform, "当前UI节点的节点锚点。", "Left_Top") { }
        }

        class Rotation : IUIProperty
        {
            public Rotation() : base("rotation", typeof(Vector2), UIPropertyEnum.Transform, "当前UI节点的旋转。", "{0,0}") { }
        }

        class FilePath : IUIProperty
        {
            public FilePath() : base("filePath", typeof(String), UIPropertyEnum.Common, "当前节点文件路径。", "") { }
        }

        class Margin : IUIProperty
        {
            public Margin() : base("margin", typeof(Rect), UIPropertyEnum.Common, "当前节点文件路径。", "{0}") { }
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
