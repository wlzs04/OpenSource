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

        public GameManager(StudioManager studioManager)
        {
            this.studioManager = studioManager;
            gameConfig = new GameConfig();
        }

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

        public bool IsLegalGamePath(string gamePath)
        {
            if (Directory.Exists(gamePath) && File.Exists(gamePath + @"\" + "Game.xml")&& Directory.Exists(gamePath+@"\"+"Resource"))
            {
                return true;
            }
            return false;
        }

        public void SaveGame()
        {

        }

        public void LoadConfig()
        {
            LLXMLConverter converter = new LLXMLConverter();
            gameConfig = new GameConfig();
            converter.LoadContentFromXML(gameConfigFilePath, gameConfig);
        }

        public void SaveConfig()
        {
            LLXMLConverter converter = new LLXMLConverter();
            converter.ExportContentToXML(gameConfigFilePath, gameConfig);
        }
    }
}
