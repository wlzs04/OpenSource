﻿#pragma once
#include "Function.h"

class Class
{
public:
	Class(wstring name);
	~Class();
	virtual Class* GetInstance();
	virtual void SetValue(wstring value) {};
	virtual wstring GetValueToWString() { return wstring(); };
	wstring GetName();
	Parameter* GetParameter(wstring pName);
	Function* GetFunction(wstring fName);
	virtual void Add(Class* classptr) {};
	virtual void Subtract(Class* classptr) {};
	virtual void Multiple(Class* classptr) {};
	virtual void Divide(Class* classptr) {};
protected:
	wstring name;//类名
	unordered_map<wstring, Parameter*> parameterMap;
	unordered_map<wstring, Function*> functionMap;

};