#include "IUINode.h"
#include "LLGameBack.h"
#include "LLGameLayout.h"
#include "LLGameImage.h"
#include "LLGameButton.h"
#include "LLGameText.h"

bool IUINode::uiLock = false;

IUINode::IUINode()
{
	propertyMap[propertyName.name] = &propertyName;
	propertyMap[propertyEnable.name] = &propertyEnable;
	propertyMap[propertyWidth.name] = &propertyWidth;
	propertyMap[propertyHeight.name] = &propertyHeight;
	propertyMap[propertyAnchorEnum.name] = &propertyAnchorEnum;
	propertyMap[propertyRotation.name] = &propertyRotation;
	propertyMap[propertyMargin.name] = &propertyMargin;
	propertyMap[propertyClipByParent.name] = &propertyClipByParent;
	propertyMap[propertyCheckStateMethod.name] = &propertyCheckStateMethod;
}

IUINode::~IUINode()
{
}

wstring IUINode::GetName()
{
	return propertyName.value;
}

float IUINode::GetActualWidth()
{
	return actualWidth;
}

float IUINode::GetActualHeight()
{
	return actualHeight;
}

float IUINode::GetActualLeft()
{
	return actualRect.left;
}

float IUINode::GetActualTop()
{
	return actualRect.top;
}

void IUINode::SetWidth(float width)
{
	propertyWidth.value = width;
	actualWidth = width;
}

void IUINode::SetProperty(wstring name, wstring value)
{
	if (propertyMap.count(name) != 0)
	{
		propertyMap[name]->SetValue(value);
	}
}

void IUINode::SetEnable(bool enable)
{
	propertyEnable.value = enable;
}

void IUINode::ResetTransform()
{
	float parentWidth = parentNode ? parentNode->GetActualWidth() : GameHelper::width;
	float parentHeight = parentNode ? parentNode->GetActualHeight() : GameHelper::height;
	float parentLeft = parentNode ? parentNode->GetActualLeft() : 0;
	float parentTop = parentNode ? parentNode->GetActualTop() : 0;

	actualWidth = MathHelper::IsRange1To0(propertyWidth.value) ? parentWidth * propertyWidth.value : propertyWidth.value;
	actualHeight = MathHelper::IsRange1To0(propertyHeight.value) ? parentHeight * propertyHeight.value : propertyHeight.value;
	
	actualRect.left = MathHelper::IsRange1To0(propertyMargin.value.left) ? parentWidth * propertyMargin.value.left : propertyMargin.value.left;
	actualRect.top = MathHelper::IsRange1To0(propertyMargin.value.top) ? parentHeight * propertyMargin.value.top : propertyMargin.value.top;
	actualRect.right = MathHelper::IsRange1To0(propertyMargin.value.right) ? parentWidth * propertyMargin.value.right : propertyMargin.value.right;
	actualRect.bottom = MathHelper::IsRange1To0(propertyMargin.value.bottom) ? parentHeight * propertyMargin.value.bottom : propertyMargin.value.bottom;

	if ((propertyAnchorEnum.value & AnchorEnum::Left) != 0)
	{
		actualRect.left = actualRect.left;
	}
	else if ((propertyAnchorEnum.value & AnchorEnum::Right) != 0)
	{
		actualRect.left = parentWidth - actualRect.right - actualWidth;
	}
	else
	{
		actualRect.left = (parentWidth - actualWidth) / 2;
	}
	if ((propertyAnchorEnum.value & AnchorEnum::Top) != 0)
	{
		actualRect.top = actualRect.top;
	}
	else if ((propertyAnchorEnum.value & AnchorEnum::Bottom) != 0)
	{
		actualRect.top = parentHeight - actualRect.bottom - actualHeight;
	}
	else
	{
		actualRect.top = (parentHeight - actualHeight) / 2;
	}

	actualRect.left += parentLeft;
	actualRect.top += parentTop;
	actualRect.right = actualRect.left + actualWidth;
	actualRect.bottom = actualRect.top + actualHeight;
	for (auto var : listNode)
	{
		var->ResetTransform();
	}
}

void IUINode::SetHeight(float height)
{
	propertyHeight.value = height;
	actualHeight = height;
}

void IUINode::AddNode(IUINode* node)
{
	listNode.push_back(node);
	node->parentNode = this;
}

IUINode* IUINode::GetNode(wstring nodeName)
{
	wstring rootNodeName;
	wstring realNodeName;
	int pos = nodeName.find(L"\\");
	if (pos != -1)
	{
		rootNodeName = nodeName.substr(0, pos);
		realNodeName = nodeName.substr(pos+1);
		for (auto var : listNode)
		{
			if (var->GetName() == rootNodeName)
			{
				return var->GetNode(realNodeName);
			}
		}
	}
	else
	{
		for (auto var : listNode)
		{
			if (var->GetName() == nodeName)
			{
				return var;
			}
		}
	}
	
	return nullptr;
}

void IUINode::RemoveNode(IUINode* node)
{
	listNode.remove(node);
	node->parentNode = nullptr;
}

void IUINode::LoadFromXMLNode(LLXMLNode * xmlNode)
{
	for (auto var : xmlNode->GetPropertyMap())
	{
		SetProperty(var.first, var.second->GetValue());
	}
	for (auto var : xmlNode->GetChildNodeList())
	{
		IUINode* uiNode = nullptr;
		if (var->GetName() == L"LLGameLayout")
		{
			uiNode = new LLGameLayout();
		}
		else if (var->GetName() == L"LLGameGrid")
		{
			uiNode = new LLGameGrid();
		}
		else if (var->GetName() == L"LLGameImage")
		{
			uiNode = new LLGameImage();
		}
		else if (var->GetName() == L"LLGameButton")
		{
			uiNode = new LLGameButton();
		}
		else if (var->GetName() == L"LLGameText")
		{
			uiNode = new LLGameText();
		}
		if (uiNode != nullptr)
		{
			AddNode(uiNode);
			uiNode->LoadFromXMLNode(var);
		}
	}
}

bool IUINode::CheckState()
{
	if (!propertyEnable.value|| propertyCheckStateMethod.value == CheckStateMethod::AllowMouseThrough)
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
	return false;
}

void IUINode::Update()
{
	if (OnUpdate != nullptr)
	{
		OnUpdate(this,0);
	}
	for (auto var : listNode)
	{
		var->Update();
	}
}
