#pragma once
#include <unordered_map>
#include "..\UI\IUIProperty.h"
#include "..\..\Common\XML\LLXML.h"
#include "Bone.h"

class Frame
{
public:
	Frame();
	void LoadFromXMLNode(LLXMLNode* xmlNode);
	void SetProperty(wstring name, wstring value);
	vector<Bone*> listBone;

private:
	unordered_map<wstring, IUIProperty*> propertyMap;
	PropertyFrameNumber propertyFrameNumber;
};