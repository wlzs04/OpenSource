using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        GameConfig gameConfig;
        bool gameLoaded = false;
        StudioManager studioManager;
        Process gameProcess;

        public bool GameLoaded { get => gameLoaded;}
        public string GameName { get => gameConfig.GameName; }

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
        }

        public void StopGame()
        {
            if (gameProcess != null && !gameProcess.HasExited)
            {
                studioManager.ShowStatusInfo("游戏：" + GameName + "已停止。");
                gameProcess.CloseMainWindow();
                gameProcess.Close();
            }
            gameProcess = null;
        }

        public void CreateGame(string gamePath, string gameName)
        {
            Directory.CreateDirectory(gamePath);
            gameConfigFilePath = gamePath + @"\" + "Game.xml";
            gameConfig.GameName = gameName;
            SaveConfig();
        }

        public bool OpenGame(string gamePath)
        {
            gameConfigFilePath = gamePath + @"\" + "Game.xml";
            if (Directory.Exists(gamePath) && File.Exists(gameConfigFilePath))
            {
                this.gamePath = gamePath;

                LoadConfig();
                gameLoaded = true;
                return true;
            }
            return false;
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
