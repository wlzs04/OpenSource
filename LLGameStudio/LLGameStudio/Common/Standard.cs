using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLGameStudio.Common
{
    /// <summary>
    /// 标准类，用来适配DPI不同导致窗体选中位置不同的问题，不知道是否有其他解决办法。
    /// </summary>
    class Standard
    {
        public static float StandardDpiX = 96;
        public static float StandardDpiY = 96;

        public static float ScaleX = 1;
        public static float ScaleY = 1;
    }
}
