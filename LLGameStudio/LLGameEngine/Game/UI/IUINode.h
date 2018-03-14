#pragma once
#include <list>
#include <unordered_map>
#include "IUIProperty.h"
#include "../../Common/Graphics/GraphicsApi.h"

using namespace std;

class IUINode
{
public:
	IUINode();
	~IUINode();
	float GetActualWidth();
	float GetActualHeight();
	void SetWidth(float width);
	void SetProperty(wstring name,wstring value);
	void SetHeight(float height);
	void AddNode(IUINode* node);
	void RemoveNode(IUINode* node);
	virtual void Render()=0;
protected:
	IUINode* parentNode = nullptr;
	list<IUINode*> listNode;
	unordered_map<wstring, IUIProperty*> propertyMap;
	float actualWidth = 0;
	float actualHeight = 0;
	PropertyName propertyName;
	PropertyWidth propertyWidth;
	PropertyHeight propertyHeight;
	PropertyAnchorEnum propertyAnchorEnum;
	PropertyrRotation propertyRotation;
	PropertyrMargin propertyMargin;
	PropertyrClipByParent propertyClipByParent;
};