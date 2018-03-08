#pragma once
#include <list>
#include <unordered_map>
#include "IUIProperty.h"

using namespace std;

class IUINode
{
public:
	IUINode();
	~IUINode();
	float GetActualWidth();
	float GetActualHeight();
	void SetWidth(float width);
	void SetHeight(float height);
	void AddNode(IUINode* node);
	void RemoveNode(IUINode* node);
private:
	list<IUINode*> listNode;//节点需要遍历渲染，使用set可能会慢，但这不是重点。
	unordered_map<wstring, IUIProperty*> propertyMap;
	float actualWidth = 0;
	float actualHeight = 0;
	PropertyName* propertyName;
	PropertyWidth* propertyWidth;
	PropertyHeight* propertyHeight;
	PropertyAnchorEnum* propertyAnchorEnum;
	PropertyrRotation* propertyRotation;
	PropertyrMargin* propertyMargin;
	PropertyrClipByParent* propertyClipByParent;
};