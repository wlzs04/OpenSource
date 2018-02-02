using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLGameStudio.Common.Helper
{
    class LLMath
    {
        /// <summary>
        /// 数值1，比1大一点，证明不按父窗体比例。
        /// </summary>
        public static double One = 1.0001;
        
        /// <summary>
        /// 判断参数是否在1和0之间，包括1不包括0。
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static bool IsRange1To0(double d)
        {
            return d>0&&d<=1;
        }
    }
}
