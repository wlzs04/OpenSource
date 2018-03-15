#include "LLGameLayout.h"

LLGameLayout::LLGameLayout()
{
	propertyMap[propertyFilePath.name] = &propertyFilePath;
}

void LLGameLayout::Render()
{
	uiGrid->Render();
}

void LLGameLayout::SetProperty(wstring name, wstring value)
{
	if (name == propertyFilePath.name)
	{
		LoadLayoutFromFile(value);
	}
	else
	{
		IUINode::SetProperty(name, value);
	}
}

void LLGameLayout::LoadLayoutFromFile(wstring filePath)
{
	if (uiGrid != nullptr)
	{
		delete uiGrid;
		uiGrid = nullptr;
	}
	propertyFilePath.SetValue(filePath);
	LLXMLDocument doc;
	doc.LoadXMLFromFile(SystemHelper::GetResourceRootPath()+L"\\"+ filePath);
	LLXMLNode* node = doc.GetRootNode();
	uiGrid = new LLGameGrid();
	uiGrid->LoadFromXMLNode(node);
}
