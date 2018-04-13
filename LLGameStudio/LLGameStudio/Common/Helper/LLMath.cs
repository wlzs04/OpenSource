using LLGameStudio.Common.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

        /// <summary>
        /// 获得两点之间的距离
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        public static double GetPointsLength(Point point1,Point point2)
        {
            return Math.Sqrt((point2.X - point1.X) * (point2.X - point1.X) + (point2.Y - point1.Y) * (point2.Y - point1.Y));
        }

        /// <summary>
        /// 获得标准化向量
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vector2 GetNormalVector2(Vector2 vector)
        {
            double length = GetVector2Length(vector);
            return new Vector2(vector.X/ length, vector.Y / length);
        }

        /// <summary>
        /// 获得向量长度
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static double GetVector2Length(Vector2 vector)
        {
            return Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
        }
        
        /// <summary>
        /// 获得向量之间夹角，顺时针时返回正值，向量参数需要提前标准化。
        /// </summary>
        /// <param name="vector1"></param>
        /// <param name="vector2"></param>
        /// <returns></returns>
        public static double GetAngleBetweenVectors(Vector2 vector1, Vector2 vector2)
        {
            double length1 = GetVector2Length(vector1);
            double length2 = GetVector2Length(vector2);
            double cross = vector1.X * vector2.X + vector1.Y * vector2.Y;
            return Math.Atan2(vector2.Y, vector2.X) - Math.Atan2(vector1.Y, vector1.X);
        }

        /// <summary>
        /// 获得两点之间的向量。
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        public static Vector2 GetVector2BetweenPoints(Point point1, Point point2)
        {
            return new Vector2(point2.X - point1.X, point2.Y - point1.Y);
        }
    }
}
