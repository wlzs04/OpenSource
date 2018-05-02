#include "Bone.h"
#include "Actor.h"

Bone::Bone()
{
	propertyMap[propertyLength.name] = &propertyLength;
	propertyMap[propertyAngle.name] = &propertyAngle;
}

Bone::~Bone()
{
	for (auto var : listBone)
	{
		delete var;
	}
}

void Bone::LoadFromXMLNode(LLXMLNode * xmlNode)
{
	for (auto var : xmlNode->GetPropertyMap())
	{
		SetProperty(var.first, var.second->GetValue());
	}
	realAngle = propertyAngle.value;
	for (auto var : xmlNode->GetChildNodeList())
	{
		if (var->GetName() == L"Bone")
		{
			Bone* bone = new Bone();
			bone->parentBone = this;
			bone->LoadFromXMLNode(var);
			listBone.push_back(bone);
		}
	}
}

void Bone::AddBone(Bone * bone)
{
	listBone.push_back(bone);
	bone->parentBone = this;
}

void Bone::SetPosition(float x, float y)
{
	position.x = x;
	position.y = y;
}

Vector2 Bone::GetBoneEndPosition()
{
	GetRealAngle();
	double x = propertyLength.value * sin(realAngle);
	double y = propertyLength.value * cos(realAngle);
	return Vector2(position.x - x, position.y + y);
}

void Bone::SetAngle(float angle)
{
	propertyAngle.value = angle;
}

Vector2 Bone::GetPosition()
{
	if (parentBone)
	{
		position = parentBone->GetBoneEndPosition();
	}
	return position;
}

float Bone::GetAngle()
{
	return propertyAngle.value;
}

float Bone::GetRealAngle()
{
	if (parentBone)
	{
		realAngle = propertyAngle.value + parentBone->GetRealAngle();
	}
	else
	{
		realAngle = propertyAngle.value;
	}
	return realAngle;
}

void Bone::Update()
{
	if (parentBone)
	{
		Vector2 parentEndPosition = parentBone->GetBoneEndPosition();
		SetPosition(parentEndPosition.x, parentEndPosition.y);
		realAngle = propertyAngle.value + parentBone->GetRealAngle();
	}
	else
	{
		Vector2 parentPosition = actor->GetPosition();
		SetPosition(parentPosition.x, parentPosition.y);
	}
	for (auto var : listBone)
	{
		var->Update();
	}
}

void Bone::Render()
{
	GraphicsApi::GetGraphicsApi()->SetTransform(realAngle * 180 / MathHelper::PI, position.x, position.y);
	GraphicsApi::GetGraphicsApi()->DrawRect(false, position.x - 5, position.y, 10, propertyLength.value);
	GraphicsApi::GetGraphicsApi()->DrawEllipse(false, position.x, position.y, 10, 10);
	GraphicsApi::GetGraphicsApi()->ResetTransform();

	for (auto var : listBone)
	{
		var->Render();
	}
}
