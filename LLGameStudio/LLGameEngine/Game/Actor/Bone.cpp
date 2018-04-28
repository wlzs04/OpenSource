#include "Bone.h"

Bone::Bone()
{
	propertyMap[propertyLength.name] = &propertyLength;
	propertyMap[propertyAngle.name] = &propertyAngle;
}

void Bone::LoadFromXMLNode(LLXMLNode * xmlNode)
{
	for (auto var : xmlNode->GetPropertyMap())
	{
		SetProperty(var.first, var.second->GetValue());
	}
	for (auto var : xmlNode->GetChildNodeList())
	{
		if (var->GetName() == L"Bone")
		{
			Bone* bone = new Bone();
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

void Bone::Update()
{
	throw std::logic_error("The method or operation is not implemented.");
}

void Bone::Render()
{
	throw std::logic_error("The method or operation is not implemented.");
}
