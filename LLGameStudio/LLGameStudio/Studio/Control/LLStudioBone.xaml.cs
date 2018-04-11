using LLGameStudio.Game.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LLGameStudio.Studio.Control
{
    /// <summary>
    /// LLStudioBone.xaml 的交互逻辑
    /// </summary>
    public partial class LLStudioBone : UserControl
    {
        public Bone bone;
        Point boneStartPosition;
        double boneLength = 100;

        public LLStudioBone(Bone bone)
        {
            InitializeComponent();
            this.bone = bone;
        }

        /// <summary>
        /// 以给定坐标为骨骼节点中心设置位置，默认骨骼方向向下。
        /// </summary>
        public void SetPostion(double x,double y)
        {
            boneStartPosition.X = x;
            boneStartPosition.Y = y;
            Margin = new Thickness(x- ellipseBoneJoint.ActualWidth / 2, y - ellipseBoneJoint.ActualHeight / 2, 0, 0);
        }

        /// <summary>
        /// 以给定坐标为骨骼节点中心设置位置，默认骨骼方向向下。
        /// </summary>
        public void SetPostion(Point point)
        {
            boneStartPosition = point;
            Margin = new Thickness(point.X - ellipseBoneJoint.ActualWidth / 2, point.Y - ellipseBoneJoint.ActualHeight / 2, 0, 0);
        }

        public Point GetStartPoint()
        {
            return boneStartPosition;
        }

        public Point GetBoneEndPosition()
        {
            Point p = new Point();
            p.X = boneStartPosition.X;
            p.Y = boneStartPosition.Y+ boneLength;
            return p;
        }

        /// <summary>
        /// 设置骨骼长度
        /// </summary>
        /// <param name="d"></param>
        public void SetBoneLength(double d)
        {
            polygonBone.Points[2] = new Point(10,d);
            boneLength = d;
        }
    }
}
