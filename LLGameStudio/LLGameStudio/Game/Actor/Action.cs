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
    public class Action : IXMLClass
    {
        public Dictionary<string, IUIProperty> propertyDictionary = new Dictionary<string, IUIProperty>();
        public ActionProperty.Name name = new ActionProperty.Name();
        public ActionProperty.TotalFrameNumber totalFrameNumber = new ActionProperty.TotalFrameNumber();
        public ActionProperty.TotalTime totalTime = new ActionProperty.TotalTime();

        public List<Frame> listFrame = new List<Frame>();

        public Action()
        {
            AddProperty(name);
            AddProperty(totalFrameNumber);
            AddProperty(totalTime);
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
            XElement element = new XElement("Action");
            ExportAttrbuteToXML(element);
            foreach (var item in listFrame)
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
                if (item.Name.ToString() == "Frame")
                {
                    Frame frame = new Frame();
                    frame.LoadContentFromXML(item);
                    listFrame.Add(frame);
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

        public Frame GetFrameByNumber(int frameNumber)
        {
            foreach (var item in listFrame)
            {
                if(item.frameNumber.Value== frameNumber)
                {
                    return item;
                }
            }
            return null;
        }
    }

    namespace ActionProperty
    {
        public class Name : IUIProperty
        {
            public Name() : base("name", typeof(string), UIPropertyEnum.Common, "动作名称。", "action") { }
        }

        public class TotalFrameNumber : IUIProperty
        {
            public TotalFrameNumber() : base("totalFrameNumber", typeof(int), UIPropertyEnum.Common, "总帧数。", "24") { }
        }

        public class TotalTime : IUIProperty
        {
            public TotalTime() : base("totalTime", typeof(double), UIPropertyEnum.Common, "总时间（秒）。", "4") { }
        }
    }
}
