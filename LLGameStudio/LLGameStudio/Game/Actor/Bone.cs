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
        public BoneProperty.Name name = new BoneProperty.Name();
        BoneProperty.Length defaultLength = new BoneProperty.Length();
        BoneProperty.Angle defaultAngle = new BoneProperty.Angle();

        public double length = 90;
        public double angle = 0;

        public Bone()
        {
            AddProperty(name);
            AddProperty(defaultLength);
            AddProperty(defaultAngle);
        }

        /// <summary>
        /// 将当前骨骼及其子骨骼的长度和角度信息设为默认值。
        /// </summary>
        public void SetDefaultPosture()
        {
            defaultLength.Value = length;
            defaultAngle.Value = angle;
            foreach (var item in listBone)
            {
                item.SetDefaultPosture();
            }
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

        /// <summary>
        /// 仅用于记录骨骼动作使用，不能使用此方法设置骨骼真实内容。
        /// </summary>
        /// <param name="bone"></param>
        public void RecordActionByBone(Bone bone)
        {
            name.Value = bone.name.Value;
            defaultAngle.Value = bone.angle;
            angle = bone.angle;
        }

        public XElement ExportContentToXML()
        {
            XElement element = new XElement("Bone");
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
            length = defaultLength.Value;
            angle = defaultAngle.Value;
        }
    }

    namespace BoneProperty
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
