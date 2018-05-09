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
					Parameter pd = GetTempValue(wsstream);
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

Parameter Function::GetTempValue(wistringstream & wsstream)
{
	wchar_t tempWChar;
	bool startCheck = false;
	Parameter leftParameter;
	wostringstream  valueStream;
	wstring tempWString;

	while (!wsstream.eof())
	{
		wsstream.get(tempWChar);
		if (!wsstream.fail())
		{
			if (WCharCanIgnore(tempWChar))
			{
				if (startCheck)
				{
					startCheck = false;
				}
				else
				{
					continue;
				}
			}
			else if (tempWChar == L';')
			{
				return leftParameter;
			}
			else if(WCharIsOperator(tempWChar))
			{
				Parameter tempP = GetTempValue(wsstream);
				leftParameter.DoFunctionByoperator(tempWChar, tempP);
				return leftParameter;
			}
			else if ((L'0' <= tempWChar && tempWChar <= L'9'))
			{
				valueStream << tempWChar;
				wsstream.get(tempWChar);
				while (tempWChar != L' ')
				{
					wsstream.get(tempWChar);
					valueStream << tempWChar;
				}
				tempWString = valueStream.str();

				StringEnum es = WStringHelper::GetStringEnum(tempWString);
				if (es == StringEnum::Int)
				{
					leftParameter.SetClassName(L"int");
				}
				else if(es == StringEnum::Float)
				{
					leftParameter.SetClassName(L"float");
				}
				leftParameter.SetValue(tempWString);
			}
			else if(tempWChar==L'"')
			{
				wsstream.get(tempWChar);

				while (tempWChar != L'"')
				{
					valueStream << tempWChar;
					wsstream.get(tempWChar);
				}
				leftParameter.SetClassName(L"string");
				leftParameter.SetValue(valueStream.str()) ;
			}
			else
			{
				valueStream << tempWChar;
				wsstream.get(tempWChar);
				while (!WCharSpecial(tempWChar))
				{
					valueStream << tempWChar;
					wsstream.get(tempWChar);
				}
				tempWString = valueStream.str();
				Parameter* tempP = GetParameter(tempWString);
				if (tempP != nullptr)
				{
					leftParameter = *tempP;
				}
			}
		}
	}
	return leftParameter;
}

bool Function::WCharCanIgnore(wchar_t wc)
{
	return (wc == L' ')//半角空格 
		|| (wc == L'　') //全角空格（输入法快捷键Shift+Space可以切换半角和全角）
		|| wc == L'\n' //换行
		|| wc == L'\r'//回车（“\r”和“\r\n”编码一样）
		|| wc == L'\t'//水平制表符
		;
}

bool Function::WCharSpecial(wchar_t wc)
{
	return (wc == L' ')//半角空格 
		|| (wc == L'.') //点
		|| wc == L';' //分号
		;
}

bool Function::WCharIsOperator(wchar_t wc)
{
	return (wc == L'+')//加
		|| (wc == L'-') //减
		|| wc == L'*' //乘
		|| wc == L'/' //除
		|| wc == L'%' //取余
		|| wc == L'&' //交
		|| wc == L'|' //并
		;
}

//Parameter Function::dhyc(wistringstream& wsstream)
//{
//	wstring tempWstring;
//	Parameter leftParameter;
//
//	while (!wsstream.eof())
//	{
//		wsstream >> tempWstring;
//		
//		if (tempWstring[0]<=L'9'&&tempWstring[0]>=L'0')
//		{
//			StringEnum se = WStringHelper::GetStringEnum(tempWstring);
//			if (se == StringEnum::Int)
//			{
//				leftParameter.SetClassName(L"int");
//				leftParameter.SetValue(tempWstring);
//			}
//			else if (se == StringEnum::Float)
//			{
//				leftParameter.SetClassName(L"float");
//				leftParameter.SetValue(tempWstring);
//			}
//		}
//		else if (tempWstring == L";")
//		{
//			return leftParameter;
//		}
//		else if(tempWstring == L"+")
//		{
//			Parameter wsv = dhyc(wsstream);
//			leftParameter.GetClassPtr()->Add(wsv.GetClassPtr());
//			return leftParameter;
//		}
//		else if (tempWstring == L"-")
//		{
//			Parameter wsv = dhyc(wsstream);
//			leftParameter.GetClassPtr()->Subtract(wsv.GetClassPtr());
//			return leftParameter;
//		}
//		else if (tempWstring == L"*")
//		{
//			Parameter wsv = dhyc(wsstream);
//			leftParameter.GetClassPtr()->Multiple(wsv.GetClassPtr());
//			return leftParameter;
//		}
//		else if (tempWstring == L"/")
//		{
//			Parameter wsv = dhyc(wsstream);
//			leftParameter.GetClassPtr()->Divide(wsv.GetClassPtr());
//			return leftParameter;
//		}
//		else
//		{
//			Parameter* tempP = GetParameter(tempWstring);
//			if (tempP != nullptr)
//			{
//				leftParameter = *tempP;
//			}
//			else
//			{
//
//			}
//		}
//	}
//	return leftParameter;
//}
