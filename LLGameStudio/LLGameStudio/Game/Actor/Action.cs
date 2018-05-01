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

        public SortedList<int,Frame> sortedListFrame = new SortedList<int,Frame>();

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
            foreach (var item in sortedListFrame)
            {
                element.Add(item.Value.ExportContentToXML());
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
                    sortedListFrame.Add(frame.frameNumber.Value,frame);
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

        /// <summary>
        /// 获得指定帧的内容
        /// </summary>
        /// <param name="frameNumber"></param>
        /// <returns></returns>
        public Frame GetFrameByNumber(int frameNumber)
        {
            if (sortedListFrame.ContainsKey(frameNumber))
            {
                return sortedListFrame[frameNumber];
            }
            return null;
        }

        /// <summary>
        /// 获得指定帧前一帧的内容
        /// </summary>
        /// <param name="frameNumber"></param>
        /// <returns></returns>
        Frame GetPreFrameByNumber(int frameNumber)
        {
            int index = -1;
            foreach (var item in sortedListFrame.Keys)
            {
                if (item < frameNumber)
                {
                    index = item;
                }
                else
                {
                    break;
                }
            }

            if(index>-1)
            {
                return sortedListFrame[index];
            }
            return null;
        }

        /// <summary>
        /// 获得指定帧后一帧的内容
        /// </summary>
        /// <param name="frameNumber"></param>
        /// <returns></returns>
        Frame GetNextFrameByNumber(int frameNumber)
        {
            int index = -1;
            foreach (var item in sortedListFrame.Keys)
            {
                if (item > frameNumber)
                {
                    index = item;
                    break;
                }
            }

            if (index > -1)
            {
                return sortedListFrame[index];
            }
            return null;
        }

        /// <summary>
        /// 通过时间计算,骨骼的实际状态，以帧的形式返回
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public Frame CalculateFrameByTime(double time)
        {
            lock (this)
            {
                int preFrameNumber = (int)Math.Floor(time / totalTime.Value * totalFrameNumber.Value);
                int nextFrameNumber = (int)Math.Ceiling(time / totalTime.Value * totalFrameNumber.Value);

                Frame preFrame = GetFrameByNumber(preFrameNumber);
                if (preFrame == null)
                {
                    preFrame = GetPreFrameByNumber(preFrameNumber);
                }

                if (preFrameNumber != nextFrameNumber)
                {
                    Frame nextFrame = GetFrameByNumber(nextFrameNumber);
                    if (nextFrame == null)
                    {
                        nextFrame = GetNextFrameByNumber(nextFrameNumber);
                    }
                    Frame frame = new Frame();
                    if (preFrame != null && nextFrame != null)
                    {
                        double preFrameTime = totalTime.Value * preFrame.frameNumber.Value / totalFrameNumber.Value;
                        double nextFrameTime = totalTime.Value * nextFrame.frameNumber.Value / totalFrameNumber.Value;
                        double preRate = 1 - (time - preFrameTime) / (nextFrameTime - preFrameTime);
                        double nextRate = 1 - (nextFrameTime - time) / (nextFrameTime - preFrameTime);

                        for (int i = 0; i < preFrame.listBone.Count; i++)
                        {
                            Bone bone = new Bone();
                            bone.name.Value = preFrame.listBone[i].name.Value;
                            bone.angle = (preRate * preFrame.listBone[i].angle + nextRate * nextFrame.listBone[i].angle);
                            frame.listBone.Add(bone);
                        }
                    }
                    return frame;
                }
                else
                {
                    return preFrame;
                }
            }
        }

        /// <summary>
        /// 通过帧数计算,骨骼的实际状态，以帧的形式返回
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public Frame CalculateFrameByFrameNumber(int frameNumber)
        {
            Frame preFrame = GetPreFrameByNumber(frameNumber);
            Frame nextFrame = GetNextFrameByNumber(frameNumber);

            Frame frame = new Frame();

            if(preFrame!=null && nextFrame!=null)
            {
                double preFrameNumber = preFrame.frameNumber.Value;
                double nextFrameNumber = nextFrame.frameNumber.Value;
                double preRate = 1-(frameNumber - preFrameNumber)/(nextFrameNumber - preFrameNumber);
                double nextRate = 1-(nextFrameNumber - frameNumber) / (nextFrameNumber - preFrameNumber);

                for (int i = 0; i < preFrame.listBone.Count; i++)
                {
                    Bone bone = new Bone();
                    bone.name.Value = preFrame.listBone[i].name.Value;
                    bone.angle = (preRate*preFrame.listBone[i].angle + nextRate*nextFrame.listBone[i].angle);
                    frame.listBone.Add(bone);
                }
            }

            return frame;
        }

        public void AddFrame(Frame frame)
        {
            sortedListFrame[frame.frameNumber.Value] = frame;
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
