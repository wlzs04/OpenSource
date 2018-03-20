#include "LLGameLayout.h"

LLGameLayout::LLGameLayout()
{
	propertyMap[propertyFilePath.name] = &propertyFilePath;
	propertyMap[propertyModal.name] = &propertyModal;
}

void LLGameLayout::Render()
{
	if (propertyModal.value)
	{
		GraphicsApi::GetGraphicsApi()->DrawRect(true, 0, 0, GameHelper::width, GameHelper::height);
	}
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

bool LLGameLayout::CheckState()
{
	if (!propertyEnable.value || propertyCheckStateMethod.value == CheckStateMethod::AllowMouseThrough)
	{
		return false;
	}
	for (auto rNode = listNode.rbegin(); rNode != listNode.rend(); rNode++)
	{
		if ((*rNode)->CheckState())
		{
			break;
		}
	}
	bool inArea = false;
	switch (propertyCheckStateMethod.value.value)
	{
	case CheckStateMethod::Rect:
		inArea = GameHelper::IsPointInRect(actualRect);
		break;
	case CheckStateMethod::Alpha:
		inArea = false;
		break;
	}
	if (inArea)
	{
		if (GameHelper::mouseLeftButtonPassed && !uiLock)
		{
			if (uiState != UIState::Click)
			{
				uiState = UIState::Click;
				if (OnMouseClick)
				{
					OnMouseClick(this, 0);
				}
				uiLock = true;
			}
		}
		else
		{
			if (uiState != UIState::Hovor)
			{
				uiState = UIState::Hovor;
				if (OnMouseEnter)
				{
					OnMouseEnter(this, 0);
				}
			}
		}
		return true;
	}
	else
	{
		if (uiState == UIState::Hovor)
		{
			uiState = UIState::Normal;
			if (OnMouseLeave)
			{
				OnMouseLeave(this, 0);
			}
		}
		return false;
	}
	return propertyModal.value;
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
	AddNode(uiGrid);
	uiGrid->LoadFromXMLNode(node);
}
