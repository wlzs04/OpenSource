#pragma once
#include "LLScript.h"
#include <set>

class LLScriptManager
{
private:
	LLScriptManager();
	~LLScriptManager();
public:
	static LLScriptManager* GetSingleInstance();
	bool LoadScriptFromFile(wstring filePath);
	void UnLoadScriptFromFile(wstring filePath);
	Parameter RunFunction(wstring scriptName, wstring functionName, vector<Parameter>* inputList = nullptr);
	bool IsLegalType(wstring typeName);
	void AddLegalType(Class* classPtr);
	void AddGlobalFunction(Function* functionPtr);
	void AddGlobalParameter(Parameter* parameterPtr);
	Function* GetGlobalFunction(wstring functionName);
	Parameter* GetGlobalParameter(wstring parameterName);
	void RemoveLegalType(Class* classPtr);
	void RemoveGlobalFunction(Function* functionPtr);
	void RemoveGlobalParameter(Parameter* parameterPtr);
	Class* GetTypeInstanceByName(wstring typeName);
private:
	void LoadSystemAndGame();
	unordered_map<wstring, LLScript*> llScriptMap;
	unordered_map<wstring, Class*> legalTypeMap;
	unordered_map<wstring, Function*> globalFunctionMap;
	unordered_map<wstring, Parameter*> globalParameterMap;

	Parameter* systemParameter = nullptr;
	Parameter* gameParameter = nullptr;
	static LLScriptManager*  scriptManager;
};
