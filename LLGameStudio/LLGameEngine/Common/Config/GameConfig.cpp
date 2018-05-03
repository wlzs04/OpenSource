#include "GameConfig.h"

GameConfig::GameConfig()
{
	propertyMap[gameName.name] = &gameName;
	propertyMap[width.name] = &width;
	propertyMap[height.name] = &height;
	propertyMap[fullScreen.name] = &fullScreen;
	propertyMap[canMultiGame.name] = &canMultiGame;
	propertyMap[startScene.name] = &startScene;
	propertyMap[graphicsApi.name] = &graphicsApi;
	propertyMap[openNetClient.name] = &openNetClient;
	propertyMap[openPhysics.name] = &openPhysics;
	propertyMap[icon.name] = &icon;
	propertyMap[defaultCursor.name] = &defaultCursor;
}

GameConfig::~GameConfig()
{
}

void GameConfig::LoadFromXMLNode(LLXMLNode* xmlNode)
{
	for (auto var : xmlNode->GetPropertyMap())
	{
		SetProperty(var.first, var.second->GetValue());
	}
}

LLXMLNode* GameConfig::ExportToXMLNode()
{
	LLXMLNode* node = new LLXMLNode(L"Game");
	for (auto var : propertyMap)
	{
		node->AddProperty(new LLXMLProperty(var.first, var.second->GetValue()));
	}
	return node;
}

void GameConfig::SetProperty(wstring name, wstring value)
{
	if (propertyMap.count(name) != 0)
	{
		propertyMap[name]->SetValue(value);
	}
}
