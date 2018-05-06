#pragma once
#include <string>
#include <windows.h>
#include "..\..\Game\UI\IUIProperty.h"
#include "..\XML\LLXML.h"

using namespace std;

class GameConfig
{
public:
	GameConfig();
	~GameConfig();
	void LoadFromXMLNode(LLXMLNode* xmlNode);
	LLXMLNode* ExportToXMLNode();
	void SetProperty(wstring name, wstring value);

	PropertyGameName gameName;
	PropertyGameWidth width;
	PropertyGameHeight height;
	PropertyGameLeft left;
	PropertyGameTop top;
	PropertyMiddleInScreen middleInScreen;
	PropertyFullScreen fullScreen;
	PropertyCanMultiGame canMultiGame;
	PropertyStartScene startScene;
	PropertyGraphicsApi graphicsApi;
	PropertyOpenNetClient openNetClient;
	PropertyOpenPhysics openPhysics;
	PropertyIcon icon;
	PropertyDefaultCursor defaultCursor;
	PropertyServerIPPort serverIPPort;

	wstring resourcePath = L"Resource";

	/*int width = 800;
	int height = 600;
	wstring gameName = L"Game";
	bool fullScreen = false;
	bool canMultiGame = false;
	wstring startScene = L"";
	wstring graphicsApi = L"Direct2D";
	bool openNetClient = false;
	bool openPhysics = false;
	wstring icon = L"";
	wstring defaultCursor = L"";*/

private:
	unordered_map<wstring, IUIProperty*> propertyMap;
	PropertyFrameNumber propertyFrameNumber;
};

