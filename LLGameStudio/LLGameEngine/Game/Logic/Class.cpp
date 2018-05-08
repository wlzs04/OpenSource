#include "Class.h"

Class::Class(wstring name)
{
	this->name = name;
}

Class::~Class()
{
	for (auto var : parameterMap)
	{
		delete var.second;
	}
	for (auto var : functionMap)
	{
		delete var.second;
	}
}

Class* Class::GetInstance()
{
	return new Class(L"");
}

std::wstring Class::GetName()
{
	return name;
}

Parameter* Class::GetParameter(wstring pName)
{
	if (parameterMap.count(pName) != 0)
	{
		return parameterMap[pName];
	}
}

Function* Class::GetFunction(wstring fName)
{
	if (functionMap.count(fName) != 0)
	{
		return functionMap[fName];
	}
}
