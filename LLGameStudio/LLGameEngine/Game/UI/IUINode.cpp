#include "IUINode.h"
#include "LLGameBack.h"
#include "LLGameLayout.h"
#include "LLGameImage.h"

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

void IUINode::SetWidth(float width)
{
	propertyWidth.value = width;
	actualWidth = width;
}

void IUINode::SetProperty(wstring name, wstring value)
{
	propertyMap[name]->SetValue(value);
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
		if (propertyMap.count(var.first) > 0)
		{
			SetProperty(var.first, var.second->GetValue());
		}
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
		if (uiNode != nullptr)
		{
			uiNode->LoadFromXMLNode(var);
			AddNode(uiNode);
		}
	}
}
