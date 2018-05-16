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
	PropertyEncryptKey encryptKey;
	PropertyDefaultScript defaultScript;

	wstring resourcePath = L"Resource";

private:
	unordered_map<wstring, IUIProperty*> propertyMap;
	PropertyFrameNumber propertyFrameNumber;
};

