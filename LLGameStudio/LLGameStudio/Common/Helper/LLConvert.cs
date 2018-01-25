using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLGameStudio.Common
{
    public static class LLConvert
    {
        public static object ChangeType(object value, Type type)
        {
            if(type.IsEnum)
            {
                return Enum.Parse(type, value.ToString());
            }
            return Convert.ChangeType(value, type);
        }
    }
}
