#pragma once
#include "Parameter.h"

class LLScript;

class Function
{
public:
	Function(wstring name, LLScript* scriptPtr,Class* classptr);
	Parameter Run(vector<Parameter>* inputList = nullptr);
	void SetContent(wstring content);
protected:
	unordered_map<wstring, Parameter*> localParameterMap;
	vector<Parameter> inputParameterList;
	Class* classPtr = nullptr;
	LLScript* scriptPtr = nullptr;
	wstring name = L"";
	wstring content = L"";
private:
	Parameter* GetParameter(wstring pName);

	Parameter dhyc(wistringstream& wsstream);

	wistringstream  wsstream;
};
