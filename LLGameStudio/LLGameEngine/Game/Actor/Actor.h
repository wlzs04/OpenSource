#pragma once
#include "Bone.h"
#include "Action.h"

class Actor
{
public:
	void LoadActorFromFile(wstring filePath);
	void LoadFromXMLNode(LLXMLNode* xmlNode);
	void LoadActionFromXML(LLXMLNode* xmlNode);

	Bone* rootBone;
	vector<Action*> listAction;
};