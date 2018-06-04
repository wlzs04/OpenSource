using LLGameStudio.Common;
using LLGameStudio.Common.DataType;
using LLGameStudio.Game.Physics;
using LLGameStudio.Game.UI;
using LLGameStudio.Studio.Control;
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
using System.Windows.Shapes;

namespace LLGameStudio.Studio.Window
{
    /// <summary>
    /// PhysicsWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PhysicsWindow : System.Windows.Window
    {
        double everyItemHeight = 30;
        double everyItemNameMinWidth = 30;
        double gridTransformHeight = 0;
        double gridCommonHeight = 0;
        double gridOtherHeight = 0;

        string filePath = "";
        PhysCollision physCollision = null;

        double pointRadius = 3;
        double lineWidth = 3;

        public PhysicsWindow(string filePath)
        {
            InitializeComponent();

            this.filePath = filePath;
            physCollision = new PhysCollision();
            LLConvert.LoadContentFromXML(filePath, physCollision);

            canvas.Width = physCollision.designWidth.Value;
            canvas.Height = physCollision.designHeight.Value;
            canvas.Background = ThemeManager.GetBrushByName("physicsCanvasColor");

            foreach (var item in physCollision.propertyDictionary)
            {
                AddProperty(item.Value);
            }

            foreach (var item in physCollision.physPolygonList)
            {
                int listPointCount = item.listPoint.Count;
                for (int i=0;i< listPointCount; i++)
                {
                    Point currentPoint = item.listPoint[i];
                    Point nextPoint = i != listPointCount - 1? item.listPoint[i+1]: item.listPoint[0];

                    Ellipse ellipse = new Ellipse();
                    ellipse.Width = pointRadius * 2;
                    ellipse.Height = pointRadius * 2;
                    ellipse.Margin = new Thickness(currentPoint.X - pointRadius, currentPoint.Y- pointRadius,0,0);
                    ellipse.Fill = ThemeManager.GetBrushByName("physicsPolygonPointColor");
                    canvas.Children.Add(ellipse);

                    Line line = new Line();
                    line.StrokeThickness = lineWidth;
                    line.Stroke= ThemeManager.GetBrushByName("physicsPolygonLineColor");
                    line.X1 = currentPoint.X;
                    line.Y1 = currentPoint.Y;
                    line.X2 = nextPoint.X;
                    line.Y2 = nextPoint.Y;
                    canvas.Children.Add(line);
                }
            }
        }

        /// <summary>
        /// 移动窗体。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridTitle_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        /// <summary>
        /// 最小化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imageMinimizeWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// 最大化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imageMaximizeWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imageExitWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// 点击保存角色按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imageSavePhys_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            LLConvert.ExportContentToXML(filePath, physCollision);
        }

        /// <summary>
        /// 添加属性
        /// </summary>
        /// <param name="uiProperty"></param>
        public void AddProperty(IUIProperty uiProperty)
        {
            switch (uiProperty.PropertyEnum)
            {
                case UIPropertyEnum.Transform:
                    gridTransform.Children.Add(GetPropertyGridByType(uiProperty, ref gridTransformHeight));
                    break;
                case UIPropertyEnum.Common:
                    gridCommon.Children.Add(GetPropertyGridByType(uiProperty, ref gridCommonHeight));
                    break;
                case UIPropertyEnum.Other:
                    gridOther.Children.Add(GetPropertyGridByType(uiProperty, ref gridOtherHeight));
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 通过属性的类型添加合适的布局。
        /// </summary>
        /// <param name="uiProperty"></param>
        /// <param name="gridHeight"></param>
        /// <returns></returns>
        public Grid GetPropertyGridByType(IUIProperty uiProperty, ref double gridHeight)
        {
            Grid grid = new Grid();
            grid.VerticalAlignment = VerticalAlignment.Top;
            grid.Margin = new Thickness(0, gridHeight, 0, 0);
            grid.Height = everyItemHeight;

            ColumnDefinition columnDefinition1 = new ColumnDefinition();
            columnDefinition1.Width = new GridLength(0.4, GridUnitType.Star);
            columnDefinition1.MinWidth = everyItemNameMinWidth;
            grid.ColumnDefinitions.Add(columnDefinition1);

            ColumnDefinition columnDefinition2 = new ColumnDefinition();
            columnDefinition2.Width = new GridLength(0.6, GridUnitType.Star);
            grid.ColumnDefinitions.Add(columnDefinition2);

            Label lable = new Label();
            lable.VerticalAlignment = VerticalAlignment.Center;
            lable.ToolTip = uiProperty.HelpText;
            lable.SetValue(Grid.ColumnProperty, 0);
            lable.MinWidth = everyItemNameMinWidth;
            lable.Content = uiProperty.Name;
            grid.Children.Add(lable);

            if (uiProperty.Type == typeof(Double) || uiProperty.Type == typeof(int) || uiProperty.Type == typeof(float))
            {
                LLStudioNumberTextBox llStudioNumberTextBox = new LLStudioNumberTextBox();
                llStudioNumberTextBox.Padding = new Thickness(3);
                llStudioNumberTextBox.SetValue(Grid.ColumnProperty, 1);
                llStudioNumberTextBox.SetNumber(uiProperty.Value);
                llStudioNumberTextBox.ChangText += (d) => { ChangText(uiProperty.Name, d.ToString()); };
                grid.Children.Add(llStudioNumberTextBox);
                gridHeight += everyItemHeight;
            }
            else if (uiProperty.Type.IsEnum)
            {
                ComboBox comboBox = new ComboBox();
                comboBox.Padding = new Thickness(3);
                comboBox.SetValue(Grid.ColumnProperty, 1);
                comboBox.BorderBrush = ThemeManager.GetBrushByName("borderComboBoxColor");
                comboBox.Background = ThemeManager.GetBrushByName("backgroundComboBoxColor");
                comboBox.Foreground = ThemeManager.GetBrushByName("fontColor");

                foreach (string enumName in Enum.GetNames(uiProperty.Type))
                {
                    comboBox.Items.Add(enumName);
                }
                comboBox.SelectedItem = uiProperty.Value.ToString();

                comboBox.SelectionChanged += (sender, o) => { ChangText(uiProperty.Name, comboBox.SelectedItem.ToString()); };

                grid.Children.Add(comboBox);
                gridHeight += everyItemHeight;
            }
            else if (uiProperty.Type == typeof(bool))
            {
                CheckBox checkBox = new CheckBox();
                checkBox.VerticalAlignment = VerticalAlignment.Center;
                checkBox.HorizontalAlignment = HorizontalAlignment.Center;
                checkBox.Padding = new Thickness(3);
                checkBox.SetValue(Grid.ColumnProperty, 1);
                checkBox.IsChecked = uiProperty.Value;
                checkBox.Click += (sender, o) => { ChangText(uiProperty.Name, checkBox.IsChecked.ToString()); };
                grid.Children.Add(checkBox);
                gridHeight += everyItemHeight;
            }
            else if (uiProperty.Type == typeof(Vector2))
            {
                Grid childGrid = new Grid();
                childGrid.SetValue(Grid.ColumnProperty, 1);

                ColumnDefinition columnDefinition21 = new ColumnDefinition();
                columnDefinition21.Width = new GridLength(0.5, GridUnitType.Star);
                columnDefinition21.MinWidth = everyItemNameMinWidth;
                childGrid.ColumnDefinitions.Add(columnDefinition21);

                ColumnDefinition columnDefinition22 = new ColumnDefinition();
                columnDefinition22.Width = new GridLength(0.5, GridUnitType.Star);
                columnDefinition22.MinWidth = everyItemNameMinWidth;
                childGrid.ColumnDefinitions.Add(columnDefinition22);

                //X
                LLStudioNumberTextBox llStudioNumberTextBox1 = new LLStudioNumberTextBox();
                llStudioNumberTextBox1.ToolTip = "X";
                llStudioNumberTextBox1.Padding = new Thickness(3);
                llStudioNumberTextBox1.SetValue(Grid.ColumnProperty, 0);
                llStudioNumberTextBox1.SetNumber(uiProperty.Value.X);
                llStudioNumberTextBox1.ChangText += (d) => { uiProperty.Value.X = d; ChangText(uiProperty.Name, uiProperty.Value.ToString()); };
                childGrid.Children.Add(llStudioNumberTextBox1);

                //Y
                LLStudioNumberTextBox llStudioNumberTextBox2 = new LLStudioNumberTextBox();
                llStudioNumberTextBox2.ToolTip = "Y";
                llStudioNumberTextBox2.Padding = new Thickness(3);
                llStudioNumberTextBox2.SetValue(Grid.ColumnProperty, 1);
                llStudioNumberTextBox2.SetNumber(uiProperty.Value.Y);
                llStudioNumberTextBox2.ChangText += (d) => { uiProperty.Value.Y = d; ChangText(uiProperty.Name, uiProperty.Value.ToString()); };
                childGrid.Children.Add(llStudioNumberTextBox2);

                grid.Children.Add(childGrid);
                gridHeight += everyItemHeight;
            }
            else if (uiProperty.Type == typeof(Common.DataType.Rect))
            {
                grid.Height = everyItemHeight * 2;

                Grid childGrid = new Grid();
                childGrid.SetValue(Grid.ColumnProperty, 1);

                Grid grandchildGrid1 = new Grid();
                grandchildGrid1.VerticalAlignment = VerticalAlignment.Top;
                grandchildGrid1.Height = everyItemHeight;

                ColumnDefinition columnDefinition21 = new ColumnDefinition();
                columnDefinition21.Width = new GridLength(0.5, GridUnitType.Star);
                columnDefinition21.MinWidth = everyItemNameMinWidth;
                grandchildGrid1.ColumnDefinitions.Add(columnDefinition21);

                ColumnDefinition columnDefinition22 = new ColumnDefinition();
                columnDefinition22.Width = new GridLength(0.5, GridUnitType.Star);
                columnDefinition22.MinWidth = everyItemNameMinWidth;
                grandchildGrid1.ColumnDefinitions.Add(columnDefinition22);

                //Left
                LLStudioNumberTextBox llStudioNumberTextBox1 = new LLStudioNumberTextBox();
                llStudioNumberTextBox1.ToolTip = "Left";
                llStudioNumberTextBox1.Padding = new Thickness(3);
                llStudioNumberTextBox1.SetValue(Grid.ColumnProperty, 0);
                llStudioNumberTextBox1.SetNumber(uiProperty.Value.Left);
                llStudioNumberTextBox1.ChangText += (d) => { uiProperty.Value.Left = d; ChangText(uiProperty.Name, uiProperty.Value.ToString()); };
                grandchildGrid1.Children.Add(llStudioNumberTextBox1);

                //Top
                LLStudioNumberTextBox llStudioNumberTextBox2 = new LLStudioNumberTextBox();
                llStudioNumberTextBox2.ToolTip = "Top";
                llStudioNumberTextBox2.Padding = new Thickness(3);
                llStudioNumberTextBox2.SetValue(Grid.ColumnProperty, 1);
                llStudioNumberTextBox2.SetNumber(uiProperty.Value.Top);
                llStudioNumberTextBox2.ChangText += (d) => { uiProperty.Value.Top = d; ChangText(uiProperty.Name, uiProperty.Value.ToString()); };
                grandchildGrid1.Children.Add(llStudioNumberTextBox2);

                childGrid.Children.Add(grandchildGrid1);

                Grid grandchildGrid2 = new Grid();
                grandchildGrid2.VerticalAlignment = VerticalAlignment.Top;
                grandchildGrid2.Height = everyItemHeight;
                grandchildGrid2.Margin = new Thickness(0, everyItemHeight, 0, 0);

                ColumnDefinition columnDefinition23 = new ColumnDefinition();
                columnDefinition23.Width = new GridLength(0.5, GridUnitType.Star);
                columnDefinition23.MinWidth = everyItemNameMinWidth;
                grandchildGrid2.ColumnDefinitions.Add(columnDefinition23);

                ColumnDefinition columnDefinition24 = new ColumnDefinition();
                columnDefinition24.Width = new GridLength(0.5, GridUnitType.Star);
                columnDefinition24.MinWidth = everyItemNameMinWidth;
                grandchildGrid2.ColumnDefinitions.Add(columnDefinition24);

                //Right
                LLStudioNumberTextBox llStudioNumberTextBox3 = new LLStudioNumberTextBox();
                llStudioNumberTextBox3.ToolTip = "Right";
                llStudioNumberTextBox3.Padding = new Thickness(3);
                llStudioNumberTextBox3.SetValue(Grid.ColumnProperty, 0);
                llStudioNumberTextBox3.SetNumber(uiProperty.Value.Right);
                llStudioNumberTextBox3.ChangText += (d) => { uiProperty.Value.Right = d; ChangText(uiProperty.Name, uiProperty.Value.ToString()); };
                grandchildGrid2.Children.Add(llStudioNumberTextBox3);

                //Bottom
                LLStudioNumberTextBox llStudioNumberTextBox4 = new LLStudioNumberTextBox();
                llStudioNumberTextBox4.ToolTip = "Bottom";
                llStudioNumberTextBox4.Padding = new Thickness(3);
                llStudioNumberTextBox4.SetValue(Grid.ColumnProperty, 1);
                llStudioNumberTextBox4.SetNumber(uiProperty.Value.Bottom);
                llStudioNumberTextBox4.ChangText += (d) => { uiProperty.Value.Bottom = d; ChangText(uiProperty.Name, uiProperty.Value.ToString()); };
                grandchildGrid2.Children.Add(llStudioNumberTextBox4);

                childGrid.Children.Add(grandchildGrid2);

                grid.Children.Add(childGrid);
                gridHeight += 2 * everyItemHeight;
            }
            else
            {
                LLStudioTextBox textBox = new LLStudioTextBox();
                textBox.Padding = new Thickness(3);
                textBox.VerticalAlignment = VerticalAlignment.Center;
                textBox.FontSize = 15;
                textBox.BorderBrush = null;
                textBox.Background = ThemeManager.GetBrushByName("backgroundTextBoxColor");
                textBox.SetValue(Grid.ColumnProperty, 1);
                textBox.SetText(uiProperty.Value);
                textBox.ChangText += (s) => { ChangText(uiProperty.Name, s); };
                grid.Children.Add(textBox);
                gridHeight += everyItemHeight;
            }
            return grid;
        }

        /// <summary>
        /// 当改变属性值的内容时的处理方法
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        void ChangText(string name, string value)
        {
            physCollision.SetProperty(name, value);

            canvas.Width = physCollision.designWidth.Value;
            canvas.Height = physCollision.designHeight.Value;
        }

        /// <summary>
        /// 清空所有属性
        /// </summary>
        public void ClearAllProperty()
        {
            gridTransform.Children.Clear();
            gridCommon.Children.Clear();
            gridOther.Children.Clear();
            gridTransformHeight = 0;
            gridCommonHeight = 0;
            gridOtherHeight = 0;
        }
    }
}
