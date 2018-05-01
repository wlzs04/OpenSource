#include "Action.h"

Action::Action()
{
	propertyMap[propertyTotalFrameNumber.name] = &propertyTotalFrameNumber;
	propertyMap[propertyTotalTime.name] = &propertyTotalTime;
	propertyMap[propertyName.name] = &propertyName;
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

Frame * Action::GetFrameByNumber(int frameNumber)
{
	for (auto var : listFrame)
	{
		if (var->GetFrameNumber() == frameNumber)
		{
			return var;
		}
	}
	return nullptr;
}

Frame * Action::GetFrameByTime(float time)
{
	int preFrameNumber = (int)(time / propertyTotalTime.value * propertyTotalFrameNumber.value);
	int nextFrameNumber = preFrameNumber +1;

	Frame* preFrame = GetFrameByNumber(preFrameNumber);
	if (preFrame == nullptr)
	{
		preFrame = GetPreFrameByNumber(preFrameNumber);
	}
	if (preFrameNumber != nextFrameNumber)
	{
		Frame* nextFrame = GetFrameByNumber(nextFrameNumber);
		if (nextFrame == nullptr)
		{
			nextFrame = GetNextFrameByNumber(nextFrameNumber);
		}
		Frame* frame = new Frame();
		if (preFrame != nullptr && nextFrame != nullptr)
		{
			double preFrameTime = propertyTotalTime.value * preFrame->GetFrameNumber() / propertyTotalFrameNumber.value;
			double nextFrameTime = propertyTotalTime.value * nextFrame->GetFrameNumber() / propertyTotalFrameNumber.value;
			double preRate = 1 - (time - preFrameTime) / (nextFrameTime - preFrameTime);
			double nextRate = 1 - (nextFrameTime - time) / (nextFrameTime - preFrameTime);

			for (int i = 0; i < preFrame->listBone.size(); i++)
			{
				Bone* bone = new Bone();
				bone->SetProperty(L"name", preFrame->listBone[i]->GetName());
				bone->SetProperty(L"angle",to_wstring( (preRate * preFrame->listBone[i]->GetAngle() + nextRate * nextFrame->listBone[i]->GetAngle())));
				frame->listBone.push_back(bone);
			}
		}
		return frame;
	}
	else
	{
		return preFrame;
	}
}

Frame * Action::GetPreFrameByNumber(int frameNumber)
{
	int index = -1;
	int i = 0;
	for(auto item : listFrame)
	{
		if (item->GetFrameNumber() < frameNumber)
		{
			index = i;
		}
		else
		{
			break;
		}
		i++;
	}
	//index = index > listFrame.size() - 1?listFrame.size() - 1 : index;
	
	if (index>-1)
	{
		return listFrame[index];
	}
	return nullptr;
}

Frame * Action::GetNextFrameByNumber(int frameNumber)
{
	int index = -1;
	int i = 0;
	for (auto item : listFrame)
	{
		if (item->GetFrameNumber() > frameNumber)
		{
			index = i;
			break;
		}
		i++;
	}

	if (index > -1)
	{
		return listFrame[index];
	}
	return nullptr;
}

wstring Action::GetName()
{
	return propertyName.value;
}

float Action::GetTotalTime()
{
	return propertyTotalTime.value;
}
