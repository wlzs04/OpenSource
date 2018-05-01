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
			rootBone->actor = this;
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

void Actor::Update()
{
	if (playAction)
	{
		float time = GameHelper::thisTickTime;
		actionPlayTime += time;
		if (actionPlayTime > currentAction->GetTotalTime())
		{
			actionPlayTime = 0;
		}
		Frame* frame = currentAction->GetFrameByTime(actionPlayTime);
		for (auto var : frame->listBone)
		{
			Bone* bone = GetBoneByName(rootBone, var->GetName());
			if (bone != nullptr)
			{
				bone->SetAngle(var->GetAngle());
			}
		}
	}
	
	rootBone->Update();
}

void Actor::Render()
{
	rootBone->Render();
}

void Actor::SetPosition(float x, float y)
{
	position.x = x;
	position.y = y;
}

Vector2 Actor::GetPosition()
{
	return position;
}

void Actor::SetCurrentAction(wstring actionName)
{
	for (auto var : listAction)
	{
		if (var->GetName() == actionName)
		{
			currentAction = var;
			actionPlayTime = 0;
			break;
		}
	}
}

void Actor::Start()
{
	playAction = true;
}

void Actor::Stop()
{
	playAction = false;
}

Bone* Actor::GetBoneByName(Bone* bone, wstring actionName)
{
	if (bone->GetName() == actionName)
	{
		return bone;
	}
	for(auto item : bone->listBone)
	{

		Bone* boneChild = GetBoneByName(item, actionName);
		if (boneChild != nullptr)
		{
			return boneChild;
		}
	}
	return nullptr;
}
