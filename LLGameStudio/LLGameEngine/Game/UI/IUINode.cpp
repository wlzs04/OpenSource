#include "IUINode.h"
#include "LLGameBack.h"
#include "LLGameLayout.h"
#include "LLGameImage.h"
#include "LLGameButton.h"
#include "LLGameText.h"

IUINode::IUINode()
{
	propertyMap[propertyName.name] = &propertyName;
	propertyMap[propertyWidth.name] = &propertyWidth;
	propertyMap[propertyHeight.name] = &propertyHeight;
	propertyMap[propertyAnchorEnum.name] = &propertyAnchorEnum;
	propertyMap[propertyRotation.name] = &propertyRotation;
	propertyMap[propertyMargin.name] = &propertyMargin;
	propertyMap[propertyClipByParent.name] = &propertyClipByParent;
}

IUINode::~IUINode()
{
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
	return actualLeft;
}

float IUINode::GetActualTop()
{
	return actualTop;
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

void IUINode::ResetTransform()
{
	float parentWidth = parentNode ? parentNode->GetActualWidth() : GameHelper::GetGameWidth();
	float parentHeight = parentNode ? parentNode->GetActualHeight() : GameHelper::GetGameHeight();
	float parentLeft = parentNode ? parentNode->GetActualLeft() : 0;
	float parentTop = parentNode ? parentNode->GetActualTop() : 0;

	actualWidth = MathHelper::IsRange1To0(propertyWidth.value) ? parentWidth * propertyWidth.value : propertyWidth.value;
	actualHeight = MathHelper::IsRange1To0(propertyHeight.value) ? parentHeight * propertyHeight.value : propertyHeight.value;
	
	actualLeft = MathHelper::IsRange1To0(propertyMargin.value.left) ? parentWidth * propertyMargin.value.left : propertyMargin.value.left;
	actualTop = MathHelper::IsRange1To0(propertyMargin.value.top) ? parentHeight * propertyMargin.value.top : propertyMargin.value.top;
	actualRight = MathHelper::IsRange1To0(propertyMargin.value.right) ? parentWidth * propertyMargin.value.right : propertyMargin.value.right;
	actualBottom = MathHelper::IsRange1To0(propertyMargin.value.bottom) ? parentHeight * propertyMargin.value.bottom : propertyMargin.value.bottom;

	if ((propertyAnchorEnum.value & AnchorEnum::Left) != 0)
	{
		actualLeft = actualLeft;
	}
	else if ((propertyAnchorEnum.value & AnchorEnum::Right) != 0)
	{
		actualLeft = parentWidth - actualRight - actualWidth;
	}
	else
	{
		actualLeft = (parentWidth - actualWidth) / 2;
	}
	if ((propertyAnchorEnum.value & AnchorEnum::Top) != 0)
	{
		actualTop = actualTop;
	}
	else if ((propertyAnchorEnum.value & AnchorEnum::Bottom) != 0)
	{
		actualTop = parentHeight - actualBottom - actualHeight;
	}
	else
	{
		actualTop = (parentHeight - actualHeight) / 2;
	}

	actualLeft += parentLeft;
	actualTop += parentTop;

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
