using LLGameStudio.Common.DataType;
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
        /// <summary>
        /// 将value值转换为指定类型。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object ChangeType(object value, Type type)
        {
            if(type.IsEnum)
            {
                return Enum.Parse(type, value.ToString());
            }
            if(typeof(IConvertStringClass).IsAssignableFrom(type))
            {
                IConvertStringClass o = (IConvertStringClass)Activator.CreateInstance(type);
                o.FromString(value.ToString());
                return o;
            }
            return Convert.ChangeType(value, type);
        }
        
        /// <summary>
        /// 将string转换为Vector，要求string格式为“{x,y}”，例“{1,1}”
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Vector2 StringToVector2(string s)
        {
            Vector2 v = new Vector2();
            v.FromString(s);
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
        public static void ExportContentToXML(string filePath, IXMLClass iXMLClass)
        {
            XDocument doc = new XDocument();
            XElement root = iXMLClass.ExportContentToXML();
            doc.Add(root);
            doc.Save(filePath);
        }
    }
}
