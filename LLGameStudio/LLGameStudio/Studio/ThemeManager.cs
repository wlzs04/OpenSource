using LLGameStudio.Common;
using LLGameStudio.Common.Config;
using LLGameStudio.Common.XML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Linq;

namespace LLGameStudio.Studio
{
    class ThemeManager
    {
        static string themeConfigFilePath = @"Config\Theme.xml";
        static Dictionary<string, Brush> dictionaryNameBrush;
        static ThemeConfig themeConfig;

        /// <summary>
        /// 加载编辑器主题。
        /// </summary>
        /// <param name="themeName"></param>
        public static void LoadTheme(string themeName)
        {
            themeConfig = new ThemeConfig();
            LLConvert.LoadContentFromXML(themeConfigFilePath, themeConfig);
            dictionaryNameBrush = new Dictionary<string, Brush>();
        }

        /// <summary>
        /// 通过画刷名字获得画刷。
        /// </summary>
        /// <param name="brushName"></param>
        /// <returns></returns>
        public static Brush GetBrushByName(string brushName)
        {
            if (!dictionaryNameBrush.ContainsKey(brushName))
            {
                Color color = (Color)ColorConverter.ConvertFromString(themeConfig.GetColorValueByName(brushName));
                dictionaryNameBrush.Add(brushName, new SolidColorBrush(color));
            }
            return dictionaryNameBrush[brushName];
        }
    }
}
