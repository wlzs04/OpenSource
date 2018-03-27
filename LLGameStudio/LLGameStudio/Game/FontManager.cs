using LLGameStudio.Common;
using LLGameStudio.Common.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLGameStudio.Game
{
    public struct LLFont
    {
        string name;
        string familyName;
        string color;
        double size;

        public string Name { get => name; set => name = value; }
        public string FamilyName { get => familyName; set => familyName = value; }
        public string Color { get => color; set => color = value; }
        public double Size { get => size; set => size = value; }

        public LLFont(string name, string familyName, string color, double size)
        {
            this.name = name;
            this.familyName = familyName;
            this.color = color;
            this.size = size;
        }
    }

    class FontManager
    {
        static FontConfig fontConfig;
        static string fontConfigPath="";

        /// <summary>
        /// 加载字体配置文件。
        /// </summary>
        /// <param name="fontConfigPath"></param>
        public static void LoadFontConfig(string fontConfigPath)
        {
            fontConfig = new FontConfig();
            FontManager.fontConfigPath = fontConfigPath;
            LLConvert.LoadContentFromXML(fontConfigPath, fontConfig);
        }

        /// <summary>
        /// 通过字体名称获得字体。
        /// </summary>
        /// <param name="fontName"></param>
        /// <returns></returns>
        public static LLFont GetFontByName(string fontName)
        {
            return fontConfig.GetFontByName(fontName);
        }

        public static Dictionary<string, LLFont> GetAllFont()
        {
            return fontConfig.dictionaryFont;
        }

        public static bool AddFont(LLFont font)
        {
            if (fontConfig.dictionaryFont.ContainsKey(font.Name))
            {
                return false;
            }
            else
            {
                fontConfig.dictionaryFont[font.Name] = font;
                return true;
            }
        }

        public static void EditFont(LLFont font)
        {
            fontConfig.dictionaryFont[font.Name] = font;
        }

        public static void RemoveFont(string name)
        {
            fontConfig.dictionaryFont.Remove(name);
        }

        public static void SaveFonts()
        {
            if(!string.IsNullOrEmpty(fontConfigPath))
            {
                LLConvert.ExportContentToXML(fontConfigPath, fontConfig);
            }
        }
    }
}
