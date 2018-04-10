using LLGameStudio.Common.XML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LLGameStudio.Game.Actor
{
    class Bone:IXMLClass
    {
        public string name="";
        public List<Bone> listBone = new List<Bone>();

        public Bone(string name)
        {
            this.name = name;
        }

        public void AddBone(Bone bone)
        {
            listBone.Add(bone);
        }

        public XElement ExportContentToXML()
        {
            throw new NotImplementedException();
        }

        public void LoadContentFromXML(XElement element)
        {
            LoadAttrbuteFromXML(element);
            foreach (var item in element.Elements())
            {
                if (item.Name.ToString() == "Bone")
                {
                    Bone bone = new Bone("temp");
                    bone.LoadContentFromXML(item);
                    AddBone(bone);
                }
            }
        }

        void LoadAttrbuteFromXML(XElement element)
        {
            XAttribute xAttribute;
            xAttribute = element.Attribute("name");
            if (xAttribute != null)
            {
                name = xAttribute.Value;
            }
            
        }
    }
}
