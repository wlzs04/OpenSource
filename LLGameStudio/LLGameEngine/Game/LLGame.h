#pragma once
#include "../Common/XML/LLXML.h"
#include "../Common/Window/LLGameWindow.h"
#include "../Common/Config/GameConfig.h"
#include "../Common/Helper/MessageHelper.h"
#include "../Common/Helper/SystemHelper.h"
#include "UI/LLGameScene.h"
#include "../Common/Graphics/Direct2D/Direct2DApi.h"
#include "GameTimer.h"

class LLGame
{
public:
	LLGame();
	~LLGame();
	//初始化
	void Init();
	//游戏启动
	void Start();
protected:
	//初始化窗体
	void InitWindow();
	//初始化数据
	virtual void InitData();
	//加载配置
	void LoadConfig();
	//保存配置
	void SaveConfig();
	//更新
	void Update();
	//渲染
	void Render();
	void OnRunEvent();

	wstring currentPath;
	GameConfig gameConfig;
	LLGameWindow* gameWindow;
	bool gameExit = false;
	LLGameScene* gameScene;
	GameTimer gameTimer;
};