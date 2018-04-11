using LLGameStudio.Common;
using LLGameStudio.Common.DataType;
using LLGameStudio.Game.Actor;
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
    /// ActorWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ActorWindow : System.Windows.Window
    {
        string filePath = "";
        TreeView treeViewUILayer = new TreeView();
        Actor actor;
        Point currentMouseSelectPosition;
        bool isAddBoneToParentBone = false;
        LLStudioBone lastSelectBoneControl = null;
        LLStudioTransformAxis transformAxis = null;

        public ActorWindow(string filePath)
        {
            InitializeComponent();
            this.filePath = filePath;
            textBoxParticleName.Text = System.IO.Path.GetFileNameWithoutExtension(filePath);
            actor = new Actor(textBoxParticleName.Text);
            transformAxis = new LLStudioTransformAxis(canvas);
            transformAxis.DragAxisEvent += DragAxisEvent;
        }

        void DragAxisEvent(object sender, Vector2 moveVector)
        {
            Point point = lastSelectBoneControl.GetStartPoint();
            lastSelectBoneControl.SetPostion(point.X+moveVector.X, point.Y + moveVector.Y);
        }

        /// <summary>
        /// 添加骨骼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AddBone(object sender, RoutedEventArgs e)
        {
            Bone bone = new Bone("bone");
            bone.position.X = currentMouseSelectPosition.X / canvas.ActualWidth;
            bone.position.Y = currentMouseSelectPosition.Y / canvas.ActualHeight;
            LLStudioBone boneControl = new LLStudioBone(bone);
            canvas.Children.Add(boneControl);
            canvas.UpdateLayout();
            boneControl.SetPostion(currentMouseSelectPosition.X, currentMouseSelectPosition.Y);
            boneControl.MouseLeftButtonDown += SelectBoneByControl;

            ContextMenu boneContextMenu = new ContextMenu();
            MenuItem addBoneToParentItem = new MenuItem();
            addBoneToParentItem.Header = "附加到父骨骼";
            addBoneToParentItem.Click += AddBoneToParentBone;
            boneContextMenu.Items.Add(addBoneToParentItem);
            boneControl.ContextMenu = boneContextMenu;
        }

        /// <summary>
        /// 将当前选中骨骼添加到父骨骼中。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AddBoneToParentBone(object sender, RoutedEventArgs e)
        {
            lastSelectBoneControl = ((sender as MenuItem).Parent as ContextMenu).PlacementTarget as LLStudioBone;
            isAddBoneToParentBone = true;
        }

        /// <summary>
        /// 重新加载角色骨骼层级树。
        /// </summary>
        void TreeResetItem()
        {
            treeViewUILayer.Items.Clear();

            TreeViewItem treeViewItem = new TreeViewItem();
            treeViewItem.IsExpanded = true;
            treeViewItem.MouseDoubleClick += SelectBoneByTreeView;
            treeViewItem.Header = actor.rootBone.name;
            AddNodeToTree(actor.rootBone, treeViewItem);
            treeViewUILayer.Items.Add(treeViewItem);
        }

        /// <summary>
        /// 将骨骼节点添加到树层级中。
        /// </summary>
        /// <param name="bone"></param>
        /// <param name="rootItem"></param>
        void AddNodeToTree(Bone bone, TreeViewItem rootItem)
        {
            foreach (var item in bone.listBone)
            {
                TreeViewItem treeViewItem = new TreeViewItem();
                treeViewItem.IsExpanded = true;
                treeViewItem.MouseDoubleClick += SelectBoneByTreeView;
                treeViewItem.Header = item.name;
                AddNodeToTree(item, treeViewItem);
                rootItem.Items.Add(treeViewItem);
            }
        }

        /// <summary>
        /// 通过双击树节点选中骨骼触发的事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SelectBoneByTreeView(object sender, MouseButtonEventArgs e)
        {

        }

        /// <summary>
        /// 通过点击控件选中骨骼触发的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SelectBoneByControl(object sender, MouseButtonEventArgs e)
        {
            LLStudioBone boneControl = (sender as LLStudioBone);
            if(isAddBoneToParentBone)
            {
                Bone bone = boneControl.bone;
                bone.AddBone(lastSelectBoneControl.bone);
                lastSelectBoneControl.SetPostion(boneControl.GetBoneEndPosition());
                isAddBoneToParentBone = false;
            }
            else
            {
                transformAxis.Visibility = Visibility.Visible;
                Panel.SetZIndex(transformAxis,1);
                transformAxis.SetPosition(boneControl.GetStartPoint());
            }
            lastSelectBoneControl = boneControl;
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
        private void imageSaveActor_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SaveActorToFile();
        }

        /// <summary>
        /// 从文件中读取角色信息
        /// </summary>
        void LoadActorFromFile()
        {
            LLConvert.LoadContentFromXML(filePath, actor);

            LLStudioBone boneControl = new LLStudioBone(actor.rootBone);
            canvas.Children.Add(boneControl);
            canvas.UpdateLayout();
            boneControl.SetPostion(canvas.ActualWidth / 2, canvas.ActualHeight / 2);
            boneControl.MouseLeftButtonDown += SelectBoneByControl;
        }

        /// <summary>
        /// 将角色信息保存到文件
        /// </summary>
        void SaveActorToFile()
        {

        }

        private void gridActor_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            currentMouseSelectPosition = e.GetPosition(canvas);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadActorFromFile();

            treeViewUILayer = new TreeView();
            treeViewUILayer.BorderThickness = new Thickness(0);
            treeViewUILayer.Background = null;
            treeViewUILayer.Margin = new Thickness(0, 25, 0, 0);
            TreeResetItem();
            gridBoneLayer.Children.Add(treeViewUILayer);

            ContextMenu gridContextMenu = new ContextMenu();
            MenuItem addParticleEmitter = new MenuItem();
            addParticleEmitter.Header = "添加骨骼";
            addParticleEmitter.Click += AddBone;
            gridContextMenu.Items.Add(addParticleEmitter);
            gridActor.ContextMenu = gridContextMenu;

            canvas.Children.Add(transformAxis);
            transformAxis.Visibility = Visibility.Hidden;
        }
    }
}
