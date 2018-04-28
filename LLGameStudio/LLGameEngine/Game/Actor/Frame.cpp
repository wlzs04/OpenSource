#include "Frame.h"

Frame::Frame()
{
	propertyMap[propertyFrameNumber.name] = &propertyFrameNumber;
}

void Frame::LoadFromXMLNode(LLXMLNode * xmlNode)
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

void Frame::SetProperty(wstring name, wstring value)
{
	if (propertyMap.count(name) != 0)
	{
		propertyMap[name]->SetValue(value);
	}
}
