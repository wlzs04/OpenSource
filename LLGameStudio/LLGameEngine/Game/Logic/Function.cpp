#include "Function.h"
#include "Class.h"
#include "LLScript.h"
#include "LLScriptManager.h"

Function::Function(wstring name, LLScript* scriptPtr, Class* classptr)
{
	this->name = name;
	this->scriptPtr = scriptPtr;
	this->classPtr = classPtr;
}

Parameter Function::Run(vector<Parameter>* inputList)
{
	if (inputList != nullptr)
	{
		for (int i = 0; i < inputList->size(); i++)
		{
			inputParameterList[i].value = (*inputList)[i].value;
		}
	}

	wsstream.clear();
	wsstream.str(content);
	Parameter returnP;

	wstring tempWstring;
	while (!wsstream.eof())
	{
		wsstream >> tempWstring;
		if (wsstream.fail())
		{
			return returnP;
		}
		if (tempWstring == L"return")
		{
			wsstream >> tempWstring;
			wstring tempWstring2;
			wsstream >> tempWstring2;
			if (tempWstring2 == L";")
			{
				Parameter* pptr = GetParameter(tempWstring);

				returnP.type = pptr->type;
				returnP.value = pptr->value;
			}
		}
		else if (LLScriptManager::GetSingleInstance()->IsLegalType(tempWstring))
		{

		}
		else
		{
			Parameter* pptr = GetParameter(tempWstring);
			if (pptr != nullptr)
			{
				wsstream >> tempWstring;
				if (tempWstring==L"=")
				{
					pptr->value = dhyc(wsstream).value;
				}
			}
			else
			{
				return returnP;
			}
		}
	}
	
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
			float value = WStringHelper::GetFloat(tempWstring);
			leftParameter.value = to_wstring(value);
			leftParameter.type = L"float";
		}
		else if (tempWstring == L";")
		{
			return leftParameter;
		}
		else if(tempWstring == L"+")
		{
			if (leftParameter.type == L"int" ||leftParameter.type == L"float")
			{
				float value = WStringHelper::GetFloat(leftParameter.value);
				wstring wsv = dhyc(wsstream).value;
				value+= WStringHelper::GetFloat(wsv);
				leftParameter.value = to_wstring(value);
				return leftParameter;
			}
		}
		else
		{
			Parameter* tempP = GetParameter(tempWstring);
			if (tempP != nullptr)
			{
				leftParameter = *tempP;
			}
		}
	}
}
