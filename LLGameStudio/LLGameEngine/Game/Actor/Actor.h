#pragma once
#include "Bone.h"
#include "Action.h"

class Actor
{
public:
	void LoadActorFromFile(wstring filePath);
	void LoadFromXMLNode(LLXMLNode* xmlNode);
	void LoadIKsFromXML(LLXMLNode* xmlNode);
	void LoadActionFromXML(LLXMLNode* xmlNode);

	void Update();
	void Render();

	void SetPosition(float x, float y);
	Vector2 GetPosition();

	void SetCurrentAction(wstring actionName);
	void Start();
	void Stop();
	Bone* GetBoneByName(Bone* bone, wstring actionName);
	void SetBoneTrandformByIK(Bone* moveBone, Vector2 newPosition);

	Bone* rootBone;
	unordered_map<Bone*, Bone*> ikMap;
	vector<Action*> listAction;
	Action* currentAction = nullptr;
private:
	Vector2 position;
	float actionPlayTime = 0;
	bool playAction = false;

	int ikCyclicNumber = 3;
};