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
			wstring image = var->GetProperty(L"image")->GetValue();
			uiNode->SetProperty(L"image", image);
			GraphicsApi::GetGraphicsApi()->AddImage(image);
			//uiNode->SetProperty(L"width", var->GetProperty(L"width")->GetValue());
			//uiNode->SetProperty(L"height", var->GetProperty(L"height")->GetValue());
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
