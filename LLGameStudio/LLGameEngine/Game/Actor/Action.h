#pragma once
#include "..\..\Common\XML\LLXML.h"
#include "Frame.h"

class Action
{
public:
	Action();
	void LoadFromXMLNode(LLXMLNode* xmlNode);
	void SetProperty(wstring name, wstring value);

	Frame* GetFrameByNumber(int frameNumber);
	Frame* GetFrameByTime(float time);
	Frame* GetPreFrameByNumber(int frameNumber);
	Frame* GetNextFrameByNumber(int frameNumber);

	wstring GetName();
	float GetTotalTime();
	
	vector<Frame*> listFrame;
private:
	unordered_map<wstring, IUIProperty*> propertyMap;
	PropertyTotalFrameNumber propertyTotalFrameNumber;
	PropertyTotalTime propertyTotalTime;
	PropertyName propertyName;
};