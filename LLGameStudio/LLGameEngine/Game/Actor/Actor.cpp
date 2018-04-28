#include "Actor.h"

void Actor::LoadActorFromFile(wstring filePath)
{
	if (rootBone != nullptr)
	{
		delete rootBone;
		rootBone = nullptr;
	}
	listAction.clear();
	LLXMLDocument doc;
	doc.LoadXMLFromFile(SystemHelper::GetResourceRootPath() + L"\\" + filePath);
	LoadFromXMLNode(doc.GetRootNode());
}

void Actor::LoadFromXMLNode(LLXMLNode * xmlNode)
{
	for (auto var : xmlNode->GetChildNodeList())
	{
		if (var->GetName() == L"Bone")
		{
			rootBone = new Bone();
			rootBone->LoadFromXMLNode(var);
		}
		if (var->GetName() == L"Actions")
		{
			LoadActionFromXML(var);
		}
	}
}

void Actor::LoadActionFromXML(LLXMLNode* xmlNode)
{
	for (auto var : xmlNode->GetChildNodeList())
	{
		if (var->GetName() == L"Action")
		{
			Action* action = new Action();
			action->LoadFromXMLNode(var);
			listAction.push_back(action);
		}
	}
}
