#pragma once
#include <string>
#include <unordered_map>

using namespace std;

class LLGame;

class LLGameProtocol
{
public:
	LLGameProtocol(wstring name); 
	wstring GetName();
	void LoadContentFromWString(wstring content);
	const wstring ExportContentToWString();

protected:
	unordered_map<wstring, wstring> contentMap;
	wstring name;
};

class LLGameClientProtocol : public LLGameProtocol
{
public:
	LLGameClientProtocol(wstring name) :LLGameProtocol(name) {}
	void AddContent(wstring key, wstring value);
};

class LLGameServerProtocol : public LLGameProtocol
{
public:
	LLGameServerProtocol(wstring name) :LLGameProtocol(name) {}
	virtual LLGameServerProtocol* GetInstance() = 0;
	wstring GetContent(wstring key);
	virtual void Process(LLGame* ptr) = 0;
};