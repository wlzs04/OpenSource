using LLGameStudio.Common.XML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LLGameStudio.Game.Actor
{
    class Actor : IXMLClass
    {
        public string name = "";
        public Bone rootBone;
        public List<Action> listAction = new List<Action>();
        public Dictionary<Bone,Bone> ikMap=new Dictionary<Bone, Bone>();

        public Actor(string name)
        {
            this.name = name;
        }

        public void LoadContentFromXML(XElement element)
        {
            foreach (var item in element.Elements())
            {
                if (item.Name.ToString() == "Bone")
                {
                    Bone bone = new Bone();
                    bone.LoadContentFromXML(item);
                    SetRootBone(bone);
                }
                else if(item.Name.ToString() == "IKs")
                {
                    LoadIKsFromXML(item);
                }
                else if(item.Name.ToString() == "Actions")
                {
                    LoadActionFromXML(item);
                }
            }
        }

        public void LoadIKsFromXML(XElement element)
        {
            foreach (var item in element.Elements())
            {
                if (item.Name.ToString() == "IK")
                {
                    XAttribute attributeEndBone= item.Attribute("endBone");
                    XAttribute attributeStartBone = item.Attribute("startBone");
                    
                    ikMap[GetBoneByName(rootBone,attributeEndBone.Value)] = GetBoneByName(rootBone, attributeStartBone.Value);
                }
            }
        }

        public void LoadActionFromXML(XElement element)
        {
            foreach (var item in element.Elements())
            {
                if (item.Name.ToString() == "Action")
                {
                    Action action = new Action();
                    action.LoadContentFromXML(item);
                    AddAction(action);
                }
            }
        }

        public XElement ExportContentToXML()
        {
            XElement element = new XElement("Actor");
            ExportAttrbuteToXML(element);
            element.Add(rootBone.ExportContentToXML());
            XElement iksElement = new XElement("IKs");
            foreach (var item in ikMap)
            {
                XElement ikElement = new XElement("IK");
                ikElement.Add(new XAttribute("endBone", item.Key.name.Value));
                ikElement.Add(new XAttribute("startBone", item.Value.name.Value));
                iksElement.Add(ikElement);
            }
            element.Add(iksElement);
            XElement actionsElement = new XElement("Actions");
            foreach (var item in listAction)
            {
                actionsElement.Add(item.ExportContentToXML());
            }
            element.Add(actionsElement);
            return element;
        }

        void ExportAttrbuteToXML(XElement element)
        {
        }

        public void SetRootBone(Bone bone)
        {
            rootBone = bone;
        }

        public void SetDefaultPosture()
        {
            rootBone.SetDefaultPosture();
        }

        public void AddAction(Action action)
        {
            listAction.Add(action);
        }

        /// <summary>
        /// 返回当前角色的骨骼数量
        /// </summary>
        public int GetBoneNumber()
        {
            return 1;
        }

        public Bone GetBoneByName(Bone bone,string name)
        {
            if(bone.name.Value==name)
            {
                return bone;
            }
            foreach (var item in bone.listBone)
            {
                Bone childBone = null;
                childBone = GetBoneByName(item, name);
                if (childBone!=null)
                {
                    return childBone;
                }
            }
            return null;
        }

        public void AddIK(Bone endBone,Bone startBone)
        {
            ikMap[endBone] = startBone;
        }

        public void RemoveIK(Bone endBone)
        {
            if (ikMap.ContainsKey(endBone))
            {
                ikMap.Remove(endBone);
            }
        }
    }
}
