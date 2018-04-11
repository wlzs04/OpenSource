using LLGameStudio.Common.XML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LLGameStudio.Game.Actor
{
    class Actor:IXMLClass
    {
        public string name="";
        public Bone rootBone;

        public Actor(string name)
        {
            this.name = name;
            SetRootBone(new Bone("rootBone"));
        }

        public void LoadContentFromXML(XElement element)
        {
            foreach (var item in element.Elements())
            {
                if (item.Name.ToString() == "Bone")
                {
                    Bone bone = new Bone("temp");
                    bone.LoadContentFromXML(item);
                    SetRootBone(bone);
                }
            }
        }

        public XElement ExportContentToXML()
        {
            throw new NotImplementedException();
        }

        public void SetRootBone(Bone bone)
        {
            rootBone = bone;
        }
    }
}
