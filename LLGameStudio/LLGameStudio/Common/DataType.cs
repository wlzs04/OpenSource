﻿using LLGameStudio.Game.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LLGameStudio.Common
{
    namespace DataType
    {
        /// <summary>
        /// 二维向量，可以与字符串相互转换，格式为：“{x,y}”,例：“{2.4,5}”。
        /// </summary>
        public struct Vector2 : IConvertStringClass
        {
            public Vector2(double x,double y) { this.x = x; this.y = y; }
            double x,y;

            public double X { get => x; set => x = value; }
            public double Y { get => y; set => y = value; }

            public void FromString(string s)
            {
                if (s.Length > 4)
                {
                    s = s.Remove(0, 1);
                    s = s.Remove(s.Length - 1, 1);
                    string[] sarray = s.Split(',');
                    if (sarray.Length > 1)
                    {
                        x = Convert.ToInt32(sarray[0]);
                        y = Convert.ToInt32(sarray[1]);
                    }
                }
            }

            public override string ToString()
            {
                return "{"+x+","+y+"}";
            }
        }

        /// <summary>
        /// 矩形，可以与字符串相互转换，格式为：“{left,top,right,down}”,例：“{0}”,“{0,0}”，“{0,0,0,0}”。
        /// </summary>
        public struct Rect : IConvertStringClass
        {
            double left, top, right, bottom;

            public double Left { get => left; set => left = value; }
            public double Top { get => top; set => top = value; }
            public double Right { get => right; set => right = value; }
            public double Bottom { get => bottom; set => bottom = value; }

            public void FromString(string s)
            {
                if (s.Length > 2)
                {
                    s = s.Remove(0, 1);
                    s = s.Remove(s.Length - 1, 1);
                    string[] sarray = s.Split(',');
                    if (sarray.Length > 3)
                    {
                        left = Convert.ToDouble(sarray[0]);
                        top = Convert.ToDouble(sarray[1]);
                        right = Convert.ToDouble(sarray[2]);
                        bottom = Convert.ToDouble(sarray[3]);
                    }
                    else if (sarray.Length == 2)
                    {
                        left = Convert.ToDouble(sarray[0]);
                        top = Convert.ToDouble(sarray[1]);
                        right = left;
                        bottom = top;
                    }
                    else if(sarray.Length==1)
                    {
                        left = Convert.ToDouble(sarray[0]);
                        top = left;
                        right = left;
                        bottom = left;
                    }
                }
            }

            public override string ToString()
            {
                if(left== top&& left== right && left == bottom)
                {
                    return "{" + left + "}";
                }
                else if(left == right && top == bottom)
                {
                    return "{" + left + "," + top + "}";
                }
                return "{" + left + "," + top + "," + right + "," + bottom + "}";
            }
        }
    }
}
