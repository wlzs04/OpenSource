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
}

void Class::RemoveClassDefine()
{
	for (auto var : functionMap)
	{
		delete var.second;
	}
	functionMap.clear();
	for (auto var : parameterMap)
	{
		delete var.second;
	}
	parameterMap.clear();
}

Class* Class::GetInstance()
{
	Class* classPtr = new Class(name);
	classPtr->SetValue(GetValueToWString());
	for (auto var : parameterMap)
	{
		Parameter* p = new Parameter();
		p->SetName(var.second->GetName());
		p->CopyClass(*var.second);
		classPtr->AddParamterDefine(p);
	}
	for (auto var : functionMap)
	{
		classPtr->AddFunctionDefine(var.second);
	}
	return classPtr;
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
	return nullptr;
}

void Class::AddParamterDefine(Parameter* p)
{
	parameterMap[p->GetName()]=p;
}

void Class::AddFunctionDefine(Function* f)
{
	functionMap[f->GetName()] = f;
}

Function* Class::GetFunction(wstring fName)
{
	if (functionMap.count(fName) != 0)
	{
		return functionMap[fName];
	}
	return nullptr;
}

Parameter Class::Add(Class * classptr)
{
	return Parameter();
}

Parameter Class::Subtract(Class * classptr)
{
	return Parameter();
}

Parameter Class::Multiple(Class* classptr)
{
	return Parameter();
}

Parameter Class::Divide(Class * classptr)
{
	return Parameter();
}

Parameter Class::Complementation(Class * classptr)
{
	return Parameter();
}

Parameter Class::Intersection(Class * classptr)
{
	return Parameter();
}

Parameter Class::Union(Class * classptr)
{
	return Parameter();
}

Parameter Class::Greater(Class * classptr)
{
	return Parameter();
}

Parameter Class::Less(Class * classptr)
{
	return Parameter();
}

Parameter Class::Equal(Class * classptr)
{
	return Parameter();
}

Parameter Class::UnEqual(Class * classptr)
{
	return Parameter();
}
