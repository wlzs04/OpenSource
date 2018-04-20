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
    /// LLStudioKeyItem.xaml 的交互逻辑
    /// </summary>
    public partial class LLStudioKeyItem : UserControl
    {
        bool isSelect = false;
        string name;
        Dictionary<int, Rectangle> flagMap = new Dictionary<int, Rectangle>();
        LLStudioTimeline timeLine;
        object sender;

        public LLStudioKeyItem(LLStudioTimeline timeLine, string name,int level)
        {
            InitializeComponent();
            this.timeLine = timeLine;
            this.name = name;
            for (int i = 0; i < level; i++)
            {
                textBlockKeyName.Text += "    ";
            }
            textBlockKeyName.Text += name;
        }
        
        /// <summary>
        /// 设置关联物
        /// </summary>
        /// <param name="sender"></param>
        public void SetRelation(object sender)
        {
            this.sender = sender;
        }

        /// <summary>
        /// 获得关联物
        /// </summary>
        /// <returns></returns>
        public object GetRelation()
        {
            return sender;
        }

        /// <summary>
        /// 设置选中状态
        /// </summary>
        public void SetSelectState()
        {
            if(!isSelect)
            {
                isSelect = true;
                border.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF638774"));
            }
        }

        /// <summary>
        /// 取消选择状态
        /// </summary>
        public void CancelSelectState()
        {
            if (isSelect)
            {
                isSelect = false;
                border.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF404E5C"));
            }
        }

        /// <summary>
        /// 添加Key标记
        /// </summary>
        /// <param name="scale">在哪一个刻度上添加标记</param>
        /// <param name="x">在哪一个位置上添加标记</param>
        public void AddKeyFlag(int scale)
        {
            double x = timeLine.GetScalePositionX(scale);
            Rectangle rectangle = new Rectangle();
            rectangle.Width = timeLine.GetScaleSpace()*0.8;
            rectangle.Height = canvas.ActualHeight*0.8;
            rectangle.Fill = ThemeManager.GetBrushByName("keyFlagColor");
            rectangle.Margin = new Thickness(x - rectangle.Width/2, canvas.ActualHeight*0.1, 0, 0);
            canvas.Children.Add(rectangle);
            flagMap[scale] = rectangle;
        }

        /// <summary>
        /// 移除指定位置上的标记
        /// </summary>
        /// <param name="scale"></param>
        public void RemoveFlag(int scale)
        {
            if(flagMap.ContainsKey(scale))
            {
                canvas.Children.Remove(flagMap[scale]);
                flagMap.Remove(scale);
            }
        }

        /// <summary>
        /// 移除所有标记
        /// </summary>
        public void RemoveAllFlag()
        {
            canvas.Children.Clear();
            flagMap.Clear();
        }

        /// <summary>
        /// 返回某刻度下是否有标记
        /// </summary>
        /// <param name="scale"></param>
        /// <returns></returns>
        public bool HaveKeyFlag(int scale)
        {
            return flagMap.ContainsKey(scale);
        }

        /// <summary>
        /// 返回名称
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return name;
        }
    }
}
