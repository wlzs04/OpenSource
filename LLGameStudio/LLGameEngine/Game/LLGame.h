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

	void Init();
	void Start();
private:
	void InitWindow();
	void InitData();
	void LoadConfig();
	void SaveConfig();
	wstring currentPath;
	GameConfig gameConfig;
	LLGameWindow* gameWindow;
};