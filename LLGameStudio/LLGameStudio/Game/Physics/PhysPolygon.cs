using LLGameStudio.Common.XML;
using LLGameStudio.Game.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace LLGameStudio.Game.Physics
{
    /// <summary>
    /// 用来保存多边形碰撞信息的类
    /// </summary>
    class PhysPolygon : IXMLClass
    {
        public Dictionary<string, IUIProperty> propertyDictionary = new Dictionary<string, IUIProperty>();

        public List<Point> listPoint = new List<Point>();

        public PhysPolygon()
        {
            
        }

        void AddPoint(Point p)
        {
            listPoint.Add(p);
        }

        /// <summary>
        /// 添加属性
        /// </summary>
        /// <param name="property"></param>
        void AddProperty(IUIProperty property)
        {
            propertyDictionary.Add(property.Name, property);
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="property"></param>
        void SetProperty(string name,string value)
        {
            propertyDictionary[name].Value = value;
        }

        public XElement ExportContentToXML()
        {
            XElement element = new XElement("PhysPolygon");
            ExportAttrbuteToXML(element);
            foreach (var item in listPoint)
            {
                XElement point = new XElement("Point");
                element.Add(new XAttribute("x", item.X));
                element.Add(new XAttribute("y", item.Y));
                element.Add(point);
            }
            return element;
        }

        void ExportAttrbuteToXML(XElement element)
        {
            foreach (var item in propertyDictionary)
            {
                if (!item.Value.IsDefault)
                {
                    element.Add(new XAttribute(item.Value.Name, item.Value.Value));
                }
            }
        }

        public void LoadContentFromXML(XElement element)
        {
            LoadAttrbuteFromXML(element);
            foreach (var item in element.Elements())
            {
                if (item.Name.ToString() == "Point")
                {
                    Point p=new Point();
                    p.X=double.Parse( item.Attribute("x").Value);
                    p.Y = double.Parse(item.Attribute("y").Value);
                    AddPoint(p);
                }
            }
        }

        void LoadAttrbuteFromXML(XElement element)
        {
            XAttribute xAttribute;
            foreach (var item in propertyDictionary)
            {
                xAttribute = element.Attribute(item.Key);
                if (xAttribute != null) { item.Value.Value = xAttribute.Value; xAttribute.Remove(); }
            }
        }
    }

    namespace PhysicsProperty
    {
        public class DesignWidth : IUIProperty
        {
            public DesignWidth() : base("designWidth", typeof(double), UIPropertyEnum.Transform, "碰撞体的设计宽度。", "200") { }
        }

        public class DesignHeight : IUIProperty
        {
            public DesignHeight() : base("designHeight", typeof(double), UIPropertyEnum.Transform, "碰撞体的设计高度。", "200") { }
        }
    }
}
