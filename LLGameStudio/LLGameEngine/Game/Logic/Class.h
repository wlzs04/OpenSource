#pragma once
#include "Function.h"

class Class
{
public:
	Class(wstring name);
	~Class();
	Class* GetInstance();
	wstring GetName();
	Parameter* GetParameter(wstring pName);
	Function* GetFunction(wstring fName);
protected:
	wstring name;//类名
	wstring content;
	unordered_map<wstring, Parameter*> parameterMap;
	unordered_map<wstring, Function*> functionMap;

};