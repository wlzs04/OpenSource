#include "IUINode.h"

IUINode::IUINode()
{
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
	propertyWidth->value = width;
}

void IUINode::SetHeight(float height)
{
	propertyWidth->value = height;
}

void IUINode::AddNode(IUINode* node)
{
	listNode.push_back(node);
}

void IUINode::RemoveNode(IUINode* node)
{
	listNode.remove(node);
}
