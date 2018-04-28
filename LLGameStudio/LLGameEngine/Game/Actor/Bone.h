#pragma once
#include "..\UI\IUINode.h"

class Bone :public IUINode
{
public:
	Bone();
	virtual void LoadFromXMLNode(LLXMLNode* xmlNode) override;
	virtual void Update() override;
	virtual void Render() override;
	void AddBone(Bone* bone);

	Bone* parentBone = nullptr;
	vector<Bone*> listBone;
private:

	PropertyLength propertyLength;
	PropertyAngle propertyAngle;
};