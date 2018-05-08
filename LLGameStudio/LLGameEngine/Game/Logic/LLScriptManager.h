#pragma once
#include "LLScript.h"
#include <set>

class LLScriptManager
{
private:
	LLScriptManager();
public:
	static LLScriptManager* GetSingleInstance();
	bool LoadScriptFromFile(wstring filePath);
	void UnLoadScriptFromFile(wstring filePath);
	Parameter RunFunction(wstring scriptName, wstring functionName);
	bool IsLegalType(wstring typeName);
	void AddLegalType(Class* classPtr);
	Class* GetTypeInstanceByName(wstring typeName);
private:
	unordered_map<wstring, LLScript*> llScriptMap;
	unordered_map<wstring, Class*> legalTypeMap;
};
