#pragma once
#include <string>

using namespace std;

class GameConfig
{
public:
	GameConfig();
	~GameConfig();

	int width = 800;
	int height = 600;
	wstring gameName = L"Game";
	bool fullScreen = false;
	bool canMultiGame = false;
};

