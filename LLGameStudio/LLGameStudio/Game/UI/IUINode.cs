﻿using LLGameStudio.Common;
using LLGameStudio.Common.DataType;
using LLGameStudio.Common.Helper;
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
    public abstract class IUINode : UIControl, IXMLClass
    {
        public List<IUINode> listNode;
        public double actualWidth = 0;
        public double actualHeight = 0;
        public Rect actualMargin;

        public Dictionary<string, IUIProperty> propertyDictionary = new Dictionary<string, IUIProperty>();

        public Property.GameUIAnchorEnum anchorEnum = new Property.GameUIAnchorEnum();
        public Property.Width width = new Property.Width();
        public Property.Height height = new Property.Height();
        public Property.Name name = new Property.Name();
        public Property.Rotation rotation = new Property.Rotation();
        public Property.Margin margin = new Property.Margin();
        public Property.ClipByParent clipByParent = new Property.ClipByParent();

        public IUINode parentNode = null;

        public IUINode()
        {
            AddProperty(name);
            AddProperty(anchorEnum);
            AddProperty(width);
            AddProperty(height);
            AddProperty(margin);
            AddProperty(rotation);
            AddProperty(clipByParent);

            listNode = new List<IUINode>();
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
        public virtual void SetProperty(string name, string value)
        {
            propertyDictionary[name].Value = value;
            ResetUIProperty();
        }

        /// <summary>
        /// 用于在对宽度、旋转等属性进行赋值后，重新计算UI节点真实宽度等属性的方法。
        /// </summary>
        public virtual void ResetUIProperty()
        {
            width.Value = width.Value;
            height.Value = height.Value;

            double parentWidth = 0;
            double parentHeight = 0;
            actualWidth = width.Value;
            actualHeight = height.Value;
            double actualLeft = margin.Value.Left;
            double actualTop = margin.Value.Top;
            double actualRight = margin.Value.Right;
            double actualBottom = margin.Value.Bottom;
            if (parentNode != null)
            {
                parentWidth = parentNode.actualWidth;
                parentHeight = parentNode.actualHeight;
            }
            else
            {
                parentWidth = GameManager.GameWidth;
                parentHeight = GameManager.GameHeight;
            }
            actualWidth = LLMath.IsRange1To0(actualWidth) ? parentWidth * actualWidth : actualWidth;
            actualHeight = LLMath.IsRange1To0(actualHeight) ? parentHeight * actualHeight : actualHeight;
            actualLeft = LLMath.IsRange1To0(actualLeft) ? parentWidth * actualLeft : actualLeft;
            actualTop = LLMath.IsRange1To0(actualTop) ? parentHeight * actualTop : actualTop;
            actualRight = LLMath.IsRange1To0(actualRight) ? parentWidth * actualRight : actualRight;
            actualBottom = LLMath.IsRange1To0(actualBottom) ? parentHeight * actualBottom : actualBottom;

            if ((anchorEnum.Value & GameUIAnchorEnum.Left) != 0)
            {
                actualMargin.Left = actualLeft;
            }
            else if ((anchorEnum.Value & GameUIAnchorEnum.Right) != 0)
            {
                actualMargin.Left = parentWidth - actualRight - actualWidth;
            }
            else
            {
                actualMargin.Left = (parentWidth - actualWidth) / 2;
            }
            if ((anchorEnum.Value & GameUIAnchorEnum.Top) != 0)
            {
                actualMargin.Top = actualTop;
            }
            else if ((anchorEnum.Value & GameUIAnchorEnum.Bottom) != 0)
            {
                actualMargin.Top = +parentHeight - actualBottom - actualHeight;
            }
            else
            {
                actualMargin.Top = (parentHeight - actualHeight) / 2;
            }
            Width = actualWidth;
            Height = actualHeight;
            Margin = new System.Windows.Thickness(actualMargin.Left, actualMargin.Top, 0, 0);
        }

        /// <summary>
        /// 向X、Y方向移动的距离，锚点为中间时对应方向的移动无效。
        /// 例：锚点为Top时，X方向的移动无效。
        /// </summary>
        public void Move(double x, double y)
        {
            actualMargin.Left += x;
            actualMargin.Right -= x;
            actualMargin.Top += y;
            actualMargin.Bottom -= y;

            margin.Value.Left += x;
            if (LLMath.IsRange1To0(margin.Value.Left))
            {
                margin.Value.Left = LLMath.One;
            }
            margin.Value.Right -= x;
            if (LLMath.IsRange1To0(margin.Value.Right))
            {
                margin.Value.Right = LLMath.One;
            }
            margin.Value.Top += y;
            if (LLMath.IsRange1To0(margin.Value.Top))
            {
                margin.Value.Top = LLMath.One;
            }
            margin.Value.Bottom -= y;
            if (LLMath.IsRange1To0(margin.Value.Bottom))
            {
                margin.Value.Bottom = LLMath.One;
            }

            Margin = new System.Windows.Thickness(actualMargin.Left, actualMargin.Top, 0, 0);
        }

        /// <summary>
        /// 移动到相对于父节点的指定位置,并将UI节点的锚点设为左上角。
        /// </summary>
        public void MoveTo(double x, double y)
        {
            anchorEnum.Value = GameUIAnchorEnum.Left_Top;
            actualMargin.Left = x;
            actualMargin.Top = y;
            margin.Value.Left = x;
            if (LLMath.IsRange1To0(margin.Value.Left))
            {
                margin.Value.Left = LLMath.One;
            }
            margin.Value.Top = x;
            if (LLMath.IsRange1To0(margin.Value.Top))
            {
                margin.Value.Top = LLMath.One;
            }
            Margin = new System.Windows.Thickness(actualMargin.Left, actualMargin.Top, 0, 0);
        }

        /// <summary>
        /// 设置宽度
        /// </summary>
        /// <param name="d"></param>
        public void SetWidth(double d)
        {
            if (d < 0)
            {
                actualWidth = 0;
                width.Value = 0;
            }
            else
            {
                actualWidth = d;
                width.Value = d;
                if (LLMath.IsRange1To0(width.Value))
                {
                    width.Value = LLMath.One;
                }
            }
            ResetUIProperty();
        }

        /// <summary>
        /// 设置高度
        /// </summary>
        /// <param name="d"></param>
        public void SetHeight(double d)
        {
            if (d < 0)
            {
                actualHeight = 0;
                height.Value = 0;
            }
            else
            {
                actualHeight = d;
                height.Value = d;
                if (LLMath.IsRange1To0(height.Value))
                {
                    height.Value = LLMath.One;
                }
            }
            ResetUIProperty();
        }

        public abstract XElement ExportContentToXML();
        public abstract void LoadContentFromXML(XElement element);

        /// <summary>
        /// 向当前节点添加子节点
        /// </summary>
        /// <param name="node"></param>
        public virtual void AddNode(IUINode node)
        {
            node.parentNode = this;
            listNode.Add(node);
            grid.Children.Add(node);
        }

        /// <summary>
        /// 移除当前节点的指定子节点
        /// </summary>
        /// <param name="node"></param>
        public virtual void RemoveNode(IUINode node)
        {
            node.parentNode = null;
            listNode.Remove(node);
            grid.Children.Remove(node);
            StudioManager.GetSingleInstance().TreeResetItem();
            CanvasManager.GetSingleInstance().SelectUINode(this);
            GameManager.GetSingleInstance().SelectUINode(this);
        }

        /// <summary>
        /// 从XML节点中加载属性。
        /// </summary>
        /// <param name="element"></param>
        protected void LoadAttrbuteFromXML(XElement element)
        {
            XAttribute xAttribute;
            foreach (var item in propertyDictionary)
            {
                xAttribute = element.Attribute(item.Key);
                if (xAttribute != null) { item.Value.Value = xAttribute.Value; xAttribute.Remove(); }
            }
        }

        /// <summary>
        /// 导出属性到XML节点。
        /// </summary>
        /// <param name="element"></param>
        protected void ExportAttrbuteToXML(XElement element)
        {
            foreach (var item in propertyDictionary)
            {
                if(!item.Value.IsDefault)
                {
                    element.Add(new XAttribute(item.Value.Name, item.Value.Value));
                }
            }
        }

        /// <summary>
        /// 控制UI节点的左边界的移动。
        /// </summary>
        /// <param name="d"></param>
        public void ChangeLeft(double d)
        {
            if ((anchorEnum.Value & GameUIAnchorEnum.Left) != 0)
            {
                Move(d, 0);
                SetWidth(actualWidth - d);
            }
            else if ((anchorEnum.Value & GameUIAnchorEnum.Right) != 0)
            {
                SetWidth(actualWidth - d);
            }
            else
            {
                Move(d, 0);
                SetWidth(actualWidth - 2 * d);
            }
        }

        /// <summary>
        /// 控制UI节点的上边界的移动。
        /// </summary>
        /// <param name="d"></param>
        public void ChangeTop(double d)
        {
            if ((anchorEnum.Value & GameUIAnchorEnum.Top) != 0)
            {
                Move(0, d);
                SetHeight(actualHeight - d);
            }
            else if ((anchorEnum.Value & GameUIAnchorEnum.Bottom) != 0)
            {
                SetHeight(actualHeight - d);
            }
            else
            {
                Move(0, d);
                SetHeight(actualHeight - 2 * d);
            }
        }

        /// <summary>
        /// 控制UI节点的右边界的移动。
        /// </summary>
        /// <param name="d"></param>
        public void ChangeRight(double d)
        {
            if ((anchorEnum.Value & GameUIAnchorEnum.Left) != 0)
            {
                SetWidth(actualWidth + d);
            }
            else if ((anchorEnum.Value & GameUIAnchorEnum.Right) != 0)
            {
                margin.Value.Right -= d;
                if (LLMath.IsRange1To0(margin.Value.Right))
                {
                    margin.Value.Right = LLMath.One;
                }
                actualMargin.Right -= d;
                SetWidth(actualWidth + d);
            }
            else
            {
                Move(-d, 0);
                SetWidth(actualWidth + 2 * d);
            }
        }

        /// <summary>
        /// 控制UI节点的下边界的移动。
        /// </summary>
        /// <param name="d"></param>
        public void ChangeBottom(double d)
        {
            if ((anchorEnum.Value & GameUIAnchorEnum.Top) != 0)
            {
                SetHeight(actualHeight + d);
            }
            else if ((anchorEnum.Value & GameUIAnchorEnum.Bottom) != 0)
            {
                Move(0, d);
                SetHeight(actualHeight + d);
            }
            else
            {
                Move(0, d);
                SetHeight(actualHeight + 2 * d);
            }
        }

        /// <summary>
        /// 从父节点中移除当前节点
        /// </summary>
        public override void RemoveThisNode()
        {
            if(parentNode!=null)
            {
                parentNode.RemoveNode(this);
            }
            else
            {
                System.Windows.MessageBox.Show("根节点无法移除");
            }
        }
    }

    namespace Property
    {
        public class Name : IUIProperty
        {
            public Name() : base("name", typeof(String), UIPropertyEnum.Common, "当前UI节点的名称。", "node") { }
        }

        public class Width : IUIProperty
        {
            public Width() : base("width", typeof(Double), UIPropertyEnum.Transform, "当前UI节点的宽度。", "1") { }
        }

        public class Height : IUIProperty
        {
            public Height() : base("height", typeof(Double), UIPropertyEnum.Transform, "当前UI节点的高度。", "1") { }
        }

        public class GameUIAnchorEnum : IUIProperty
        {
            public GameUIAnchorEnum() : base("anchorEnum", typeof(UI.GameUIAnchorEnum), UIPropertyEnum.Transform, "当前UI节点的节点锚点。", "Left_Top") { }
        }

        public class Rotation : IUIProperty
        {
            public Rotation() : base("rotation", typeof(Vector2), UIPropertyEnum.Transform, "当前UI节点的旋转。", "{0,0}") { }
        }

        public class FilePath : IUIProperty
        {
            public FilePath() : base("filePath", typeof(String), UIPropertyEnum.Common, "当前节点文件路径。", "") { }
        }

        public class Image : IUIProperty
        {
            public Image() : base("image", typeof(String), UIPropertyEnum.Common, "图片文件路径。", "") { }
        }

        public class Margin : IUIProperty
        {
            public Margin() : base("margin", typeof(Rect), UIPropertyEnum.Transform, "当前节点相对父容器位置。", "{0}") { }
        }

        public class ClipByParent : IUIProperty
        {
            public ClipByParent() : base("clipByParent", typeof(bool), UIPropertyEnum.Common, "是否被父容器剪切。", "False") { }
        }

        public class Text : IUIProperty
        {
            public Text() : base("text", typeof(string), UIPropertyEnum.Common, "显示文字。", "") { }
        }

        public class TextFamily : IUIProperty
        {
            public TextFamily() : base("textFamily", typeof(string), UIPropertyEnum.Common, "显示文字的样式。", "") { }
        }

        public class BackImage : IUIProperty
        {
            public BackImage() : base("backImage", typeof(string), UIPropertyEnum.Common, "背景图片。", "") { }
        }

        public class ButtonImage : IUIProperty
        {
            public ButtonImage() : base("buttonImage", typeof(string), UIPropertyEnum.Common, "按钮图片。", "") { }
        }
        

        public class ButtonSize : IUIProperty
        {
            public ButtonSize() : base("buttonSize", typeof(double), UIPropertyEnum.Common, "按钮大小。", "0.1") { }
        }
        
        public class BlockImage : IUIProperty
        {
            public BlockImage() : base("blockImage", typeof(string), UIPropertyEnum.Common, "滑块图片。", "") { }
        }
        
        public class Progress : IUIProperty
        {
            public Progress() : base("progress", typeof(double), UIPropertyEnum.Common, "进度。", "0") { }
        }

        public class Pad : IUIProperty
        {
            public Pad() : base("pad", typeof(double), UIPropertyEnum.Common, "内容距离控件边缘长度。", "0") { }
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
