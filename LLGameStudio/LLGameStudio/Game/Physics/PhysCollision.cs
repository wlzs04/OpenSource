using LLGameStudio.Common.XML;
using LLGameStudio.Game.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LLGameStudio.Game.Physics
{
    class PhysCollision : IXMLClass
    {
        public Dictionary<string, IUIProperty> propertyDictionary = new Dictionary<string, IUIProperty>();
        public PhysicsProperty.DesignWidth designWidth = new PhysicsProperty.DesignWidth();
        public PhysicsProperty.DesignHeight designHeight = new PhysicsProperty.DesignHeight();
        
        public List<PhysPolygon> physPolygonList = new List<PhysPolygon>();

        public PhysCollision()
        {
            AddProperty(designWidth);
            AddProperty(designHeight);
        }

        /// <summary>
        /// 向碰撞体中添加多边形
        /// </summary>
        /// <param name="polygon"></param>
        public void AddPolygon(PhysPolygon polygon)
        {
            physPolygonList.Add(polygon);
        }

        /// <summary>
        /// 从碰撞体中移除指定多边形
        /// </summary>
        /// <param name="polygon"></param>
        public void RemovePolygon(PhysPolygon polygon)
        {
            physPolygonList.Remove(polygon);
        }

        /// <summary>
        /// 清空碰撞体中的多边形
        /// </summary>
        public void RemoveAllPolygon()
        {
            physPolygonList.Clear();
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
        public void SetProperty(string name, string value)
        {
            propertyDictionary[name].Value = value;
        }

        public void LoadContentFromXML(XElement element)
        {
            LoadAttrbuteFromXML(element);
            foreach (var item in element.Elements())
            {
                if (item.Name.ToString() == "PhysPolygon")
                {
                    PhysPolygon polygon = new PhysPolygon();
                    polygon.LoadContentFromXML(item);
                    AddPolygon(polygon);
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

        public XElement ExportContentToXML()
        {
            XElement element = new XElement("PhysCollision");
            ExportAttrbuteToXML(element);

            foreach (var item in physPolygonList)
            {
                element.Add(item.ExportContentToXML());
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
    }
}
