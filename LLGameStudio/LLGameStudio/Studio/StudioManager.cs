﻿using LLGameStudio.Common.XML;
using LLGameStudio.Common;
using LLGameStudio.Game;
using LLGameStudio.Studio.Control;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using LLGameStudio.Game.UI;
using LLGameStudio.Studio.Window;
using LLGameStudio.Game.Particle;
using LLGameStudio.Game.Actor;

namespace LLGameStudio.Studio
{
    public class StudioManager
    {
        string studioConfigFilePath = @"Config\Studio.xml";
        string fileAreaDirectory="";
        string currentFilePath="";
        StudioConfig studioConfig;
        MainWindow window =null;
        GameManager gameManager = null;
        CanvasManager canvasManager = null;

        TreeView treeViewUILayer;

        //以下是从主窗体中获得的用来显示控件的容器
        //菜单区
        WrapPanel wrapPanelMenuArea;
        //游戏控制区
        Grid gridGameControlArea;
        //文件区
        WrapPanel wrapPanelFileArea;
        //状态显示
        Label labelStatusInfo;
        //UI层级区
        Grid gridUILayer;
        //UI控件区
        WrapPanel wrapPanelUIControlArea;
        //属性编辑区
        Grid gridPropertyEditorArea;
        //属性编辑列表
        LLStudioPropertyListBox listBoxPropertyEditor;

        public bool FullScreen { get => studioConfig.FullScreen;}
        public string FileAreaDirectory { get => fileAreaDirectory; }
        public string CurrentFilePath { get => currentFilePath;}

        private static StudioManager studioManager = null;

        private StudioManager(MainWindow window)
        {
            System.Drawing.Graphics g = System.Drawing.Graphics.FromHwnd(IntPtr.Zero);
            Common.Standard.CurrentDpiX = g.DpiX;
            Common.Standard.CurrentDpiY = g.DpiY;
            Common.Standard.ScaleX = Common.Standard.StandardDpiX / g.DpiX;
            Common.Standard.ScaleY = Common.Standard.StandardDpiY / g.DpiY;

            this.window = window;

            //从主窗体中获得控件
            wrapPanelMenuArea = window.GetWrapPanelMenuArea();
            gridGameControlArea = window.GetGridGameControlArea();
            wrapPanelFileArea = window.GetWrapPanelFileArea();
            labelStatusInfo = window.GetLabelStatusInfo();
            gridUILayer = window.GetGridUILayer();
            wrapPanelUIControlArea = window.GetWrapPanelUIControlArea();
            gridPropertyEditorArea = window.GetGridPropertyEditorArea();

            LoadConfig();
            GameManager.InitGameManager(this);
            gameManager = GameManager.GetSingleInstance();
            ThemeManager.LoadTheme(studioConfig.Theme);

            CanvasManager.InitCanvasManager(window.GetCanvas(), gameManager);
            canvasManager = CanvasManager.GetSingleInstance();
            
            if(!string.IsNullOrEmpty(studioConfig.LastGamePath))
            {
                OpenGameByPath(studioConfig.LastGamePath);
            }
        }

        /// <summary>
        /// 初始化编辑管理器
        /// </summary>
        /// <param name="window"></param>
        public static void InitStudioManager(MainWindow window)
        {
            if(studioManager==null)
            {
                studioManager = new StudioManager(window);
            }
        }

        /// <summary>
        /// 获得编辑管理器的单例
        /// </summary>
        /// <returns></returns>
        public static StudioManager GetSingleInstance()
        {
            return studioManager;
        }

        /// <summary>
        /// 让画布重新设置UI选中边框的位置和大小。
        /// </summary>
        public void ResetUINodeBorderPositionAndSize()
        {
            canvasManager.ResetUINodeBorderPositionAndSize();
        }

        /// <summary>
        /// 从文件中加载编辑器配置。
        /// </summary>
        public void LoadConfig()
        {
            ShowStatusInfo("正加载配置。");
            studioConfig = new StudioConfig();
            LLConvert.LoadContentFromXML(studioConfigFilePath, studioConfig);

            if (studioConfig.FullScreen)
            {
                window.Height = SystemParameters.WorkArea.Height;
                window.Width = SystemParameters.WorkArea.Width;
                window.Top = SystemParameters.WorkArea.Top;
                window.Left = SystemParameters.WorkArea.Left;
            }
            else
            {
                window.Top = studioConfig.Top;
                window.Left = studioConfig.Left;
                window.Width = studioConfig.Width;
                window.Height = studioConfig.Height;
            }
            window.Title = studioConfig.StudioName;
            ShowStatusInfo("配置加载完成。");
        }

        /// <summary>
        /// 保存编辑器配置到文件中。
        /// </summary>
        public void SaveConfig()
        {
            ShowStatusInfo("正保存配置。");
            LLConvert.ExportContentToXML(studioConfigFilePath, studioConfig);
            ShowStatusInfo("配置保存完成。");
        }

        /// <summary>
        /// 初始化控件
        /// </summary>
        public void InitControls()
        {
            //菜单区

            LLStudioButton createGameButton = new LLStudioButton();
            createGameButton.SetImage("Resource/新建文件.png"); 
            createGameButton.ToolTip = "新建游戏";
            createGameButton.ClickHandler += ChooseNewGamePathAndOpen;
            wrapPanelMenuArea.Children.Add(createGameButton);

            LLStudioButton openGameButton = new LLStudioButton();
            openGameButton.SetImage("Resource/打开文件.png");
            openGameButton.ToolTip = "打开游戏";
            openGameButton.ClickHandler += ChooseGamePathAndOpen;
            wrapPanelMenuArea.Children.Add(openGameButton);

            LLStudioButton saveGameButton = new LLStudioButton();
            saveGameButton.SetImage("Resource/保存.png");
            saveGameButton.ToolTip = "保存游戏";
            saveGameButton.ClickHandler += (object sender, 
                MouseButtonEventArgs e) => { SaveGame(); };
            wrapPanelMenuArea.Children.Add(saveGameButton);

            LLStudioButton setGameButton = new LLStudioButton();
            setGameButton.SetImage("Resource/设置.png"); 
            setGameButton.ToolTip = "设置游戏配置";
            setGameButton.ClickHandler += (object sender,
                MouseButtonEventArgs e) => { SetGameConfig(); };
            wrapPanelMenuArea.Children.Add(setGameButton);

            //游戏控制区

            LLStudioButton startGameButton = new LLStudioButton();
            startGameButton.SetImage("Resource/开始.png");
            startGameButton.ToolTip = "开始游戏";
            startGameButton.ClickHandler += StartGame;
            startGameButton.HorizontalAlignment = HorizontalAlignment.Left;
            startGameButton.VerticalAlignment = VerticalAlignment.Center;
            gridGameControlArea.Children.Add(startGameButton);

            LLStudioButton stopGameButton = new LLStudioButton();
            stopGameButton.SetImage("Resource/结束.png");
            stopGameButton.ToolTip = "结束游戏";
            stopGameButton.ClickHandler += StopGame;
            stopGameButton.HorizontalAlignment = HorizontalAlignment.Right;
            stopGameButton.VerticalAlignment = VerticalAlignment.Center;
            stopGameButton.Margin = new Thickness(100, 0, 0, 0);
            gridGameControlArea.Children.Add(stopGameButton);

            //UI层级区

            Label label = new Label();
            label.Height = 25;
            label.VerticalAlignment = VerticalAlignment.Top;
            label.HorizontalAlignment = HorizontalAlignment.Left;
            label.Margin = new Thickness(0, 5, 0, 2.5);
            label.Content = "查找节点：";
            gridUILayer.Children.Add(label);

            TextBox textBox = new TextBox();
            textBox.Height = 25;
            textBox.VerticalAlignment = VerticalAlignment.Top;
            textBox.Margin = new Thickness(80,2.5,10,2.5);
            textBox.Background = ThemeManager.GetBrushByName("backgroundTextBoxColor");
            textBox.ToolTip = "输入文字过滤节点";
            textBox.TextChanged += UILayerFilterTextChanged;
            gridUILayer.Children.Add(textBox);

            treeViewUILayer = new TreeView();
            treeViewUILayer.BorderThickness = new Thickness(0);
            treeViewUILayer.Background = null;
            treeViewUILayer.Margin= new Thickness(0, 25, 0, 0);
            TreeResetItem();
            gridUILayer.Children.Add(treeViewUILayer);

            //UI控件区

            AddLegalControl();

            //文件区：右键菜单

            ContextMenu contextMenu = new ContextMenu();

            MenuItem mi0 = new MenuItem();
            mi0.Header = "新建文件夹";
            mi0.Click += CreateNewFolderToCurrrentDirectory;
            contextMenu.Items.Add(mi0);

            contextMenu.Items.Add(new Separator());

            MenuItem mi1 = new MenuItem();
            mi1.Header = "新建Scene";
            mi1.Click += CreateNewSceneToCurrrentDirectory;
            contextMenu.Items.Add(mi1);

            MenuItem mi2 = new MenuItem();
            mi2.Header = "新建layout";
            mi2.Click += CreateNewLayoutToCurrrentDirectory;
            contextMenu.Items.Add(mi2);

            MenuItem mi3 = new MenuItem();
            mi3.Header = "新建particle";
            mi3.Click += CreateNewParticleToCurrrentDirectory;
            contextMenu.Items.Add(mi3);

            MenuItem mi4 = new MenuItem();
            mi4.Header = "新建actor";
            mi4.Click += CreateNewActorToCurrrentDirectory;
            contextMenu.Items.Add(mi4);

            MenuItem mi5 = new MenuItem();
            mi5.Header = "新建llscript";
            mi5.Click += CreateNewScriptToCurrrentDirectory;
            contextMenu.Items.Add(mi5);

            wrapPanelFileArea.ContextMenu = contextMenu;

            //属性编辑区

            listBoxPropertyEditor = new LLStudioPropertyListBox(this);
            gridPropertyEditorArea.Children.Add(listBoxPropertyEditor);
        }

        /// <summary>
        /// 添加现阶段支持的控件
        /// </summary>
        private void AddLegalControl()
        {
            wrapPanelUIControlArea.Children.Clear();
            if (!gameManager.GameLoaded)
            {
                return;
            }

            if(gameManager.CurrentOpenResourceEnum== GameResourceEnum.Layout)
            {
                LLStudioButton gameControlButton = new LLStudioButton();
                gameControlButton.Width = 100;
                gameControlButton.Height = 100;
                gameControlButton.SetImage("Resource/Button.png");
                gameControlButton.MouseDoubleClick +=
                    (object sender, MouseButtonEventArgs e) => { gameManager.AddControlToLayout(new LLGameButton()); };
                gameControlButton.ToolTip = "按钮";
                wrapPanelUIControlArea.Children.Add(gameControlButton);

                LLStudioButton gameControlText = new LLStudioButton();
                gameControlText.Width = 100;
                gameControlText.Height = 100;
                gameControlText.SetImage("Resource/Text.png");
                gameControlText.MouseDoubleClick +=
                    (object sender, MouseButtonEventArgs e) => { gameManager.AddControlToLayout(new LLGameText()); };
                gameControlText.ToolTip = "文字";
                wrapPanelUIControlArea.Children.Add(gameControlText);

                LLStudioButton gameControlImage = new LLStudioButton();
                gameControlImage.Width = 100;
                gameControlImage.Height = 100;
                gameControlImage.SetImage("Resource/Image.png");
                gameControlImage.MouseDoubleClick +=
                    (object sender, MouseButtonEventArgs e) => { gameManager.AddControlToLayout(new LLGameImage()); };
                gameControlImage.ToolTip = "图片";
                wrapPanelUIControlArea.Children.Add(gameControlImage);

                LLStudioButton gameControlComboBox = new LLStudioButton();
                gameControlComboBox.Width = 100;
                gameControlComboBox.Height = 100;
                gameControlComboBox.SetImage("Resource/ComboBox.png");
                gameControlComboBox.MouseDoubleClick +=
                    (object sender, MouseButtonEventArgs e) => { gameManager.AddControlToLayout(new LLGameComboBox()); };
                gameControlComboBox.ToolTip = "下拉框";
                wrapPanelUIControlArea.Children.Add(gameControlComboBox);

                LLStudioButton gameControlSlide = new LLStudioButton();
                gameControlSlide.Width = 100;
                gameControlSlide.Height = 100;
                gameControlSlide.SetImage("Resource/Slide.png");
                gameControlSlide.MouseDoubleClick +=
                    (object sender, MouseButtonEventArgs e) => { gameManager.AddControlToLayout(new LLGameSlide()); };
                gameControlSlide.ToolTip = "滑动条";
                wrapPanelUIControlArea.Children.Add(gameControlSlide);

                LLStudioButton gameControlTextBox = new LLStudioButton();
                gameControlTextBox.Width = 100;
                gameControlTextBox.Height = 100;
                gameControlTextBox.SetImage("Resource/TextBox.png");
                gameControlTextBox.MouseDoubleClick +=
                    (object sender, MouseButtonEventArgs e) => { gameManager.AddControlToLayout(new LLGameTextBox()); };
                gameControlTextBox.ToolTip = "文本框";
                wrapPanelUIControlArea.Children.Add(gameControlTextBox);
            }
            else if(gameManager.CurrentOpenResourceEnum == GameResourceEnum.Scene)
            {
                LLStudioButton gameControlBack = new LLStudioButton();
                gameControlBack.Width = 100;
                gameControlBack.Height = 100;
                gameControlBack.SetImage("Resource/Back.png");
                gameControlBack.MouseDoubleClick += 
                    (object sender, MouseButtonEventArgs e) => { gameManager.AddControlToLayout(new LLGameBack()); };
                gameControlBack.ToolTip = "背景";
                wrapPanelUIControlArea.Children.Add(gameControlBack);

                LLStudioButton gameControlCanvas = new LLStudioButton();
                gameControlCanvas.Width = 100;
                gameControlCanvas.Height = 100;
                gameControlCanvas.SetImage("Resource/Canvas.png");
                gameControlCanvas.MouseDoubleClick +=
                    (object sender, MouseButtonEventArgs e) => { gameManager.AddControlToLayout(new LLGameCanvas()); };
                gameControlCanvas.ToolTip = "画布";
                wrapPanelUIControlArea.Children.Add(gameControlCanvas);

                LLStudioButton gameControlLayout = new LLStudioButton();
                gameControlLayout.Width = 100;
                gameControlLayout.Height = 100;
                gameControlLayout.SetImage("Resource/Layout.png");
                gameControlLayout.MouseDoubleClick +=
                    (object sender, MouseButtonEventArgs e) => { gameManager.AddControlToLayout(new LLGameLayout()); };
                gameControlLayout.ToolTip = "布局";
                wrapPanelUIControlArea.Children.Add(gameControlLayout);
            }
        }

        /// <summary>
        /// 在资源管理器中打开文件夹
        /// </summary>
        public void ShowDirectoryInSystem()
        {
            System.Diagnostics.Process.Start(fileAreaDirectory);
        }

        /// <summary>
        /// 将选中的UI节点同步到UI层级树中。
        /// </summary>
        /// <param name="currentUINode"></param>
        /// <param name="treeView"></param>
        public void SelectUINodeToTree(IUINode currentUINode, LLStudioTreeViewItem treeView =null)
        {
            ItemCollection itemCollection;
            if (treeView == null)
            {
                itemCollection = treeViewUILayer.Items;
            }
            else
            {
                itemCollection = treeView.Items;
            }
            foreach (LLStudioTreeViewItem item in itemCollection)
            {
                if (item.GetUINode() == currentUINode)
                {
                    ShowPropertyToEditorArea(currentUINode);
                    item.IsSelected = true;
                    LLStudioTreeViewItem parentTreeViewItem = item.Parent as LLStudioTreeViewItem;
                    while (parentTreeViewItem != null)
                    {
                        parentTreeViewItem.IsExpanded = true;
                        parentTreeViewItem = parentTreeViewItem.Parent as LLStudioTreeViewItem;
                    }
                    return;
                }
                SelectUINodeToTree(currentUINode, item);
            }
        }

        /// <summary>
        /// 将当前节点的属性添加到属性编辑区。
        /// </summary>
        /// <param name="currentUINode"></param>
        public void ShowPropertyToEditorArea(IUINode currentUINode)
        {
            if(currentUINode!=null)
            {
                listBoxPropertyEditor.ClearAllProperty();
                listBoxPropertyEditor.currentUINode = currentUINode;
                foreach (var item in currentUINode.propertyDictionary)
                {
                    listBoxPropertyEditor.AddProperty(item.Value);
                }
            }
        }

        /// <summary>
        /// 添加新文件夹到当前文件夹下，文件夹命名规则：
        /// “新建文件夹”+数字，最多新建1000个。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateNewFolderToCurrrentDirectory(object sender, RoutedEventArgs e)
        {
            if(gameManager.GameLoaded)
            {
                int folderNum=1;
                string newFolderPath = fileAreaDirectory + @"/" +"新建文件夹"+ folderNum;
                while (Directory.Exists(newFolderPath))
                {
                    folderNum++;
                    newFolderPath = fileAreaDirectory + @"/" + folderNum;
                    if(folderNum==1000)
                    {
                        ShowStatusInfo("当前文件夹内文件夹过多。");
                        return ;
                    }
                }
                Directory.CreateDirectory(newFolderPath);
                LoadDirectoryToFileArea(fileAreaDirectory);
            }
            else
            {
                ShowStatusInfo("未打开游戏目录。");
            }
        }

        /// <summary>
        /// 新建文件到当前文件夹下，文件命名规则：
        /// 文件类型+数字+“.”+文件类型，最多新建1000个。
        /// </summary>
        /// <param name="uiType"></param>
        /// <returns></returns>
        private bool CreateNewFileToCurrrentDirectory(GameResourceEnum uiType)
        {
            if (gameManager.GameLoaded)
            {
                int fileNum = 0;
                string newFilePath;
                string fileType = uiType.ToString().ToLower();
                do
                {
                    if (fileNum == 1000)
                    {
                        ShowStatusInfo("当前文件夹内新建文件过多。");
                        return false;
                    }
                    fileNum++;
                    newFilePath = fileAreaDirectory + @"/" + fileType + fileNum + "."+ fileType;

                } while (File.Exists(newFilePath));
                File.Create(newFilePath).Close();
                IXMLClass iXMLClass=null;
                string fileName = fileType + fileNum + "." + fileType;
                switch (uiType)
                {
                    case GameResourceEnum.Folder:
                        break;
                    case GameResourceEnum.Scene:
                        iXMLClass = new LLGameScene();
                        break;
                    case GameResourceEnum.Layout:
                        iXMLClass = new LLGameLayout();
                        break;
                    case GameResourceEnum.Particle:
                        iXMLClass = new ParticleSystem(fileName, null);
                        break;
                    case GameResourceEnum.Actor:
                        iXMLClass = new Actor(fileName);
                        break;
                    case GameResourceEnum.Script:
                        break;
                    case GameResourceEnum.Unknown:
                        break;
                    default:
                        break;
                }
                if(iXMLClass!=null)
                {
                    LLConvert.ExportContentToXML(newFilePath, iXMLClass);
                }
                LoadDirectoryToFileArea(fileAreaDirectory);
            }
            else
            {
                ShowStatusInfo("未打开游戏目录。");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 新建场景文件到当前文件夹下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateNewSceneToCurrrentDirectory(object sender, RoutedEventArgs e)
        {
            CreateNewFileToCurrrentDirectory(GameResourceEnum.Scene);
        }

        /// <summary>
        /// 新建布局文件到当前文件夹下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateNewLayoutToCurrrentDirectory(object sender, RoutedEventArgs e)
        {
            CreateNewFileToCurrrentDirectory(GameResourceEnum.Layout);
        }

        /// <summary>
        /// 新建粒子文件到当前文件夹下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateNewParticleToCurrrentDirectory(object sender, RoutedEventArgs e)
        {
            CreateNewFileToCurrrentDirectory(GameResourceEnum.Particle);
        }

        /// <summary>
        /// 新建角色文件到当前文件夹下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateNewActorToCurrrentDirectory(object sender, RoutedEventArgs e)
        {
            CreateNewFileToCurrrentDirectory(GameResourceEnum.Actor);
        }

        /// <summary>
        /// 新建角色文件到当前文件夹下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateNewScriptToCurrrentDirectory(object sender, RoutedEventArgs e)
        {
            CreateNewFileToCurrrentDirectory(GameResourceEnum.Script);
        }

        /// <summary>
        /// 重新加载UI结构树
        /// </summary>
        public void TreeResetItem()
        {
            if(gameManager.rootNode != null)
            {
                treeViewUILayer.Items.Clear();

                if(gameManager.CurrentOpenResourceEnum== GameResourceEnum.Scene)
                {
                    LLStudioTreeViewItem treeViewItem = new LLStudioTreeViewItem();
                    treeViewItem.SetUINodeItem(gameManager.rootNode);
                    treeViewItem.Header = gameManager.rootNode.name.Value;
                    treeViewItem.IsExpanded = true;
                    treeViewItem.MouseDoubleClick += SelectUINodeByTreeView;
                    if (!(gameManager.rootNode is LLGameLayout))
                    {
                        AddNodeToTree(gameManager.rootNode, treeViewItem);
                    }
                    treeViewUILayer.Items.Add(treeViewItem);
                }
                else
                {
                    foreach (var item in gameManager.rootNode.listNode)
                    {
                        LLStudioTreeViewItem treeViewItem = new LLStudioTreeViewItem();
                        treeViewItem.SetUINodeItem(item);
                        treeViewItem.Header = item.name.Value;
                        treeViewItem.IsExpanded = true;
                        treeViewItem.MouseDoubleClick += SelectUINodeByTreeView;
                        if (!(item is LLGameLayout))
                        {
                            AddNodeToTree(item, treeViewItem);
                        }
                        treeViewUILayer.Items.Add(treeViewItem);
                    }
                }
            }
        }

        /// <summary>
        /// 添加UI节点到UI层级树中。
        /// </summary>
        /// <param name="node"></param>
        /// <param name="rootItem"></param>
        public void AddNodeToTree(IUINode node, LLStudioTreeViewItem rootItem)
        {
            foreach (var item in node.listNode)
            {
                LLStudioTreeViewItem treeViewItem = new LLStudioTreeViewItem();
                treeViewItem.SetUINodeItem(item);
                treeViewItem.ContextMenu =new ContextMenu();

                MenuItem mi0 = new MenuItem();
                mi0.Header = "删除节点";
                mi0.Click += DeleteNodeFromTree;
                treeViewItem.ContextMenu.Items.Add(mi0);

                treeViewItem.Header = item.name.Value;
                treeViewItem.IsExpanded = true;
                treeViewItem.MouseDoubleClick += SelectUINodeByTreeView;
                if(!(item is LLGameLayout))
                {
                    AddNodeToTree(item, treeViewItem);
                }
                rootItem.Items.Add(treeViewItem);
            }
        }

        /// <summary>
        /// 通过双击UI层级树的某个节点来选中画布中的相应节点，并刷新属性编辑区。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectUINodeByTreeView(object sender, MouseButtonEventArgs e)
        {
            LLStudioTreeViewItem treeViewItem = sender as LLStudioTreeViewItem;
            if (treeViewItem == treeViewUILayer.SelectedItem)
            {
                IUINode node = treeViewItem.GetUINode() as IUINode;
                canvasManager.SelectUINode(node);
                gameManager.currentSelectUINode = node;
            }
            ShowPropertyToEditorArea(gameManager.currentSelectUINode);
        }

        /// <summary>
        /// 使用指定字符串对节点进行过滤
        /// </summary>
        /// <param name="root"></param>
        /// <param name="filterString"></param>
        /// <returns>true：代表找到符合节点</returns>
        private bool FilterNextNode(LLStudioTreeViewItem root, string filterString)
        {
            bool findFlag = false;
            for (int i = 0; i < root.Items.Count;)
            {
                LLStudioTreeViewItem item = root.Items[i] as LLStudioTreeViewItem;
                if (FilterNextNode(item, filterString))
                {
                    findFlag = true;
                    i++;
                }
                else
                {
                    root.Items.Remove(item);
                }
            }
            if (((string)root.Header).Contains(filterString))
            {
                findFlag = true;
            }
            return findFlag;
        }

        /// <summary>
        /// 改变输入时过滤UI层级节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UILayerFilterTextChanged(object sender, TextChangedEventArgs e)
        {
            TreeResetItem();
            TextBox textBox = sender as TextBox;
            if (textBox.Text == "")
            {
                TreeResetItem();
                return;
            }
            for (int i = 0; i < treeViewUILayer.Items.Count;)
            {
                LLStudioTreeViewItem item = treeViewUILayer.Items[i] as LLStudioTreeViewItem;
                if (!FilterNextNode(item, textBox.Text))
                {
                    treeViewUILayer.Items.Remove(item);
                }
                else
                {
                    i++;
                }
            }
        }

        /// <summary>
        /// 通过树删除节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteNodeFromTree(object sender, RoutedEventArgs e)
        {
            ContextMenu contextMenu = (sender as MenuItem).Parent as ContextMenu;
            LLStudioTreeViewItem treeViewItem = contextMenu.PlacementTarget as LLStudioTreeViewItem;
            
            if (treeViewItem == treeViewUILayer.SelectedItem)
            {
                IUINode node = treeViewItem.GetUINode() as IUINode;
                node.RemoveThisNode();
            }
        }

        /// <summary>
        /// 初始化画布
        /// </summary>
        public void InitCanvas()
        {
            ClearCanvas();
            DrawCanvas();
        }

        /// <summary>
        /// 移动画布基础位置
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void MoveCanvas(double x, double y)
        {
            if(gameManager.GameLoaded)
            {
                canvasManager.MoveCanvas(x, y);
            }
        }

        /// <summary>
        /// 缩放画布：对变换过的画布进行再变换，返回当前缩放比例
        /// </summary>
        /// <param name="centerPosition">中心位置</param>
        /// <param name="rate">缩放比例</param>
        public double ScaleCanvas(Point centerPosition, double rate)
        {
            if (gameManager.GameLoaded)
            {
                return canvasManager.ScaleCanvas(centerPosition, rate);
            }
            return 1;
        }

        /// <summary>
        /// 缩放画布：直接缩放画布到指定比例
        /// </summary>
        /// <param name="rate"></param>
        public void ScaleCanvas(double rate)
        {
            if (gameManager.GameLoaded)
            {
                canvasManager.ScaleCanvas(rate);
            }
        }

        /// <summary>
        /// 清空画布内容
        /// </summary>
        public void ClearCanvas()
        {
            canvasManager.ClearAll();
        }

        /// <summary>
        /// 重新设置编辑器窗体大小。
        /// </summary>
        public void ResizeStudio()
        {
            if (!studioConfig.FullScreen)
            {
                studioConfig.Height = (int)window.Height;
                studioConfig.Width = (int)window.Width;
                studioConfig.Top = (int)window.Top;
                studioConfig.Left = (int)window.Left;
            }
        }

        /// <summary>
        /// 移动编辑器窗体位置。
        /// </summary>
        public void MoveStudio()
        {
            window.DragMove();
            studioConfig.Top = (int)window.Top;
            studioConfig.Left = (int)window.Left;
            studioConfig.FullScreen = false;
        }

        /// <summary>
        /// 最大化编辑器。
        /// </summary>
        public void MaximizeStudio()
        {
            studioConfig.FullScreen = true;
            window.Height = SystemParameters.WorkArea.Height;
            window.Width = SystemParameters.WorkArea.Width;
            window.Top = SystemParameters.WorkArea.Top;
            window.Left = SystemParameters.WorkArea.Left;
        }

        /// <summary>
        /// 恢复编辑器大小。
        /// </summary>
        public void RestoreStudio()
        {
            window.Height = studioConfig.Height;
            window.Width = studioConfig.Width;
            window.Top = studioConfig.Top;
            window.Left = studioConfig.Left;
            studioConfig.FullScreen = false;
        }

        /// <summary>
        /// 最小化编辑器。
        /// </summary>
        public void MinimizeStudio()
        {
            window.WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// 退出编辑器。
        /// </summary>
        public void ExitStudio()
        {
            SaveConfig();
            ShowStatusInfo(studioConfig.StudioName + "已退出。");
            Environment.Exit(0);
        }

        /// <summary>
        /// 显示编辑器帮助信息。
        /// </summary>
        public void ShowStudioHelpInfo()
        {
            MessageBox.Show("当前版本：1.0.0。");
        }

        /// <summary>
        /// 开始游戏。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void StartGame(object sender, MouseButtonEventArgs e)
        {
            SaveGame();
            gameManager.StartGame();
        }

        /// <summary>
        /// 保存游戏文件。
        /// </summary>
        public void SaveGame()
        {
            ShowStatusInfo("正保存游戏。");
            gameManager.SaveGame();
            ShowStatusInfo("游戏保存完成。");
        }

        /// <summary>
        /// 设置游戏配置
        /// </summary>
        public void SetGameConfig()
        {
            if(gameManager.GameLoaded)
            {
                GameConfigWindow gameConfigWindow = new GameConfigWindow(gameManager);
                gameConfigWindow.Owner = window;
                gameConfigWindow.ShowDialog();
            }
            else
            {
                ShowStatusInfo("当前未加载游戏！");
            }
        }

        /// <summary>
        /// 停止游戏。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void StopGame(object sender, MouseButtonEventArgs e)
        {
            gameManager.StopGame();
        }

        /// <summary>
        /// 绘制画布内容
        /// </summary>
        public void DrawCanvas()
        {
            if(gameManager.GameLoaded)
            {
                canvasManager.ReDrawAll();
            }
        }

        /// <summary>
        /// 恢复画布变换
        /// </summary>
        public void RestoreCanvas()
        {
            if (gameManager.GameLoaded)
            {
                canvasManager.RestoreCanvas();
            }
        }

        /// <summary>
        /// 打开下一文件目录
        /// </summary>
        private void OpenDirectory(object sender, MouseButtonEventArgs e)
        {
            currentFilePath = "";
            LLStudioFileItem item = sender as LLStudioFileItem;
            fileAreaDirectory += @"\" + item.textBox.Text;
            window.SetCurrentDirectory(fileAreaDirectory);
            LoadDirectoryToFileArea(FileAreaDirectory);
        }

        /// <summary>
        /// 打开文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenFile(object sender, MouseButtonEventArgs e)
        {
            LLStudioFileItem item = sender as LLStudioFileItem;
            string openFilePath = fileAreaDirectory + @"\" + item.textBox.Text;
            switch (item.GetFileEnum())
            {
                case GameResourceEnum.Scene:
                case GameResourceEnum.Layout:
                    currentFilePath = openFilePath;
                    OpenFile();
                    window.RestoreCanvas();
                    break;
                case GameResourceEnum.Particle:
                    ParticleWindow particleWindow = new ParticleWindow(openFilePath);
                    particleWindow.Owner = window;
                    particleWindow.ShowDialog();
                    break;
                case GameResourceEnum.Actor:
                    ActorWindow actorWindow = new ActorWindow(openFilePath);
                    actorWindow.Owner = window;
                    actorWindow.ShowDialog();
                    break;
                case GameResourceEnum.Script:
                    System.Diagnostics.Process.Start(openFilePath);
                    break;
                case GameResourceEnum.Physics:
                    PhysicsWindow physicsWindow = new PhysicsWindow(openFilePath);
                    physicsWindow.Owner = window;
                    physicsWindow.ShowDialog();
                    break;
                case GameResourceEnum.Unknown:
                    ShowStatusInfo("未知文件无法打开！");
                    break;
            }
        }

        /// <summary>
        /// 加载指定路径下文件图标到显示文件区域。
        /// </summary>
        /// <param name="path"></param>
        public void LoadDirectoryToFileArea(string path)
        {
            wrapPanelFileArea.Children.Clear();
            DirectoryInfo di = new DirectoryInfo(path);
            foreach (DirectoryInfo diitem in di.GetDirectories())
            {
                LLStudioFileItem fileItem = new LLStudioFileItem(wrapPanelFileArea, diitem.FullName);
                fileItem.MouseDoubleClick += OpenDirectory;
                wrapPanelFileArea.Children.Add(fileItem);
            }

            foreach (FileInfo fi in di.GetFiles())
            {
                LLStudioFileItem fileItem = new LLStudioFileItem(wrapPanelFileArea, fi.FullName);
                fileItem.MouseDoubleClick += OpenFile;
                //fileItem添加拖动事件
                wrapPanelFileArea.Children.Add(fileItem);
            }
        }

        /// <summary>
        /// 新建游戏路径并打开。
        /// </summary>
        /// <returns>返回在当前路径是否创建成功。</returns>
        public void ChooseNewGamePathAndOpen(object sender, MouseButtonEventArgs e)
        {
            NewGameWindow newGameWindow = new NewGameWindow(this);
            newGameWindow.Owner = window;
            newGameWindow.ShowDialog();
        }

        /// <summary>
        /// 新建游戏
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        public void CreateGame(string path,string name)
        {
            ShowStatusInfo("正新建游戏目录。");
            gameManager.CreateGame(path, name);
            ShowStatusInfo("游戏目录新建完成。");
            OpenGameByPath(path);
        }

        /// <summary>
        /// 选择游戏路径并打开。
        /// </summary>
        /// <returns>是否成功打开</returns>
        public void ChooseGamePathAndOpen(object sender, MouseButtonEventArgs e)
        {
            CommonOpenFileDialog folderDialog = new CommonOpenFileDialog("请选择游戏目录。");
            folderDialog.IsFolderPicker = true;
            if (folderDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                ShowStatusInfo("正打开游戏目录。");
                OpenGameByPath(folderDialog.FileName);
            }
        }

        /// <summary>
        /// 通过路径打开游戏。
        /// </summary>
        /// <param name="path"></param>
        public void OpenGameByPath(string path)
        {
            if (!gameManager.OpenGame(path))
            {
                MessageBox.Show("此文件夹不是有效的游戏路径");
                ShowStatusInfo("此文件夹不是有效的游戏路径");
            }
            else
            {
                fileAreaDirectory = GameManager.GameResourcePath;
                window.SetCurrentDirectory(fileAreaDirectory);
                ShowStatusInfo("打开游戏目录完成。");
                LoadDirectoryToFileArea(GameManager.GameResourcePath);
                studioConfig.LastGamePath = GameManager.GamePath;
                AddLegalControl();
            }
        }

        /// <summary>
        /// 由其他管理类使用，更改显示的游戏名称。
        /// </summary>
        public void SetGameName(string gameName)
        {
            window.SetGameName(gameName);
        }

        /// <summary>
        /// 返回到上一目录。
        /// </summary>
        public void ReturnLastDirectory()
        {
            if(fileAreaDirectory== GameManager.GameResourcePath)
            {
                ShowStatusInfo("当前已经是根目录。");
            }
            else
            {
                fileAreaDirectory = fileAreaDirectory.Substring(0, fileAreaDirectory.LastIndexOf('\\'));
                window.SetCurrentDirectory(fileAreaDirectory);
                LoadDirectoryToFileArea(fileAreaDirectory);
            }
        }

        /// <summary>
        /// 显示编辑器状态信息。
        /// </summary>
        /// <param name="info">想要显示的文字。</param>
        public void ShowStatusInfo(string info)
        {
            labelStatusInfo.Content = info;
            LogStatusInfo(info);
        }

        /// <summary>
        /// 保存记录到log文件。
        /// </summary>
        /// <param name="info">想要记录的文字。</param>
        public void LogStatusInfo(string info)
        {
            //未完成。
        }

        /// <summary>
        /// 打开文件。
        /// </summary>
        public void OpenFile()
        {
            if (gameManager.OpenFile(currentFilePath))
            {
                canvasManager.ClearAll();
                gameManager.RenderToCanvas(canvasManager);
                gameManager.ResetUIProperty();
                AddLegalControl();
            }
            else
            {
                ShowStatusInfo("文件打开失败。");
            }
        }
        
        /// <summary>
        /// 调用Win32窗体处理方法，使用前需要hook到主窗体上。
        /// 当前方法只用于帮助无边框wpf窗体调整边界大小。
        /// </summary>
        /// <param name="hwnd">窗体句柄</param>
        /// <param name="msg">触发的事件名称</param>
        /// <param name="wParam">参数</param>
        /// <param name="lParam">参数</param>
        /// <param name="handled">是否已经处理过</param>
        /// <returns></returns>
        public IntPtr WndResizeProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (FullScreen)
            {
                return hwnd;
            }
            switch ((Window32MessageEnum)msg)
            {
                case Window32MessageEnum.WM_NCHITTEST:
                    Point point = new Point();
                    point.X = (lParam.ToInt32() & 0xFFFF) * Common.Standard.ScaleX-window.Left;
                    point.Y = (lParam.ToInt32() >> 16)* Common.Standard.ScaleY- window.Top;
                    // 窗口左上角
                    if (point.Y <= studioConfig.BorderWidth+2
                       && point.X <= studioConfig.BorderWidth)
                    {
                        handled = true;
                        return new IntPtr((int)Window32HandleEnum.HTTOPLEFT);
                    }
                    // 窗口左下角
                    else if (point.X <= studioConfig.BorderWidth
                        && point.Y >= window.Height - studioConfig.BorderWidth)
                    {
                        handled = true;
                        return new IntPtr((int)Window32HandleEnum.HTBOTTOMLEFT);
                    }
                    // 窗口右上角
                    else if (point.Y <= studioConfig.BorderWidth
                       && point.X >= window.Width - studioConfig.BorderWidth)
                    {
                        handled = true;
                        return new IntPtr((int)Window32HandleEnum.HTTOPRIGHT);
                    }
                    // 窗口右下角
                    else if (point.X >= window.Width - studioConfig.BorderWidth
                       && point.Y >= window.Height - studioConfig.BorderWidth)
                    {
                        handled = true;
                        return new IntPtr((int)Window32HandleEnum.HTBOTTOMRIGHT);
                    }
                    // 窗口左侧
                    else if (point.X < studioConfig.BorderWidth)
                    {
                        handled = true;
                        return new IntPtr((int)Window32HandleEnum.HTLEFT);
                    }
                    // 窗口右侧
                    else if (point.X > window.Width - studioConfig.BorderWidth)
                    {
                        handled = true;
                        return new IntPtr((int)Window32HandleEnum.HTRIGHT);
                    }
                    // 窗口上方
                    else if (point.Y < studioConfig.BorderWidth)
                    {
                        handled = true;
                        return new IntPtr((int)Window32HandleEnum.HTTOP);
                    }
                    // 窗口下方
                    else if (point.Y > window.Height - studioConfig.BorderWidth)
                    {
                        handled = true;
                        return new IntPtr((int)Window32HandleEnum.HTBOTTOM);
                    }
                    else
                    {
                        return hwnd;
                    }
            }
            return hwnd;
        }
    }
}
