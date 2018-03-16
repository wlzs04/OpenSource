#pragma once
#include <list>
#include <unordered_map>
#include "IUIProperty.h"
#include "../../Common/XML/LLXML.h"
#include "../../Common/Helper/SystemHelper.h"
#include "../../Common/Helper/GameHelper.h"
#include "..\..\Common\Helper\MathHelper.h"

using namespace std;

class IUINode
{
public:
	IUINode();
	~IUINode();
	float GetActualWidth();
	float GetActualHeight();
	float GetActualLeft();
	float GetActualTop();
	void SetWidth(float width);
	virtual void SetProperty(wstring name,wstring value);
	void ResetTransform();
	void SetHeight(float height);
	void AddNode(IUINode* node);
	void RemoveNode(IUINode* node);
	void LoadFromXMLNode(LLXMLNode* xmlNode);
	virtual void Update() {};
	virtual void Render()=0;
protected:
	IUINode* parentNode = nullptr;
	list<IUINode*> listNode;
	unordered_map<wstring, IUIProperty*> propertyMap;
	float actualWidth = 0;
	float actualHeight = 0;
	float actualLeft = 0;
	float actualTop = 0;
	float actualRight = 0;
	float actualBottom = 0;
	PropertyName propertyName;
	PropertyWidth propertyWidth;
	PropertyHeight propertyHeight;
	PropertyAnchorEnum propertyAnchorEnum;
	PropertyrRotation propertyRotation;
	PropertyrMargin propertyMargin;
	PropertyrClipByParent propertyClipByParent;
};