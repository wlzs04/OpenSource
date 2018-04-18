using LLGameStudio.Common.DataType;
using LLGameStudio.Common.Helper;
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
        double boneControlAngle = 0;//相对于画布的旋转角度，用于显示。
        public LLStudioBone parentBoneControl = null;
        public List<LLStudioBone> listBoneControl = new List<LLStudioBone>();

        public LLStudioBone(Bone bone)
        {
            InitializeComponent();
            this.bone = bone;
            SetBoneLength(bone.length);
        }

        /// <summary>
        /// 添加骨骼
        /// </summary>
        /// <param name="boneControl"></param>
        public void AddBoneControl(LLStudioBone boneControl)
        {
            listBoneControl.Add(boneControl);
            boneControl.parentBoneControl = this;
            bone.AddBone(boneControl.bone);
        }

        /// <summary>
        /// 以给定坐标为骨骼节点中心设置位置，默认骨骼方向向下。
        /// </summary>
        public void SetPostion(double x,double y)
        {
            boneStartPosition.X = x;
            boneStartPosition.Y = y;
            Margin = new Thickness(x- ellipseBoneJoint.ActualWidth / 2, y - ellipseBoneJoint.ActualHeight / 2, 0, 0);
            foreach (var item in listBoneControl)
            {
                item.ChangeTransformByParent();
            }
        }

        /// <summary>
        /// 以给定坐标为骨骼节点中心设置位置，默认骨骼方向向下。
        /// </summary>
        public void SetPostion(Point point)
        {
            SetPostion(point.X, point.Y);
        }

        /// <summary>
        /// 获得显示的真实的角度
        /// </summary>
        /// <returns></returns>
        public double GetBoneControlAngle()
        {
            return boneControlAngle;
        }

        /// <summary>
        /// 获得骨骼自身的角度
        /// </summary>
        /// <returns></returns>
        public double GetBoneAngle()
        {
            return bone.angle;
        }

        /// <summary>
        /// 设置骨骼自身的旋转角度
        /// </summary>
        /// <param name="angle"></param>
        public void SetBoneAngle(double angle)
        {
            bone.angle = angle;
            double parentBoneControlAngle =
                parentBoneControl != null? 
                parentBoneControl.GetBoneControlAngle(): 0;

            boneControlAngle = bone.angle + parentBoneControlAngle;

            RenderTransform = new RotateTransform(boneControlAngle * 180 / Math.PI);
            foreach (var item in listBoneControl)
            {
                item.ChangeTransformByParent();
            }
        }

        /// <summary>
        /// 获得骨骼开始节点位置。
        /// </summary>
        /// <returns></returns>
        public Point GetStartPoint()
        {
            return boneStartPosition;
        }

        /// <summary>
        /// 获得骨骼尾节点位置。
        /// </summary>
        /// <returns></returns>
        public Point GetBoneEndPosition()
        {
            double x = bone.length * Math.Sin(boneControlAngle);
            double y = bone.length * Math.Cos(boneControlAngle);
            Point p = new Point();
            p.X = boneStartPosition.X - x;
            p.Y = boneStartPosition.Y + y;
            return p;
        }

        /// <summary>
        /// 获得骨骼长度。
        /// </summary>
        /// <returns></returns>
        public double GetBoneLength()
        {
            return bone.length;
        }

        /// <summary>
        /// 更改骨骼长度时调整控件中心点等信息，仅类内部使用。
        /// </summary>
        /// <param name="length"></param>
        void SetBoneLength(double length)
        {
            bone.length = length;
            polygonBone.Points[2] = new Point(10, bone.length);
            RenderTransformOrigin = new Point(0.5, 10 / (10 + bone.length));
        }

        /// <summary>
        /// 由父骨骼调用，通过传递自身位置坐标和旋转度数让子骨骼变换。
        /// </summary>
        public void ChangeTransformByParent()
        {
            double parentBoneControlAngle =
                parentBoneControl != null ?
                parentBoneControl.GetBoneControlAngle() : 0;

            boneControlAngle = bone.angle + parentBoneControlAngle;
            SetPostion(parentBoneControl.GetBoneEndPosition());
            RenderTransform = new RotateTransform(boneControlAngle * 180 / Math.PI);
        }

        /// <summary>
        /// 由子骨骼调用，通过传递子骨骼位置坐标让父骨骼变换
        /// </summary>
        /// <param name="childBonePoint"></param>
        public void ChangeTransformByChildBone(Point childBonePoint)
        {
            SetBoneLength(LLMath.GetPointsLength(childBonePoint, boneStartPosition));
            
            Vector2 boneDirction = LLMath.GetNormalVector2( new Vector2(childBonePoint.X- boneStartPosition.X, childBonePoint.Y - boneStartPosition.Y));
            boneControlAngle = LLMath.GetAngleBetweenVectors(new Vector2(0, 1), boneDirction);
            
            ResetBoneAngleByBoneControlAngle();
            RenderTransform = new RotateTransform(boneControlAngle * 180 / Math.PI);
        }

        /// <summary>
        /// 通过骨骼显示的旋转角度和父骨骼旋转角度计算骨骼旋转角度
        /// </summary>
        public void ResetBoneAngleByBoneControlAngle()
        {
            double parentBoneControlAngle = parentBoneControl != null ?
                parentBoneControl.GetBoneControlAngle() : 0;
            bone.angle = boneControlAngle - parentBoneControlAngle;
        }

        /// <summary>
        /// 通过自身的旋转角度和父骨骼旋转角度计算显示的旋转角度
        /// </summary>
        public void ResetBoneControlAngleByBoneAngle()
        {
            boneControlAngle = bone.angle;
            RenderTransform = new RotateTransform(boneControlAngle * 180 / Math.PI);
        }

        /// <summary>
        /// 设置选中状态
        /// </summary>
        public void SetSelectState()
        {
            polygonBone.Stroke = new SolidColorBrush(Colors.Green);
        }

        /// <summary>
        /// 取消选中状态
        /// </summary>
        public void CancelSelectState()
        {
            polygonBone.Stroke = null;
        }
    }
}
