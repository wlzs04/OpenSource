using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using LLGameStudio.Common.Config;
using LLGameStudio.Common.XML;
using LLGameStudio.Studio;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace LLGameStudio.Game
{
    class GameManager
    {
        string gamePath = "";
        string gameConfigFilePath = "";
        string gameResourcePath = "";
        GameConfig gameConfig;
        bool gameLoaded = false;
        StudioManager studioManager;
        Process gameProcess;
        
        public bool GameLoaded { get => gameLoaded;}
        public string GameName { get => gameConfig.GameName; }
        public string GameResourcePath { get => gameResourcePath; }
        public int GameWidth { get => gameConfig.Width; }
        public int GameHeight { get => gameConfig.Height; }

        public GameManager(StudioManager studioManager)
        {
            this.studioManager = studioManager;
            gameConfig = new GameConfig();
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
            studioManager.ShowStatusInfo("游戏：" + GameName + "正在启动。");
            gameProcess = Process.Start(gamePath + @"\" + gameConfig.GameName + ".exe");
            gameProcess.EnableRaisingEvents = true;
            var x=Dispatcher.CurrentDispatcher;
            gameProcess.Exited += (sender, e) => {
                x.BeginInvoke(
                    new Action(() => { StopGame(); }), DispatcherPriority.Normal);
            };
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
                studioManager.ShowStatusInfo("游戏：" + GameName + "已停止。");
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

        }

        /// <summary>
        /// 加载游戏配置。
        /// </summary>
        public void LoadConfig()
        {
            LLXMLConverter converter = new LLXMLConverter();
            gameConfig = new GameConfig();
            converter.LoadContentFromXML(gameConfigFilePath, gameConfig);
        }

        /// <summary>
        /// 保存游戏配置。
        /// </summary>
        public void SaveConfig()
        {
            LLXMLConverter converter = new LLXMLConverter();
            converter.ExportContentToXML(gameConfigFilePath, gameConfig);
        }
    }
}
