using LLGameStudio.Game;
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
    /// FontEditWindow.xaml 的交互逻辑
    /// </summary>
    public partial class FontEditWindow : System.Windows.Window
    {
        //List<LLFont> fontList = new List<LLFont>();
        Dictionary<string, LLStudioFontEdit> fontMap = new Dictionary<string, LLStudioFontEdit>();

        public FontEditWindow()
        {
            InitializeComponent();
            buttonClose.SetImage("Resource/退出.png");
            buttonClose.ToolTip = "关闭并保存";
            buttonClose.ClickHandler += CloseAndSave;
        }

        void CloseAndSave(object sender, MouseButtonEventArgs e)
        {
            FontManager.SaveFonts();
            Close();
        }

        public void Init()
        {
            foreach (var item in FontManager.GetAllFont())
            {
                AddFontToListBox(item.Value);
            }
        }

        void AddFontToListBox(LLFont font)
        {
            LLStudioFontEdit studioFontEdit = new LLStudioFontEdit();
            studioFontEdit.SetContent(font);
            studioFontEdit.changeNameHandler = ChangeName;
            studioFontEdit.changeAttributeHandler = ChangeAttribute;
            studioFontEdit.deleteHandler = DeleteFont;
            listBoxFont.Children.Add(studioFontEdit);
            fontMap[font.Name] = studioFontEdit;
        }

        void ChangeName(string oldName,LLFont font)
        {
            DeleteFont(oldName);
            AddFont(font);
        }

        void ChangeAttribute(LLFont font)
        {
            FontManager.EditFont(font);
        }

        void AddFont(LLFont font)
        {
            FontManager.AddFont(font);
            AddFontToListBox(font);
        }

        void DeleteFont(string name)
        {
            listBoxFont.Children.Remove(fontMap[name]);
            fontMap.Remove(name);
            FontManager.RemoveFont(name);
        }
    }
}
