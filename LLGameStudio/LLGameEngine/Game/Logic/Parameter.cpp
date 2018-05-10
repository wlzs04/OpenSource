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
	if (p.classPtr != nullptr)
	{
		classPtr = LLScriptManager::GetSingleInstance()->GetTypeInstanceByName(p.GetClassName());
		classPtr->SetValue(p.GetValueToWString());
	}
}

Parameter & Parameter::operator=(const Parameter & p)
{
	if (classPtr != nullptr)
	{
		delete classPtr;
		classPtr = nullptr;
	}
	if (p.classPtr != nullptr)
	{
		classPtr = LLScriptManager::GetSingleInstance()->GetTypeInstanceByName(p.GetClassName());
		classPtr->SetValue(p.GetValueToWString());
	}
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
	if (classPtr != nullptr)
	{
		delete classPtr;
		classPtr = nullptr;
	}
	if (p.classPtr != nullptr)
	{
		classPtr = p.classPtr->GetInstance();
		classPtr->SetValue(p.classPtr->GetValueToWString());
	}
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

void Parameter::DoFunctionByoperator(wchar_t wc, Parameter& p)
{
	if (wc == L'+')
	{
		classPtr->Add(p.GetClassPtr());
	}
	else if (wc == L'-')
	{
		classPtr->Subtract(p.GetClassPtr());
	}
	else if (wc == L'*')
	{
		classPtr->Multiple(p.GetClassPtr());
	}
	else if (wc == L'/')
	{
		classPtr->Divide(p.GetClassPtr());
	}
	else if (wc == L'%')
	{
		classPtr->Complementation(p.GetClassPtr());
	}
	else if (wc == L'&')
	{
		classPtr->Intersection(p.GetClassPtr());
	}
	else if (wc == L'|')
	{
		classPtr->Union(p.GetClassPtr());
	}
}

bool Parameter::IsEmpty()
{
	return classPtr==nullptr;
}
