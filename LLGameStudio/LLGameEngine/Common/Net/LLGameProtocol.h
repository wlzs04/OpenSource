#pragma once
#include <string>

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

	void SetContent(wstring content)
	{
		this->content = content;
	}

	virtual void Process(void* ptr) {};
private:
	wstring content;
};