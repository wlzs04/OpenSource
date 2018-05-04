#include "LLGameProtocol.h"
#include <sstream>
#include "..\Helper\WStringHelper.h"
#include <vector>

LLGameProtocol::LLGameProtocol(wstring name)
{
	this->name = name;
}

wstring LLGameProtocol::GetName()
{
	return name;
}

void LLGameProtocol::LoadContentFromWString(wstring content)
{
	vector<wstring> contentVector;
	WStringHelper::Split(content, L' ', contentVector);
	if (contentVector.size() % 2 != 1)
	{
		return;
	}
	for (int i=1;i<contentVector.size();i+=2)
	{
		contentMap[contentVector[i]] = contentVector[i + 1];
	}
}

const std::wstring LLGameProtocol::ExportContentToWString()
{
	wstringstream wss(L"");
	wss << name << L" ";
	for (auto var : contentMap)
	{
		wss << var.first << L" " << var.second<<L" ";
	}
	return wss.str();
}

void LLGameClientProtocol::AddContent(wstring key, wstring value)
{
	contentMap[key] = value;
}

wstring LLGameServerProtocol::GetContent(wstring key)
{
	return contentMap[key];
}
