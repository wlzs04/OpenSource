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
	void SetWidth(float width);
	void SetHeight(float Height);
private:
	list<IUINode*> listNode;
	unordered_map<wstring, IUIProperty*> propertyMap;
	float actualWidth = 0;
	float actualHeight = 0;
};