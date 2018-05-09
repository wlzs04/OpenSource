#include "Parameter.h"
#include "Class.h"
#include "LLScriptManager.h"

Parameter::Parameter(wstring className = L"int")
{
	classPtr = LLScriptManager::GetSingleInstance()->GetTypeInstanceByName(className);
}

Parameter::Parameter(wstring className, wstring name)
{
	classPtr = LLScriptManager::GetSingleInstance()->GetTypeInstanceByName(className);
	this->name = name;
}

Parameter::Parameter(const Parameter& p)
{
	classPtr = LLScriptManager::GetSingleInstance()->GetTypeInstanceByName(p.GetClassName());
	classPtr->SetValue(p.GetValueToWString());
}

Parameter & Parameter::operator=(const Parameter & p)
{
	classPtr = LLScriptManager::GetSingleInstance()->GetTypeInstanceByName(p.GetClassName());
	classPtr->SetValue(p.GetValueToWString());
	return *this;
}

Parameter::~Parameter()
{
	if (classPtr != nullptr)
	{
		delete classPtr;
		classPtr = nullptr;
	}
}

Class* Parameter::GetClassPtr()
{
	return classPtr;
}

void Parameter::CopyClass(Parameter & p)
{
	classPtr = p.classPtr->GetInstance();
	classPtr->SetValue(p.classPtr->GetValueToWString());
}

wstring Parameter::GetName() const
{
	return name;
}

wstring Parameter::GetValueToWString() const
{
	return classPtr->GetValueToWString();
}

void Parameter::SetValue(wstring value)
{
	classPtr->SetValue(value);
}

void Parameter::SetClassName(wstring className)
{
	if (classPtr == nullptr)
	{
		classPtr = LLScriptManager::GetSingleInstance()->GetTypeInstanceByName(className);
	}
}

wstring Parameter::GetClassName() const
{
	return classPtr->GetName();
}
