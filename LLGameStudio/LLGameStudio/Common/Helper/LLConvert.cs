using LLGameStudio.Common.XML;
using LLGameStudio.Game.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Linq;

namespace LLGameStudio.Common
{
    /// <summary>
    /// 自定义类型转换器，可以转换基础类型、枚举类型和
    /// 实现了IConvertStringClass接口的类，还可以
    /// 用于与XML文件交互,可以将XML内容和实现了IXMLClass接口的类进行加载或导出。
    /// </summary>
    class LLConvert
    {
        public static object ChangeType(object value, Type type)
        {
            if(type.IsEnum)
            {
                return Enum.Parse(type, value.ToString());
            }
            if(type==typeof(LLGameStudio.Game.UI.Property.Rotation))
            if(type.GetInterface("IConvertStringClass")!=null)
            {
                IConvertStringClass o = (IConvertStringClass)Activator.CreateInstance(type);
                o.GetValueFromString(value.ToString());
                return o;
            }
            return Convert.ChangeType(value, type);
        }
        
        /// <summary>
        /// 将string转换为Vector，要求string格式为“{x,y}”，例“{1,1}”
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Vector StringToVector(string s)
        {
            Vector v = new Vector();
            if(s.Length>4)
            {
                s.Remove(0, 1);
                s.Remove(s.Length - 1, 1);
                string[] sarray=s.Split(',');
                if(sarray.Length>1)
                {
                    v.X = Convert.ToInt32(sarray[0]);
                    v.Y = Convert.ToInt32(sarray[1]);
                }
            }
            return v;
        }

        /// <summary>
        /// 将XML内容加载到类中
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="iXMLClass"></param>
        public static void LoadContentFromXML(string filepath, IXMLClass iXMLClass)
        {
            XDocument doc = XDocument.Load(filepath);
            XElement root = doc.Root;

            iXMLClass.LoadContentFromXML(root);
        }

        /// <summary>
        /// 导出类内容到XML文件中
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="iXMLClass"></param>
        public static void ExportContentToXML(string filepath, IXMLClass iXMLClass)
        {
            XDocument doc = new XDocument();
            XElement root = iXMLClass.ExportContentToXML();
            doc.Add(root);
            doc.Save(filepath);
        }
    }
}
