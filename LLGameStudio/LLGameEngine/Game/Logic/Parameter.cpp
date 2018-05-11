#include "Parameter.h"
#include "Class.h"
#include "LLScriptManager.h"

Parameter::Parameter()
{
}

Parameter::Parameter(wstring className)
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
		classPtr = p.GetClassPtr()->GetInstance();
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
		classPtr = p.GetClassPtr()->GetInstance();
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

Class* Parameter::GetClassPtr() const
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
		classPtr = p.GetClassPtr()->GetInstance();
	}
}

wstring Parameter::GetName() const
{
	return name;
}

void Parameter::SetName(wstring name)
{
	this->name = name;
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

Parameter Parameter::DoFunctionByoperator(wchar_t wc, Parameter& p)
{
	if (wc == L'+')
	{
		return classPtr->Add(p.GetClassPtr());
	}
	else if (wc == L'-')
	{
		return classPtr->Subtract(p.GetClassPtr());
	}
	else if (wc == L'*')
	{
		return classPtr->Multiple(p.GetClassPtr());
	}
	else if (wc == L'/')
	{
		return classPtr->Divide(p.GetClassPtr());
	}
	else if (wc == L'%')
	{
		return classPtr->Complementation(p.GetClassPtr());
	}
	else if (wc == L'&')
	{
		return classPtr->Intersection(p.GetClassPtr());
	}
	else if (wc == L'|')
	{
		return classPtr->Union(p.GetClassPtr());
	}
	else if (wc == L'>')
	{
		return classPtr->Greater(p.GetClassPtr());
	}
	else if (wc == L'<')
	{
		return classPtr->Less(p.GetClassPtr());
	}
	else if (wc == L'#')
	{
		return classPtr->Equal(p.GetClassPtr());
	}
	else if (wc == L'!')
	{
		return classPtr->UnEqual(p.GetClassPtr());
	}
	return Parameter();
}

bool Parameter::IsEmpty()
{
	return classPtr==nullptr;
}
