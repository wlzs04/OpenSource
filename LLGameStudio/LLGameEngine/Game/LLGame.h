﻿#pragma once
#include "../Common/XML/LLXML.h"
#include "../Common/Window/LLGameWindow.h"
#include "../Common/Config/GameConfig.h"
#include "../Common/Helper/MessageHelper.h"
#include "../Common/Helper/SystemHelper.h"
#include "UI/LLGameScene.h"
#include "../Common/Graphics/Direct2D/Direct2DApi.h"
#include "GameTimer.h"
#include "../Game/UI/LLGameText.h"
#include "../Game/UI/LLGameButton.h"
#include "../Game/UI/LLGameImage.h"
#include "../Common/Net/LLGameNetClient.h"

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
	//初始化自定义数据
	virtual void InitUserData() {};
	//加载配置
	void LoadConfig();
	//保存配置
	void SaveConfig();
	//更新
	void Update();
	//渲染
	void Render();
	void OnRunEvent();
	void OnLeftMouseDown(void* iuiNode, int i);
	void OnLeftMouseUp(void* iuiNode, int i);
	void OnRightMouseDown(void* iuiNode, int i);
	void OnRightMouseUp(void* iuiNode, int i);
	void OnMouseOver(void* iuiNode, int i);
	void OnMouseWheel(void* iuiNode, int i);

	wstring currentPath;
	GameConfig gameConfig;
	LLGameWindow* gameWindow;
	bool gameExit = false;
	LLGameScene* gameScene;
	GameTimer gameTimer;
	LLGameNetClient* gameNetClient;
};