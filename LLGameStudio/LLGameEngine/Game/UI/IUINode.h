#pragma once
#include <list>
#include <unordered_map>
#include "IUIProperty.h"
#include "../../Common/XML/LLXML.h"
#include "../../Common/Helper/SystemHelper.h"
#include "../../Common/Helper/GameHelper.h"
#include "..\..\Common\Helper\MathHelper.h"
#include "..\..\Common\Window\LLGameWindow.h"

using namespace std;

enum class UIState
{
	Normal = 0,
	Hovor,
	Click
};

class IUINode
{
public:
	IUINode();
	~IUINode();
	wstring GetName();
	float GetActualWidth();
	float GetActualHeight();
	float GetActualLeft();
	float GetActualTop();
	void SetWidth(float width);
	virtual void SetProperty(wstring name,wstring value);
	void ResetTransform();
	void SetHeight(float height);
	void AddNode(IUINode* node);
	IUINode* GetNode(wstring nodeName);
	void RemoveNode(IUINode* node);
	void LoadFromXMLNode(LLXMLNode* xmlNode);
	virtual bool CheckState();
	virtual void Update();
	virtual void Render() = 0;

	HandleUIEvent OnUpdate;
	HandleUIEvent OnMouseClick;
	HandleUIEvent OnMouseEnter;
	HandleUIEvent OnMouseLeave;
	static bool uiLock;
protected:
	IUINode* parentNode = nullptr;
	list<IUINode*> listNode;
	unordered_map<wstring, IUIProperty*> propertyMap;
	float actualWidth = 0;
	float actualHeight = 0;
	Rect actualRect;
	PropertyName propertyName;
	PropertyEnable propertyEnable;
	PropertyWidth propertyWidth;
	PropertyHeight propertyHeight;
	PropertyAnchorEnum propertyAnchorEnum;
	PropertyrRotation propertyRotation;
	PropertyrMargin propertyMargin;
	PropertyrClipByParent propertyClipByParent;

	UIState uiState = UIState::Normal;
};