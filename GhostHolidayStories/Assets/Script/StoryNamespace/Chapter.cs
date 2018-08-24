using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Assets.Script.StoryNamespace
{
    class Chapter
    {
        string chapterPath;
        int index;
        string name;
        string description = "";

        List<Section> sectionList = new List<Section>();

        public Chapter(string chapterPath, int index)
        {
            this.chapterPath = chapterPath;
            this.index = index;
        }

        public void LoadContent()
        {
            XDocument doc = XDocument.Load(chapterPath);
            XElement root = doc.Root;

            foreach (var attribute in root.Attributes())
            {
                switch (attribute.Name.ToString())
                {
                    case "name":
                        name = attribute.Value;
                        break;
                    case "description":
                        description = attribute.Value;
                        break;
                    default:
                        break;
                }
            }

            foreach (var item in root.Elements())
            {
                if (item.Name == "Section")
                {
                    Section section = new Section();
                    section.LoadContent(item);
                    sectionList.Add(section);
                }
            }
        }

        public string GetName()
        {
            return name;
        }

        public string GetDescription()
        {
            return description;
        }
    }
}
