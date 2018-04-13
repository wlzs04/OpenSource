using LLGameStudio.Common.DataType;
using LLGameStudio.Common.XML;
using LLGameStudio.Game.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace LLGameStudio.Game.Actor
{
    public class Bone : IXMLClass
    {
        public Bone parentBone = null;
        public List<Bone> listBone = new List<Bone>();

        public Dictionary<string, IUIProperty> propertyDictionary = new Dictionary<string, IUIProperty>();
        public Property.Name name = new Property.Name();
        public Property.Length length = new Property.Length();
        public Property.Angle angle = new Property.Angle();


        public Bone()
        {
            AddProperty(name);
            AddProperty(length);
            AddProperty(angle);
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

        /// <summary>
        /// 添加子骨骼
        /// </summary>
        /// <param name="bone"></param>
        public void AddBone(Bone bone)
        {
            listBone.Add(bone);
            bone.parentBone = this;
        }

        /// <summary>
        /// 设置父骨骼。
        /// </summary>
        /// <param name="bone"></param>
        public void SetParentBone(Bone bone)
        {
            parentBone = bone;
        }

        public XElement ExportContentToXML()
        {
            XElement element = new XElement("Bone");
            ExportAttrbuteToXML(element);
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
            foreach (var item in listBone)
            {
                element.Add(item.ExportContentToXML());
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
                    AddBone(bone);
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

    namespace Property
    {
        public class Length : IUIProperty
        {
            public Length() : base("length", typeof(double), UIPropertyEnum.Transform, "骨骼长度。", "90") { }
        }

        public class Angle : IUIProperty
        {
            public Angle() : base("angle", typeof(double), UIPropertyEnum.Transform, "骨骼旋转角度。", "0") { }
        }

        public class Name : IUIProperty
        {
            public Name() : base("name", typeof(string), UIPropertyEnum.Common, "骨骼名称。", "bone") { }
        }
    }
}
