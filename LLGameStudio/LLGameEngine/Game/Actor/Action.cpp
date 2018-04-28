#include "Action.h"

Action::Action()
{
	propertyMap[propertyTotalFrameNumber.name] = &propertyTotalFrameNumber;
	propertyMap[propertyTotalTime.name] = &propertyTotalTime;
}

void Action::LoadFromXMLNode(LLXMLNode * xmlNode)
{
	for (auto var : xmlNode->GetPropertyMap())
	{
		SetProperty(var.first, var.second->GetValue());
	}
	for (auto var : xmlNode->GetChildNodeList())
	{
		if (var->GetName() == L"Frame")
		{
			Frame* frame = new Frame();
			frame->LoadFromXMLNode(var);
			listFrame.push_back(frame);
		}
	}
}

void Action::SetProperty(wstring name, wstring value)
{
	if (propertyMap.count(name) != 0)
	{
		propertyMap[name]->SetValue(value);
	}
}
