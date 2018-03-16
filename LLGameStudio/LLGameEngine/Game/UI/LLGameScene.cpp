#include "LLGameScene.h"

LLGameScene::LLGameScene()
{
	propertyMap[propertyFilePath.name] = &propertyFilePath;
}

void LLGameScene::LoadSceneFromFile(wstring filePath)
{
	propertyFilePath.SetValue(filePath);
	LLXMLDocument doc;
	if (doc.LoadXMLFromFile(filePath))
	{
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
			}
			else if (var->GetName() == L"LLGameCanvas")
			{
				uiNode = new LLGameCanvas();
			}
			else if (var->GetName() == L"LLGameLayout")
			{
				uiNode = new LLGameLayout();
			}
			if (uiNode != nullptr)
			{
				AddNode(uiNode);
				uiNode->LoadFromXMLNode(var);
			}
		}
		ResetTransform();
	}
}

void LLGameScene::Render()
{
	for (auto var : listNode)
	{
		var->Render();
	}
}
