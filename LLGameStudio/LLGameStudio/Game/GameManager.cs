using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Threading;
using LLGameStudio.Common;
using LLGameStudio.Common.Helper;
using LLGameStudio.Common.XML;
using LLGameStudio.Game.UI;
using LLGameStudio.Studio;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace LLGameStudio.Game
{
    public class GameManager
    {
        static string gamePath = "";
        string gameConfigFilePath = "";
        static string gameResourcePath = "";
        GameConfig gameConfig;
        bool gameLoaded = false;
        StudioManager studioManager;
        Process gameProcess;
        public IUINode rootNode;
        public IUINode currentSelectUINode;
        string currentUINodeFilePath;
        GameResourceEnum currentOpenResourceEnum= GameResourceEnum.Unknown;

        static double gameWidth = 0;
        static double gameHeight = 0;

        public string GameConfigPath { get => gameConfigFilePath; }
        public bool GameLoaded { get => gameLoaded; }
        public string GameName { get => gameConfig.GameName; }
        public GameResourceEnum CurrentOpenResourceEnum { get => currentOpenResourceEnum; }
        public static string GamePath { get => gamePath; }
        public static string GameResourcePath { get => gameResourcePath; }
        public static double GameWidth { get => gameWidth; }
        public static double GameHeight { get => gameHeight; }

        private static GameManager gameManager = null;

        private GameManager(StudioManager studioManager)
        {
            this.studioManager = studioManager;
            gameConfig = new GameConfig();
        }

        /// <summary>
        /// 初始化游戏管理器
        /// </summary>
        /// <param name="studioManager"></param>
        public static void InitGameManager(StudioManager studioManager)
        {
            if (gameManager == null)
            {
                gameManager = new GameManager(studioManager);
            }
        }

        /// <summary>
        /// 获得游戏管理器单例
        /// </summary>
        /// <returns></returns>
        public static GameManager GetSingleInstance()
        {
            return gameManager;
        }

        /// <summary>
        /// 让studioManager重新从当前选中的UI节点中加载属性到属性编辑区。
        /// </summary>
        public void ReLoadTransformProperty()
        {
            studioManager.ShowPropertyToEditorArea(currentSelectUINode);
        }

        /// <summary>
        /// 开始游戏。
        /// </summary>
        public void StartGame()
        {
            StopGame();
            if (!gameLoaded)
            {
                MessageBox.Show("当前未加载游戏！");
                return;
            }
            CopyGameExe(gamePath + @"\LLGameEngine.exe", gamePath + @"\" + gameConfig.GameName + ".exe");
            ShowStatusInfo("游戏：" + GameName + "正在启动。");
            gameProcess = Process.Start(gamePath + @"\" + gameConfig.GameName + ".exe");
            gameProcess.EnableRaisingEvents = true;
            var x=Dispatcher.CurrentDispatcher;
            gameProcess.Exited += (sender, e) => {
                x.BeginInvoke(
                    new Action(() => { StopGame(); }), DispatcherPriority.Normal);
            };
        }

        /// <summary>
        /// 拷贝游戏运行程序
        /// </summary>
        /// <param name="fileSPath"></param>
        /// <param name="fileSDPath"></param>
        void CopyGameExe(string fileSPath, string fileSDPath)
        {
            File.Copy(fileSPath, fileSDPath,true);
        }

        /// <summary>
        /// 显示当前状态到编辑器到状态显示区。
        /// </summary>
        /// <param name="s"></param>
        public void ShowStatusInfo(string s)
        {
            studioManager.ShowStatusInfo(s);
        }

        /// <summary>
        /// 停止游戏。
        /// </summary>
        public void StopGame()
        {
            if (gameProcess != null)
            {
                if(!gameProcess.HasExited)
                {
                    gameProcess.CloseMainWindow();
                    gameProcess.Close();
                }
                ShowStatusInfo("游戏：" + GameName + "已停止。");
            }
            gameProcess = null;
        }

        /// <summary>
        /// 创建新游戏目录。
        /// </summary>
        /// <param name="gamePath">游戏根目录</param>
        /// <param name="gameName">游戏名称</param>
        public void CreateGame(string gamePath, string gameName)
        {
            Directory.CreateDirectory(gamePath);
            GameConfig tempGameConfig = new GameConfig();
            tempGameConfig.GameName = gameName;
            LLConvert.ExportContentToXML(gamePath + @"\" + "Game.xml", tempGameConfig);
            gameResourcePath = gamePath + @"\" + "Resource";
            FileHelper.CopyDirectory(@"DefaultContent\Resource", gamePath);
            FontManager.CreateFontConfig(gamePath + @"\" + "Font.xml");
            File.Copy("../../../x64/Debug/LLGameEngine.exe", gamePath + @"\LLGameEngine.exe", true);
            gameLoaded = true;
        }

        /// <summary>
        /// 打开游戏目录。
        /// </summary>
        /// <param name="gamePath">游戏路径</param>
        /// <returns></returns>
        public bool OpenGame(string gamePath)
        {
            if (IsLegalGamePath(gamePath))
            {
                GameManager.gamePath = gamePath;
                gameConfigFilePath = gamePath + @"\" + "Game.xml";
                LoadConfig();
                gameLoaded = true;
                gameResourcePath = gamePath + @"\" + "Resource";

                FontManager.LoadFontConfig(gamePath + @"\" + "Font.xml");

                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断传入路径下文件结构是否符合要求。
        /// </summary>
        /// <param name="gamePath">游戏路径</param>
        /// <returns></returns>
        public bool IsLegalGamePath(string gamePath)
        {
            if (Directory.Exists(gamePath) && File.Exists(gamePath + @"\" + "Game.xml")&& Directory.Exists(gamePath+@"\"+"Resource"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 保存游戏文件
        /// </summary>
        public void SaveGame()
        {
            if(rootNode != null)
            {
                if(rootNode is LLGameScene)
                {
                    LLConvert.ExportContentToXML(currentUINodeFilePath, rootNode);
                }
                else if(rootNode is LLGameLayout)
                {
                    LLGameLayout lLGameLayout = rootNode as LLGameLayout;
                    LLConvert.ExportContentToXML(currentUINodeFilePath, lLGameLayout.llGameGrid);
                }
            }
        }

        /// <summary>
        /// 加载游戏配置。
        /// </summary>
        public void LoadConfig()
        {
            gameConfig = new GameConfig();
            LLConvert.LoadContentFromXML(gameConfigFilePath, gameConfig);
            gameWidth = gameConfig.Width;
            gameHeight = gameConfig.Height;
            studioManager.SetGameName(gameConfig.GameName);
        }

        /// <summary>
        /// 保存游戏配置。
        /// </summary>
        public void SaveConfig()
        {
            LLConvert.ExportContentToXML(gameConfigFilePath, gameConfig);
        }

        /// <summary>
        /// 打开文件。
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool OpenFile(string path)
        {
            IUINode uiNode=null;
            bool canOpen = false;
            switch (System.IO.Path.GetExtension(path))
            {
                case ".layout":
                    uiNode = new LLGameLayout();
                    currentOpenResourceEnum = GameResourceEnum.Layout;
                    canOpen =((LLGameLayout)uiNode).LoadContentFromFile(path);
                    break;
                case ".scene":
                    uiNode = new LLGameScene();
                    currentOpenResourceEnum = GameResourceEnum.Scene;
                    canOpen =((LLGameScene)uiNode).LoadContentFromFile(path);
                    break;
                default:
                    break;
            }
            if (canOpen)
            {
                currentUINodeFilePath = path;
                rootNode = uiNode;
                currentSelectUINode = rootNode;
                return true;
            }
            currentOpenResourceEnum = GameResourceEnum.Unknown;
            return false;
        }
        
        /// <summary>
        /// 将当前UI根节点添加到画布管理器上并重新设置
        /// </summary>
        /// <param name="canvasManager"></param>
        public void RenderToCanvas(CanvasManager canvasManager)
        {
            canvasManager.AddRootUINode(rootNode);
            studioManager.TreeResetItem();
        }

        /// <summary>
        /// 重新设置UI根节点信息
        /// </summary>
        public void ResetUIProperty()
        {
            rootNode.ResetUIProperty();
        }

        /// <summary>
        /// 选择此节点并同步到UI层级树。
        /// </summary>
        /// <param name="currentUINode"></param>
        public void SelectUINode(IUINode currentUINode)
        {
            currentSelectUINode = currentUINode;
            studioManager.SelectUINodeToTree(currentUINode);
        }

        /// <summary>
        /// 添加控件到布局上
        /// </summary>
        /// <param name="uiNode"></param>
        public void AddControlToLayout(IUINode uiNode)
        {
            if (currentSelectUINode != null)
            {
                currentSelectUINode.AddNode(uiNode);
                StudioManager.GetSingleInstance().TreeResetItem();
                uiNode.ResetUIProperty();
                CanvasManager.GetSingleInstance().SelectUINode(uiNode);
                SelectUINode(uiNode);
                CanvasManager.GetSingleInstance().SetEventForUINode(uiNode);
            }
        }
        
    }
}

/// <summary>
/// 游戏资源文件类型
/// </summary>
public enum GameResourceEnum
{
    Folder,//文件夹
    Scene,//场景
    Layout,//布局
    Particle,//粒子
    Actor,//角色
    Script,//脚本
    Physics,//物理
    Unknown,//未知
}

