#pragma once
#include "..\..\Common\XML\LLXML.h"
#include "Frame.h"

class Action
{
public:
	Action();
	void LoadFromXMLNode(LLXMLNode* xmlNode);
	void SetProperty(wstring name, wstring value);
	vector<Frame*> listFrame;

private:
	unordered_map<wstring, IUIProperty*> propertyMap;
	PropertyTotalFrameNumber propertyTotalFrameNumber;
	PropertyTotalTime propertyTotalTime;
};