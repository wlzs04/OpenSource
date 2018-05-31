using LLGameStudio.Common;
using LLGameStudio.Common.DataType;
using LLGameStudio.Common.Helper;
using LLGameStudio.Game.Actor;
using LLGameStudio.Studio.Control;
using System;
using System.Collections.Generic;
using System.IO;
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
        bool isAddIKFromBoneToParentBone = false;
        LLStudioBone rootBoneControl = null;
        LLStudioBone currentSelectBoneControl = null;
        LLStudioTransformAxis transformAxis = null;
        LLStudioTimeline timeLine = null;
        Game.Actor.Action currentAction = null;
        Dictionary<string, LLStudioKeyItem> keyItemMap = new Dictionary<string, LLStudioKeyItem>();

        Dictionary<LLStudioBone, LLStudioBone> ikMap=new Dictionary<LLStudioBone, LLStudioBone>();

        bool isEditBone = true;//界面打开时默认编辑骨骼，false情况下为编辑骨骼动画。

        int ikCyclicNumber = 3;

        public ActorWindow(string filePath)
        {
            InitializeComponent();
            this.filePath = filePath;
            textBoxActorName.Text = System.IO.Path.GetFileNameWithoutExtension(filePath);
            actor = new Actor(textBoxActorName.Text);
            transformAxis = new LLStudioTransformAxis(canvas);
            transformAxis.DragAxisEvent += DragAxisEvent;
            transformAxis.SetTransformType(TransformType.Tranlation);
            gridTimeLineArea.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// 坐标轴的拖拽事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="moveVector"></param>
        void DragAxisEvent(object sender, Vector2 moveVector)
        {
            switch (transformAxis.GetTransformType())
            {
                case TransformType.Tranlation:
                    Point point = currentSelectBoneControl.GetStartPoint();
                    point.X += moveVector.X;
                    point.Y += moveVector.Y;

                    if (ikMap.ContainsKey(currentSelectBoneControl))
                    {
                        point = Mouse.GetPosition(canvas);
                        
                        for (int i = 0; i < ikCyclicNumber; i++)
                        {
                            LLStudioBone tempBoneControl = currentSelectBoneControl;
                            while (tempBoneControl != null)
                            {
                                double tempAngle = tempBoneControl.GetBoneControlAngle();
                                Point tempPoint = tempBoneControl.GetStartPoint();
                                Point sPoint = currentSelectBoneControl.GetBoneEndPosition();
                                Vector2 dv = new Vector2(point.X - tempPoint.X, point.Y - tempPoint.Y);
                                Vector2 sv =new Vector2(sPoint.X - tempPoint.X, sPoint.Y - tempPoint.Y);

                                double vectorAngle = LLMath.GetAngleBetweenVectors(sv, dv);
                                
                                tempBoneControl.SetBoneAngle(tempBoneControl.GetBoneAngle() + vectorAngle);
                                if(tempBoneControl == ikMap[currentSelectBoneControl])
                                {
                                    break;
                                }
                                tempBoneControl = tempBoneControl.parentBoneControl;
                            }
                        }
                    }
                    else
                    {
                        if (currentSelectBoneControl.parentBoneControl != null)
                        {
                            currentSelectBoneControl.parentBoneControl.ChangeTransformByChildBone(point);
                            foreach (var item in currentSelectBoneControl.parentBoneControl.listBoneControl)
                            {
                                item.SetPostion(point);
                                item.ResetBoneAngleByBoneControlAngle();
                            }
                        }
                        else
                        {
                            currentSelectBoneControl.SetPostion(point);
                        }
                    }
                    break;
                case TransformType.Rotation:
                    double angle = moveVector.X;
                    currentSelectBoneControl.SetBoneAngle(currentSelectBoneControl.GetBoneAngle() + angle);
                    break;
                case TransformType.Scale:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 添加骨骼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AddBone(object sender, RoutedEventArgs e)
        {
            AddBoneToCanvas(new Bone());
        }

        void AddIK(object sender, RoutedEventArgs e)
        {
            if (currentSelectBoneControl != null)
            {
                currentSelectBoneControl.CancelSelectState();
            }
            currentSelectBoneControl = ((sender as MenuItem).Parent as ContextMenu).PlacementTarget as LLStudioBone;
            currentSelectBoneControl.SetSelectState();
            isAddIKFromBoneToParentBone = true;
        }

        void RemoveIK(object sender, RoutedEventArgs e)
        {
            if (currentSelectBoneControl != null)
            {
                currentSelectBoneControl.CancelSelectState();
            }
            currentSelectBoneControl = ((sender as MenuItem).Parent as ContextMenu).PlacementTarget as LLStudioBone;
            currentSelectBoneControl.SetSelectState();
            if (ikMap.ContainsKey(currentSelectBoneControl))
            {
                ikMap.Remove(currentSelectBoneControl);
                actor.RemoveIK(currentSelectBoneControl.bone);
            }
        }

        /// <summary>
        /// 添加骨骼到画布中
        /// </summary>
        /// <param name="bone"></param>
        /// <returns></returns>
        LLStudioBone AddBoneToCanvas(Bone bone)
        {
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

            MenuItem addIKMenuItem = new MenuItem();
            addIKMenuItem.Header = "添加IK解算";
            addIKMenuItem.Click += AddIK;
            boneContextMenu.Items.Add(addIKMenuItem);

            MenuItem removeIKMenuItem = new MenuItem();
            removeIKMenuItem.Header = "删除IK解算";
            removeIKMenuItem.Click += RemoveIK;
            boneContextMenu.Items.Add(removeIKMenuItem);

            boneControl.ContextMenu = boneContextMenu;

            foreach (var item in bone.listBone)
            {
                LLStudioBone childBoneControl = AddBoneToCanvas(item);
                boneControl.listBoneControl.Add(childBoneControl);
                childBoneControl.parentBoneControl = boneControl;
            }
            if(currentSelectBoneControl!=null)
            {
                currentSelectBoneControl.CancelSelectState();
            }
            currentSelectBoneControl = boneControl;
            currentSelectBoneControl.SetSelectState();
            return boneControl;
        }

        /// <summary>
        /// 将当前选中骨骼添加到父骨骼中。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AddBoneToParentBone(object sender, RoutedEventArgs e)
        {
            if (currentSelectBoneControl != null)
            {
                currentSelectBoneControl.CancelSelectState();
            }
            currentSelectBoneControl = ((sender as MenuItem).Parent as ContextMenu).PlacementTarget as LLStudioBone;
            currentSelectBoneControl.SetSelectState();
            isAddBoneToParentBone = true;
        }

        /// <summary>
        /// 重新加载角色骨骼层级树。
        /// </summary>
        void TreeResetItem()
        {
            treeViewUILayer.Items.Clear();

            LLStudioTreeViewItem treeViewItem = new LLStudioTreeViewItem();
            treeViewItem.IsExpanded = true;
            treeViewItem.MouseDoubleClick += SelectBoneByTreeView;
            treeViewItem.SetUINodeItem(rootBoneControl);
            treeViewItem.Header = rootBoneControl.bone.name.Value;
            AddNodeToTree(rootBoneControl, treeViewItem);
            treeViewUILayer.Items.Add(treeViewItem);
        }

        /// <summary>
        /// 将骨骼节点添加到树层级中。
        /// </summary>
        /// <param name="bone"></param>
        /// <param name="rootItem"></param>
        void AddNodeToTree(LLStudioBone boneControl, LLStudioTreeViewItem rootItem)
        {
            foreach (var item in boneControl.listBoneControl)
            {
                LLStudioTreeViewItem treeViewItem = new LLStudioTreeViewItem();
                treeViewItem.IsExpanded = true;
                treeViewItem.MouseDoubleClick += SelectBoneByTreeView;
                treeViewItem.SetUINodeItem(item);
                treeViewItem.Header = item.bone.name.Value;
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
            LLStudioTreeViewItem treeViewItem = sender as LLStudioTreeViewItem;
            if (treeViewItem == treeViewUILayer.SelectedItem)
            {
                SelectBone(treeViewItem.GetUINode() as LLStudioBone);
            }
        }

        /// <summary>
        /// 通过点击控件选中骨骼触发的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SelectBoneByControl(object sender, MouseButtonEventArgs e)
        {
            SelectBone((sender as LLStudioBone));
            SelectUINodeToTree();
        }

        /// <summary>
        /// 将选中的骨骼节点同步到UI层级树中。
        /// </summary>
        /// <param name="currentUINode"></param>
        /// <param name="treeView"></param>
        public void SelectUINodeToTree(LLStudioTreeViewItem treeView = null)
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
                if (item.GetUINode() == currentSelectBoneControl)
                {
                    item.IsSelected = true;
                    LLStudioTreeViewItem parentTreeViewItem = item.Parent as LLStudioTreeViewItem;
                    while (parentTreeViewItem != null)
                    {
                        parentTreeViewItem.IsExpanded = true;
                        parentTreeViewItem = parentTreeViewItem.Parent as LLStudioTreeViewItem;
                    }
                    return;
                }
                SelectUINodeToTree(item);
            }
        }

        /// <summary>
        /// 选中骨骼
        /// </summary>
        /// <param name="boneControl"></param>
        void SelectBone(LLStudioBone boneControl)
        {
            if (isAddBoneToParentBone)
            {
                currentSelectBoneControl.SetPostion(boneControl.GetBoneEndPosition());
                boneControl.AddBoneControl(currentSelectBoneControl);
                transformAxis.SetPosition(boneControl.GetBoneEndPosition());
                isAddBoneToParentBone = false;
            }
            else if(isAddIKFromBoneToParentBone)
            {
                if(boneControl.IsMyChildBoneControl(currentSelectBoneControl))
                {
                    ikMap[currentSelectBoneControl] = boneControl;

                    actor.AddIK(currentSelectBoneControl.bone, boneControl.bone);
                }

                isAddIKFromBoneToParentBone = false;
            }
            else
            {
                transformAxis.Visibility = Visibility.Visible;
                Panel.SetZIndex(transformAxis, 1);
                transformAxis.SetPosition(boneControl.GetStartPoint());
                currentSelectBoneControl.CancelSelectState();
                currentSelectBoneControl = boneControl;
                currentSelectBoneControl.SetSelectState();
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
        private void imageSaveActor_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SaveActorToFile();
        }

        /// <summary>
        /// 点击设置骨骼默认姿势按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imageSetDefaultPosture_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            actor.SetDefaultPosture();
        }

        /// <summary>
        /// 点击添加动作按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imageAddAction_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Game.Actor.Action action = new Game.Actor.Action();
            actor.AddAction(action);
            LLStudioActionItem actionItem = new LLStudioActionItem(action);
            actionItem.MouseDoubleClick += LLStudioActionItem_MouseDoubleClick;
            stackPanelActionArea.Children.Add(actionItem);
        }
        
        /// <summary>
        /// 点击进入骨骼编辑模式按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imageEnterBoneEdit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            EnterBoneEdit();
        }

        /// <summary>
        /// 点击进入动作编辑模式按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imageEnterActionEdit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            EnterActionEdit();
        }

        private void LLStudioActionItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(isEditBone)
            {
                
            }
            else
            {
                LoadActionToCanvas((sender as LLStudioActionItem).GetAction());
            }
        }

        private void imageTranslation_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            transformAxis.SetTransformType(TransformType.Tranlation);
        }

        private void imageRotation_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            transformAxis.SetTransformType(TransformType.Rotation);
        }

        /// <summary>
        /// 从文件中读取角色信息
        /// </summary>
        void LoadActorFromFile()
        {
            LLConvert.LoadContentFromXML(filePath, actor);
            rootBoneControl=AddBoneToCanvas(actor.rootBone);

            rootBoneControl.ResetBoneControlAngleByBoneAngle();
            rootBoneControl.SetPostion(canvas.ActualWidth *0.5, canvas.ActualHeight *0.8);
            ikMap.Clear();
            foreach (var item in actor.ikMap)
            {
                ikMap[GetBoneControlByName(rootBoneControl, item.Key.name.Value)] = GetBoneControlByName(rootBoneControl, item.Value.name.Value);
            }
        }

        /// <summary>
        /// 将角色信息保存到文件
        /// </summary>
        void SaveActorToFile()
        {
            LLConvert.ExportContentToXML(filePath, actor);
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

            MenuItem addBoneMenuItem = new MenuItem();
            addBoneMenuItem.Header = "添加骨骼";
            addBoneMenuItem.Click += AddBone;
            gridContextMenu.Items.Add(addBoneMenuItem);
            
            canvas.ContextMenu = gridContextMenu;

            canvas.Children.Add(transformAxis);
            canvas.UpdateLayout();
            SelectBone(rootBoneControl);
            SelectUINodeToTree();

            timeLine = new LLStudioTimeline();
            timeLine.DragTimeBlackEvent += DragTimeBlackEvent;
            timeLine.KeyFrameEvent += KeyFrameEvent;
            timeLine.TimeRunEvent += TimeRunEvent;
            gridTimeLineArea.Children.Add(timeLine);
            gridTimeLineArea.UpdateLayout();
        }

        void DragTimeBlackEvent(int scale)
        {
            if (timeLine.GetTimeState() != TimeState.Start)
            {
                Game.Actor.Frame frame = currentAction.GetFrameByNumber(scale);

                if (frame == null)
                {
                    frame = currentAction.CalculateFrameByFrameNumber(scale);
                }
                foreach (var item in frame.listBone)
                {
                    LLStudioBone boneControl = GetBoneControlByName(rootBoneControl, item.name.Value);
                    boneControl.SetBoneAngle(item.angle);
                }
            }
        }

        void KeyFrameEvent(int scale)
        {
            Game.Actor.Frame frame = new Game.Actor.Frame();
            frame.frameNumber.Value = scale;

            foreach (var item in timeLine.listKeyItem)
            {
                Bone bone = new Bone();
                bone.RecordActionByBone(((LLStudioBone)item.GetRelation()).bone);
                frame.listBone.Add(bone);
            }

            currentAction.AddFrame(frame);
        }

        void TimeRunEvent(double time)
        {
            Game.Actor.Frame frame = currentAction.CalculateFrameByTime(time);

            foreach (var item in frame.listBone)
            {
                LLStudioBone boneControl = GetBoneControlByName(rootBoneControl, item.name.Value);
                Dispatcher.Invoke(new System.Action(()=> { boneControl.SetBoneAngle(item.angle); }));
            }
        }

        public LLStudioBone GetBoneControlByName(LLStudioBone boneControl,string name)
        {
            if (boneControl.bone.name.Value == name)
            {
                return boneControl;
            }
            foreach (var item in boneControl.listBoneControl)
            {
                
                LLStudioBone bonecontrol = GetBoneControlByName(item, name);
                if (bonecontrol!=null)
                {
                    return bonecontrol;
                }
            }
            return null;
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                    transformAxis.SetTransformType(TransformType.Tranlation);
                    break;
                case Key.E:
                    transformAxis.SetTransformType(TransformType.Rotation);
                    break;
                default:
                    break;
            }
        }

        private void canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            currentMouseSelectPosition = e.GetPosition(canvas);
        }

        /// <summary>
        /// 将骨骼动画加载到画布中，开始编辑骨骼动画。
        /// </summary>
        void LoadActionToCanvas(Game.Actor.Action action)
        { 
            currentAction=action;
            timeLine.SetTimeLimit(action.totalTime.Value);
            timeLine.SetScaleLimit(action.totalFrameNumber.Value);
            timeLine.ResetTimeLine();
            timeLine.RemoveAllKeyItemFlag();
            foreach (var frame in action.sortedListFrame)
            {
                foreach (var bone in frame.Value.listBone)
                {
                    LLStudioKeyItem keyItem = timeLine.GetKeyItemByBoneName(bone.name.Value);
                    keyItem.AddKeyFlag(frame.Key);
                }
            }
        }

        /// <summary>
        /// 将骨骼及其子骨骼添加到时间轴中
        /// </summary>
        /// <param name="boneControl"></param>
        /// <param name="level"></param>
        void AddBoneAddToTimeLine(LLStudioBone boneControl,int level)
        {
            LLStudioKeyItem keyItem = new LLStudioKeyItem(timeLine,boneControl.bone.name.Value, level);
            keyItem.SetRelation(boneControl);
            keyItem.MouseLeftButtonDown += SelectKeyItem;
            timeLine.AddKeyItem(keyItem);
            keyItemMap.Add(boneControl.bone.name.Value, keyItem);
            foreach (var item in boneControl.listBoneControl)
            {
                AddBoneAddToTimeLine(item, level + 1);
            }
        }

        /// <summary>
        /// 选中某项同时取消其他项的选中状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SelectKeyItem(object sender, MouseButtonEventArgs e)
        {
            foreach (var item in keyItemMap)
            {
                item.Value.CancelSelectState();
            }
            (sender as LLStudioKeyItem).SetSelectState();
        }

        /// <summary>
        /// 进入骨骼编辑模式
        /// </summary>
        void EnterBoneEdit()
        {
            if(!isEditBone)
            {
                isEditBone = true;
                gridTimeLineArea.Visibility = Visibility.Hidden;
                timeLine.RemoveAllKeyItem();
                keyItemMap.Clear();
                labelEditState.Content = "骨骼编辑";
                stackPanelActionArea.Children.Clear();
            }
        }

        /// <summary>
        /// 进入动作编辑模式
        /// </summary>
        void EnterActionEdit()
        {
            if(isEditBone)
            {
                isEditBone = false;
                gridTimeLineArea.Visibility = Visibility.Visible;
                AddBoneAddToTimeLine(rootBoneControl, 1);
                labelEditState.Content = "动作编辑";
                stackPanelActionArea.Children.Clear();
                foreach (var item in actor.listAction)
                {
                    LLStudioActionItem actionItem = new LLStudioActionItem(item);
                    actionItem.MouseDoubleClick += LLStudioActionItem_MouseDoubleClick;
                    stackPanelActionArea.Children.Add(actionItem);
                }
            }
        }
    }
}
