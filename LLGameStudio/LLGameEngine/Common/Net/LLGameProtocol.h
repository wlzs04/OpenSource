#pragma once
#include <string>
#include <unordered_map>

using namespace std;

class LLGameProtocol
{
public:
	const wstring GetContent()
	{
		return content;
	}

	int GetLength()
	{
		return content.length();
	}

	virtual void SetContent(wstring content)
	{
		this->content = content;
	}

	virtual void Process(void* ptr) {};

protected:
	wstring content;
};