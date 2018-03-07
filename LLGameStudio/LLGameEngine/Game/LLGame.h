#pragma once
#include "../Common/XML/LLXML.h"
#include "../Common/Window/LLGameWindow.h"
#include "../Common/Config/GameConfig.h"
#include "../Common/Helper/MessageHelper.h"
#include "../Common/Helper/SystemHelper.h"

class LLGame
{
public:
	LLGame();
	~LLGame();
	//初始化
	void Init();
	//游戏启动
	void Start();
private:
	//初始化窗体
	void InitWindow();
	//初始化数据
	void InitData();
	//加载配置
	void LoadConfig();
	//保存配置
	void SaveConfig();
	wstring currentPath;
	GameConfig gameConfig;
	LLGameWindow* gameWindow;
};