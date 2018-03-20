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
	wstring startScene = L"";
	wstring resourcePath = L"Resource"; 
	wstring graphicsApi = L"Direct2D";
	bool openNetServer = false;
};

