#include "LLGameScene.h"

LLGameScene::LLGameScene()
{
	propertyMap[propertyFilePath.name] = &propertyFilePath;
}

void LLGameScene::LoadSceneFromFile(wstring filePath)
{
	propertyFilePath.SetValue(filePath);
	LLXMLDocument doc;
	doc.LoadXMLFromFile(filePath);
	LLXMLNode* node = doc.GetRootNode();
	for (auto var : node->GetPropertyMap())
	{
		if (propertyMap.count(var.first) > 0)
		{
			propertyMap[var.first]->SetValue(var.second->GetValue());
		}
	}
	for (auto var : node->GetChildNodeList())
	{
		IUINode* uiNode = nullptr;
		if (var->GetName() == L"LLGameBack")
		{
			uiNode = new LLGameBack();
			uiNode->LoadFromXMLNode(var);
		}
		else if (var->GetName() == L"LLGameCanvas")
		{
			uiNode = new LLGameCanvas();
			uiNode->LoadFromXMLNode(var);
		}
		else if (var->GetName() == L"LLGameLayout")
		{
			uiNode = new LLGameLayout();
			uiNode->LoadFromXMLNode(var);
		}
		if (uiNode != nullptr)
		{
			AddNode(uiNode);
		}
	}
}

void LLGameScene::Render()
{
	
	actualWidth = propertyWidth.value;
	actualHeight = propertyHeight.value;
	
	GraphicsApi::GetGraphicsApi()->DrawRect(0,0, actualWidth, actualHeight);
	for (auto var : listNode)
	{
		var->Render();
	}
}
