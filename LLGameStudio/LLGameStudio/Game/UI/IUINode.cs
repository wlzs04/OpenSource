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
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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
    abstract class IUINode : UIControl, IXMLClass
    {
        protected double posX = 0;
        protected double posY = 0;
        protected double actualWidth = 0;
        protected double actualHeight = 0;
        protected Dictionary<string, IUIProperty> propertyDictionary = new Dictionary<string, IUIProperty>();

        public Property.GameUIAnchorEnum anchorEnum = new Property.GameUIAnchorEnum();
        public Property.Width width = new Property.Width();
        public Property.Height height = new Property.Height();
        public Property.Name name = new Property.Name();
        public Property.Rotation rotation = new Property.Rotation();
        public Property.Margin margin = new Property.Margin();
        public Property.ClipByParent clipByParent = new Property.ClipByParent();

        public IUINode parentNode=null;

        public IUINode()
        {
            AddProperty(anchorEnum);
            AddProperty(width);
            AddProperty(height);
            AddProperty(name);
            AddProperty(rotation);
            AddProperty(margin);
            AddProperty(clipByParent);
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

        /// <summary>
        /// 用于在对宽度、旋转等属性进行赋值后，重新计算UI节点真实宽度等属性的方法。
        /// </summary>
        public virtual void ResetUIProperty()
        {
            double parentWidth = 0;
            double parentHeight = 0;
            actualWidth = width.Value;
            actualHeight = height.Value;
            double actualLeft = margin.Value.Left;
            double actualTop = margin.Value.Top;
            double actualRight = margin.Value.Right;
            double actualBottom = margin.Value.Bottom;
            if (parentNode!=null)
            {
                parentWidth = parentNode.actualWidth;
                parentHeight = parentNode.actualHeight;
                actualWidth = actualWidth <= 1 ? parentWidth * actualWidth : actualWidth;
                actualHeight = actualHeight <= 1 ? parentHeight * actualHeight : actualHeight;
                actualLeft = actualLeft <= 1 ? parentWidth * actualLeft : actualLeft;
                actualTop = actualTop <= 1 ? parentHeight * actualTop : actualTop;
                actualRight = actualRight <= 1 ? parentWidth * actualRight : actualRight;
                actualBottom = actualBottom <= 1 ? parentHeight * actualBottom : actualBottom;
            }
            if ((anchorEnum.Value & GameUIAnchorEnum.Left)!=0)
            {
                posX = actualLeft;
            }
            else if ((anchorEnum.Value & GameUIAnchorEnum.Right) != 0)
            {
                posX =  parentWidth - actualRight - actualWidth;
            }
            else
            {
                posX = (parentWidth - actualWidth) / 2;
            }
            if((anchorEnum.Value & GameUIAnchorEnum.Top) != 0)
            {
                posY = actualTop;
            }
            else if((anchorEnum.Value & GameUIAnchorEnum.Bottom) != 0)
            {
                posY =  + parentHeight- actualBottom - actualHeight;
            }
            else
            {
                posY = (parentHeight - actualHeight) / 2;
            }

            Width = actualWidth;
            Height = actualHeight;
            Margin = new System.Windows.Thickness(posX,posY,0,0);
            ClipToBounds = clipByParent.Value;
        }

        public abstract XElement ExportContentToXML();
        public abstract void LoadContentFromXML(XElement element);
        public abstract void AddUINodeToCanvas(CanvasManager canvasManager);
        
        public virtual void AddNode(IUINode node)
        {
            node.parentNode = this;
            grid.Children.Add(node);
        }

        public virtual void RemoveNode(IUINode node)
        {
            node.parentNode = null;
            grid.Children.Remove(node);
        }

        protected void LoadAttrbuteFromXML(XElement element)
        {
            XAttribute xAttribute;
            foreach (var item in propertyDictionary)
            {
                xAttribute = element.Attribute(item.Key);
                if (xAttribute != null) {item.Value.Value = xAttribute.Value; xAttribute.Remove(); }
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
            public Width() : base("width", typeof(Double), UIPropertyEnum.Transform, "当前UI节点的宽度。", "1") { }
        }

        class Height : IUIProperty
        {
            public Height() : base("height", typeof(Double), UIPropertyEnum.Transform, "当前UI节点的高度。", "1") { }
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

        class ClipByParent : IUIProperty
        {
            public ClipByParent() : base("clipByParent", typeof(bool), UIPropertyEnum.Common, "是否被父容器剪切。", "false") { }
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
