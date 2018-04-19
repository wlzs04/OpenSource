using LLGameStudio.Common.XML;
using LLGameStudio.Game.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LLGameStudio.Game.Actor
{
    public class Frame : IXMLClass
    {
        public Dictionary<string, IUIProperty> propertyDictionary = new Dictionary<string, IUIProperty>();
        public FrameProperty.FrameNumber frameNumber = new FrameProperty.FrameNumber();
        public List<Bone> listBone = new List<Bone>();

        public Frame()
        {
            AddProperty(frameNumber);
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
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetProperty(string name, string value)
        {
            propertyDictionary[name].Value = value;
        }

        public XElement ExportContentToXML()
        {
            XElement element = new XElement("Frame");
            ExportAttrbuteToXML(element);
            foreach (var item in listBone)
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

        public void LoadContentFromXML(XElement element)
        {
            LoadAttrbuteFromXML(element);
            foreach (var item in element.Elements())
            {
                if (item.Name.ToString() == "Bone")
                {
                    Bone bone = new Bone();
                    bone.LoadContentFromXML(item);
                    listBone.Add(bone);
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

    namespace FrameProperty
    {
        public class FrameNumber : IUIProperty
        {
            public FrameNumber() : base("frameNumber", typeof(int), UIPropertyEnum.Common, "代表第几帧。", "0") { }
        }
    }
}
