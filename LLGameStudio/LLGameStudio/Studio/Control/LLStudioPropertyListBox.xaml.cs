using LLGameStudio.Common.DataType;
using LLGameStudio.Game.UI;
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
    /// LLStudioPropertyListBox.xaml 的交互逻辑
    /// </summary>
    public partial class LLStudioPropertyListBox : UserControl
    {
        double everyItemHeight = 20;
        double everyItemNameMinWidth = 30;

        public LLStudioPropertyListBox()
        {
            InitializeComponent();
        }

        public void AddProperty(IUIProperty uiProperty)
        {
            switch (uiProperty.PropertyEnum)
            {
                case UIPropertyEnum.Transform:
                    gridTransform.Children.Add(AddPropertyByType(uiProperty, gridTransform.Children.Count));
                    break;
                case UIPropertyEnum.Common:
                    gridCommon.Children.Add(AddPropertyByType(uiProperty, gridTransform.Children.Count));
                    break;
                case UIPropertyEnum.Other:
                    gridOther.Children.Add(AddPropertyByType(uiProperty, gridTransform.Children.Count));
                    break;
                default:
                    break;
            }
        }

        public Grid AddPropertyByType(IUIProperty uiProperty,int itemCount)
        {
            Grid grid = new Grid();
            grid.VerticalAlignment = VerticalAlignment.Top;
            grid.Margin = new Thickness(0, itemCount * everyItemHeight,0,0);
            grid.Height = everyItemHeight;

            ColumnDefinition columnDefinition1 = new ColumnDefinition();
            columnDefinition1.Width = new GridLength(0.4,GridUnitType.Star);
            columnDefinition1.MinWidth = everyItemNameMinWidth;
            grid.ColumnDefinitions.Add(columnDefinition1);

            ColumnDefinition columnDefinition2 = new ColumnDefinition();
            columnDefinition2.Width = new GridLength(0.6, GridUnitType.Star);
            grid.ColumnDefinitions.Add(columnDefinition2);

            Label lable = new Label();
            lable.ToolTip = uiProperty.HelpText;
            lable.SetValue(Grid.ColumnProperty,0);
            lable.MinWidth = everyItemNameMinWidth;
            lable.Content = uiProperty.Name;
            grid.Children.Add(lable);

            if (uiProperty.Type == typeof(String))
            {
                TextBox textBox = new TextBox();
                textBox.BorderBrush = null;
                textBox.Background = ThemeManager.GetBrushByName("backgroundTextBoxColor");
                textBox.SetValue(Grid.ColumnProperty, 1);
                grid.Children.Add(textBox);
            }
            else if (uiProperty.Type == typeof(Double))
            {
                LLStudioNumberTextBox llStudioNumberTextBox = new LLStudioNumberTextBox();
                llStudioNumberTextBox.SetValue(Grid.ColumnProperty, 1);
                grid.Children.Add(llStudioNumberTextBox);
            }
            else if (uiProperty.Type == typeof(GameUIAnchorEnum))
            {
                ComboBox comboBox = new ComboBox();
                comboBox.SetValue(Grid.ColumnProperty, 1);
                comboBox.BorderBrush = ThemeManager.GetBrushByName("borderComboBoxColor");
                comboBox.Background = ThemeManager.GetBrushByName("backgroundComboBoxColor");
                comboBox.Foreground = ThemeManager.GetBrushByName("fontColor");
                grid.Children.Add(comboBox);
            }
            else if (uiProperty.Type == typeof(bool))
            {
                ComboBox comboBox = new ComboBox();
                comboBox.SetValue(Grid.ColumnProperty, 1);
                comboBox.BorderBrush = ThemeManager.GetBrushByName("borderComboBoxColor");
                comboBox.Background = ThemeManager.GetBrushByName("backgroundComboBoxColor");
                comboBox.Foreground = ThemeManager.GetBrushByName("fontColor");
                grid.Children.Add(comboBox);
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

                LLStudioNumberTextBox llStudioNumberTextBox1 = new LLStudioNumberTextBox();
                llStudioNumberTextBox1.SetValue(Grid.ColumnProperty, 0);
                childGrid.Children.Add(llStudioNumberTextBox1);

                LLStudioNumberTextBox llStudioNumberTextBox2 = new LLStudioNumberTextBox();
                llStudioNumberTextBox2.SetValue(Grid.ColumnProperty, 1);
                childGrid.Children.Add(llStudioNumberTextBox2);

                grid.Children.Add(childGrid);
            }
            else if (uiProperty.Type == typeof(Common.DataType.Rect))
            {

            }
            else
            {
                MessageBox.Show("属性未知类型！");
            }
            return grid;
        }
    }
}
