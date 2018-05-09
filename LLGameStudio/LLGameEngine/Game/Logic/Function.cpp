#include "Function.h"
#include "Class.h"
#include "LLScript.h"
#include "LLScriptManager.h"
#include "Parameter.h"

Function::Function(wstring name, wstring returnClassName, LLScript* scriptPtr, Class* classptr)
{
	this->name = name;
	this->returnClassName = returnClassName;
	this->scriptPtr = scriptPtr;
	this->classPtr = classPtr;
}

Parameter Function::Run(vector<Parameter>* inputList)
{
	if (inputList != nullptr)
	{
		for (int i = 0; i < inputList->size(); i++)
		{
			Parameter* p=new Parameter(inputParameterList[i].GetClassName(), inputParameterList[i].GetName());
			p->CopyClass(inputParameterList[i]);
			localParameterMap[p->GetName()] = p;
		}
	}

	wsstream.clear();
	wsstream.str(content);
	Parameter returnP(returnClassName);

	wstring tempWstring;
	while (!wsstream.eof())
	{
		wsstream >> tempWstring;
		if (wsstream.fail())
		{
			break;
		}
		if (tempWstring == L"return")
		{
			wsstream >> tempWstring;
			wstring tempWstring2;
			wsstream >> tempWstring2;
			if (tempWstring2 == L";")
			{
				Parameter* pptr = GetParameter(tempWstring);
				returnP.CopyClass(*pptr);
				break;
			}
		}
		else if (LLScriptManager::GetSingleInstance()->IsLegalType(tempWstring))
		{
			wstring typeName = tempWstring;
			wstring tempName;
			wsstream >> tempName;
			wstring tempX;
			wsstream >> tempX;
			if (tempX == L"=")
			{
				wsstream >> tempX;
				Parameter* p = new Parameter(typeName, tempName);
				p->SetValue(tempX);
				localParameterMap[tempName] = p;
			}
			else if (tempX == L";")
			{
				Parameter* p = new Parameter(typeName, tempName);
				localParameterMap[tempName] = p;
			}
		}
		else
		{
			Parameter* pptr = GetParameter(tempWstring);
			if (pptr != nullptr)
			{
				wsstream >> tempWstring;
				if (tempWstring==L"=")
				{
					Parameter pd = dhyc(wsstream);
					pptr->CopyClass(pd);
				}
			}
			else
			{
				break;
			}
		}
	}
	for (auto var : localParameterMap)
	{
		delete var.second;
	}
	localParameterMap.clear();
	return returnP;
}

void Function::SetContent(wstring content)
{
	this->content = content;
}

Parameter* Function::GetParameter(wstring pName)
{
	Parameter* tempP = nullptr;
	if (localParameterMap.count(pName) != 0)
	{
		tempP = localParameterMap[pName];
		if(tempP!=nullptr)
		{
			return tempP;
		}
	}
	if (classPtr != nullptr)
	{
		tempP = classPtr->GetParameter(pName); 
		if (tempP != nullptr)
		{
			return tempP;
		}
	}
	if (scriptPtr != nullptr)
	{
		tempP = scriptPtr->GetParameter(pName);
		if (tempP != nullptr)
		{
			return tempP;
		}
	}
	return tempP;
}

Parameter Function::dhyc(wistringstream& wsstream)
{
	wstring tempWstring;
	Parameter leftParameter;

	while (!wsstream.eof())
	{
		wsstream >> tempWstring;
		
		if (tempWstring[0]<=L'9'&&tempWstring[0]>=L'0')
		{
			StringEnum se = WStringHelper::GetStringEnum(tempWstring);
			if (se == StringEnum::Int)
			{
				leftParameter.SetClassName(L"int");
				leftParameter.SetValue(tempWstring);
			}
			else if (se == StringEnum::Float)
			{
				leftParameter.SetClassName(L"float");
				leftParameter.SetValue(tempWstring);
			}
		}
		else if (tempWstring == L";")
		{
			return leftParameter;
		}
		else if(tempWstring == L"+")
		{
			Parameter wsv = dhyc(wsstream);
			leftParameter.GetClassPtr()->Add(wsv.GetClassPtr());
			return leftParameter;
		}
		else if (tempWstring == L"-")
		{
			Parameter wsv = dhyc(wsstream);
			leftParameter.GetClassPtr()->Subtract(wsv.GetClassPtr());
			return leftParameter;
		}
		else if (tempWstring == L"*")
		{
			Parameter wsv = dhyc(wsstream);
			leftParameter.GetClassPtr()->Multiple(wsv.GetClassPtr());
			return leftParameter;
		}
		else if (tempWstring == L"/")
		{
			Parameter wsv = dhyc(wsstream);
			leftParameter.GetClassPtr()->Divide(wsv.GetClassPtr());
			return leftParameter;
		}
		else
		{
			Parameter* tempP = GetParameter(tempWstring);
			if (tempP != nullptr)
			{
				leftParameter = *tempP;
			}
			else
			{

			}
		}
	}
	return leftParameter;
}
