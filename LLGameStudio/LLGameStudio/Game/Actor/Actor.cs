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
                else if(item.Name.ToString() == "Actions")
                {
                    LoadActionFromXML(item);
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
    }
}
