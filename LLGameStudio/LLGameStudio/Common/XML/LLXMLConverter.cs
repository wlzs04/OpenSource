using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace LLGameStudio.Common.XML
{
    class LLXMLConverter
    {
        public static void LoadContentFromXML(string filepath,IXMLClass iXMLClass)
        {
            XDocument doc = XDocument.Load(filepath);
            XElement root = doc.Root;
            
            iXMLClass.LoadContentFromXML(root);
        }

        public static void ExportContentToXML(string filepath, IXMLClass iXMLClass)
        {
            XDocument doc = new XDocument();
            XElement root = iXMLClass.ExportContentToXML();
            doc.Add(root);
            doc.Save(filepath);
        }
    }
}
