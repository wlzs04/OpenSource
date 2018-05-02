#pragma once
#include "..\UI\IUINode.h"

class Actor;

class Bone :public IUINode
{
public:
	Bone();
	~Bone();
	virtual void LoadFromXMLNode(LLXMLNode* xmlNode) override;
	virtual void Update() override;
	virtual void Render() override;
	void AddBone(Bone* bone);
	void SetPosition(float x, float y);
	Vector2 GetPosition();
	Vector2 GetBoneEndPosition();
	void SetAngle(float angle);
	float GetAngle(); 
	float GetRealAngle();
	Bone* parentBone = nullptr;
	Actor* actor = nullptr;
	vector<Bone*> listBone;
private:
	PropertyLength propertyLength;
	PropertyAngle propertyAngle;

	Vector2 position;
	float realAngle = 0;
};