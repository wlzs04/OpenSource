﻿using System;
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
using LLGameStudio.Common.Config;
using LLGameStudio.Common.XML;
using LLGameStudio.Game.UI;
using LLGameStudio.Studio;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace LLGameStudio.Game
{
    public class GameManager
    {
        string gamePath = "";
        string gameConfigFilePath = "";
        static string gameResourcePath = "";
        GameConfig gameConfig;
        bool gameLoaded = false;
        StudioManager studioManager;
        Process gameProcess;
        public IUINode rootNode;
        public IUINode currentSelectUINode;
        string currentUINodeFilePath;

        static double gameWidth = 0;
        static double gameHeight = 0;

        public string GamePath { get => gamePath; }
        public bool GameLoaded { get => gameLoaded;}
        public string GameName { get => gameConfig.GameName; }
        public static string GameResourcePath { get => gameResourcePath; }
        public static double GameWidth { get => gameWidth; }
        public static double GameHeight { get => gameHeight; }

        public GameManager(StudioManager studioManager)
        {
            this.studioManager = studioManager;
            gameConfig = new GameConfig();
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
                System.Windows.MessageBox.Show("当前未加载游戏！");
                return;
            }
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
            gameConfigFilePath = gamePath + @"\" + "Game.xml";
            gameConfig.GameName = gameName;
            SaveConfig();
            gameResourcePath = gamePath + @"\" + "Resource";
            Directory.CreateDirectory(gameResourcePath);
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
                this.gamePath = gamePath;
                gameConfigFilePath = gamePath + @"\" + "Game.xml";
                LoadConfig();
                gameLoaded = true;
                gameResourcePath = gamePath + @"\" + "Resource";
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
                    canOpen=((LLGameLayout)uiNode).LoadContentFromFile(path);
                    break;
                case ".scene":
                    uiNode = new LLGameScene();
                    canOpen=((LLGameScene)uiNode).LoadContentFromFile(path);
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
    }
}

public enum GameUIFileEnum
{
    Folder,//文件夹
    Scene,//场景
    Layout,//布局
    Unknown,//未知
}

enum GameUIControlEnum
{
    Window,//窗体
    Button,//按钮
    Image,//图片
}
