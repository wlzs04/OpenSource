#include "Function.h"
#include "Class.h"
#include "LLScript.h"
#include "LLScriptManager.h"
#include "Parameter.h"
#include "LLScriptGrammar.h"
#include "ScriptClass\bool.h"

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

	wsstream.str(content);
	returnP = Parameter(returnClassName);

	RunInSpace(wsstream);
	
	for (auto var : localParameterMap)
	{
		delete var.second;
	}
	localParameterMap.clear();
	wsstream.str(L"");
	wsstream.clear();
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
	}
	else if (classPtr != nullptr)
	{
		tempP = classPtr->GetParameter(pName);
		if (tempP != nullptr)
		{
			return tempP;
		}
	}
	else if (scriptPtr != nullptr)
	{
		tempP = scriptPtr->GetParameter(pName);
		if (tempP != nullptr)
		{
			return tempP;
		}
	}
	return tempP;
}

Parameter Function::GetTempValue(wstringstream & wsstream)
{
	wchar_t tempWChar=L' ';
	bool startCheck = false;
	Parameter leftParameter;
	valueStream.clear();
	valueStream.str(L"");
	wstring tempWString;

	while (!wsstream.eof())
	{
		if (!wsstream.fail())
		{
			if (LLScriptGrammar::WCharCanIgnore(tempWChar))
			{
				if (startCheck)
				{
					startCheck = false;
				}
				else
				{
					wsstream.get(tempWChar);
					continue;
				}
			}
			else if (tempWChar == L')')
			{
				return leftParameter;
			}
			else if (tempWChar == L',')
			{
				return leftParameter;
			}
			else if (tempWChar == L';')
			{
				return leftParameter;
			}
			else if (LLScriptGrammar::WCharIsOperator(tempWChar))
			{
				Parameter tempP = GetTempValue(wsstream);
				leftParameter.DoFunctionByoperator(tempWChar, tempP);
				return leftParameter;
			}
			else if ((L'0' <= tempWChar && tempWChar <= L'9'))
			{
				valueStream << tempWChar;
				wsstream.get(tempWChar);
				while (tempWChar != L' '&& tempWChar != L';' && LLScriptGrammar::WCharIsOperator(tempWChar))
				{
					valueStream << tempWChar;
					wsstream.get(tempWChar);
				}
				tempWString = valueStream.str();

				StringEnum es = WStringHelper::GetStringEnum(tempWString);
				if (es == StringEnum::Int)
				{
					leftParameter.SetClassName(L"int");
				}
				else if (es == StringEnum::Float)
				{
					leftParameter.SetClassName(L"float");
				}
				leftParameter.SetValue(tempWString);
			}
			else if (tempWChar == L'"')
			{
				wsstream.get(tempWChar);

				while (tempWChar != L'"')
				{
					valueStream << tempWChar;
					wsstream.get(tempWChar);
				}
				leftParameter.SetClassName(L"string");
				leftParameter.SetValue(valueStream.str());
				return leftParameter;
			}
			else
			{
				valueStream << tempWChar;
				wsstream.get(tempWChar);
				while (!LLScriptGrammar::WCharIsSpecial(tempWChar))
				{
					valueStream << tempWChar;
					wsstream.get(tempWChar);
				}
				tempWString = valueStream.str();

				Function* tempF = GetFunction(tempWString);
				if (tempF != nullptr)
				{
					if (LLScriptGrammar::WCharCanIgnore(tempWChar))
					{
						wsstream >> tempWChar;
					}
					if (tempWChar == L'(')
					{
						//读取方法内参数。
						vector<Parameter> inputNewList;
						Parameter tempInputP = GetTempValue(wsstream);
						while (!tempInputP.IsEmpty())
						{
							inputNewList.push_back(tempInputP);
						}
						leftParameter = tempF->Run(&inputNewList);
					}
				}
				else
				{
					Parameter* tempP = GetParameter(tempWString);
					if (tempP != nullptr)
					{
						leftParameter = *tempP;
					}
				}
			}
		}
	}
	return leftParameter;
}

Function * Function::GetFunction(wstring fName)
{
	Function* tempF = nullptr;
	if (classPtr != nullptr)
	{
		tempF = classPtr->GetFunction(fName);
		if (tempF != nullptr)
		{
			return tempF;
		}
	}
	else if (scriptPtr != nullptr)
	{
		tempF = scriptPtr->GetFunction(fName);
		if (tempF != nullptr)
		{
			return tempF;
		}
	}
	return tempF;
}

void Function::RunInSpace(wstringstream & wsstream)
{
	valueStream.clear();
	valueStream.str(L"");
	wchar_t tempWchar;
	while (!wsstream.eof())
	{
		wsstream >> tempWchar;
		if (wsstream.fail())
		{
			break;
		}
		if (tempWchar == L';')
		{

		}
		else if (tempWchar == L'}')
		{
			break;
		}
		else if (tempWchar == L'/')
		{
			wsstream.get(tempWchar);
			if (tempWchar == L'/')
			{
				wsstream.get(tempWchar);
				while (tempWchar != L'\n')
				{
					wsstream.get(tempWchar);
				}
			}
		}
		else
		{
			wstring tempWString;
			valueStream.str(L"");
			valueStream << tempWchar;
			wsstream.get(tempWchar);
			while (!LLScriptGrammar::WCharIsSpecial(tempWchar))
			{
				valueStream << tempWchar;
				wsstream.get(tempWchar);
			}
			tempWString = valueStream.str();
			valueStream.str(L"");
			if (LLScriptGrammar::WStringIsKeyWord(tempWString))
			{
				if (tempWString == L"return")
				{
					Parameter tempP = GetTempValue(wsstream);
					returnP.CopyClass(tempP);
					break;
				}
				else if(tempWString == L"else")
				{
					valueStream.str(L"");
					wsstream >> tempWchar;
					while (!LLScriptGrammar::WCharIsSpecial(tempWchar))
					{
						valueStream << tempWchar;
						wsstream.get(tempWchar);
					}
					tempWString = valueStream.str();
					if (tempWString == L"if")
					{
						while (tempWchar!=L'{')
						{
							wsstream.get(tempWchar);
						}
						{
							JumpOverSpace(wsstream);
						}
					}
					else if (tempWString == L"{")
					{
						JumpOverSpace(wsstream);
					}
				}
				else if (tempWString == L"if")
				{
					while (true)
					{
						while (LLScriptGrammar::WCharCanIgnore(tempWchar))
						{
							wsstream.get(tempWchar);
						}
						if (tempWchar == L'(')
						{
							Parameter tempP = GetTempValue(wsstream);
							if (((Bool*)tempP.GetClassPtr())->GetValue())
							{
								wsstream >> tempWchar;
								if (tempWchar == L'{')
								{
									RunInSpace(wsstream);
									break;
								}
							}
							else
							{
								wsstream >> tempWchar;
								if (tempWchar == L'{')
								{
									JumpOverSpace(wsstream);
								}
							}
						}
						valueStream.clear();
						valueStream.str(L"");
						wsstream >> tempWchar;
						while (!LLScriptGrammar::WCharIsSpecial(tempWchar))
						{
							valueStream << tempWchar;
							wsstream.get(tempWchar);
						}
						tempWString = valueStream.str();
						valueStream.str(L"");
						if (tempWString == L"else")
						{
							valueStream.str(L"");
							wsstream >> tempWchar;
							while (!LLScriptGrammar::WCharIsSpecial(tempWchar)|| tempWchar==L'{')
							{
								valueStream << tempWchar;
								wsstream.get(tempWchar);
							}
							tempWString = valueStream.str();
							if (tempWString == L"if")
							{
								continue;
							}
							else if (tempWString == L"{")
							{
								RunInSpace(wsstream);
								break;
							}
						}
					}
				}
				else if(tempWString == L"while")
				{
					while (LLScriptGrammar::WCharCanIgnore(tempWchar))
					{
						wsstream.get(tempWchar);
					}
					if (tempWchar == L'(')
					{
						streampos temppos = wsstream.tellg();
						while (true)
						{
							wsstream.seekg(temppos);
							Parameter tempP = GetTempValue(wsstream);
							wsstream >> tempWchar;
							if (tempWchar == L'{')
							{
								if (((Bool*)tempP.GetClassPtr())->GetValue())
								{
									RunInSpace(wsstream);
								}
								else
								{
									JumpOverSpace(wsstream);
									break;
								}
							}
						}
					}
				}
			}
			else if (LLScriptManager::GetSingleInstance()->IsLegalType(tempWString))
			{
				wstring typeName = tempWString;
				wsstream.get(tempWchar);
				valueStream.clear();
				valueStream.str(L"");

				while (!LLScriptGrammar::WCharIsSpecial(tempWchar))
				{
					valueStream << tempWchar;
					wsstream.get(tempWchar);
				}

				wstring tempName = valueStream.str();

				while (LLScriptGrammar::WCharCanIgnore(tempWchar))
				{
					wsstream.get(tempWchar);
				}

				if (tempWchar == L'=')
				{
					Parameter pd = GetTempValue(wsstream);
					Parameter* p = new Parameter(typeName, tempName);
					p->CopyClass(pd);
					localParameterMap[tempName] = p;
				}
				else if (tempWchar == L';')
				{
					Parameter* p = new Parameter(typeName, tempName);
					localParameterMap[tempName] = p;
				}
			}
			else
			{
				Function* tempF = GetFunction(tempWString);
				if (tempF != nullptr)
				{
					if (LLScriptGrammar::WCharCanIgnore(tempWchar))
					{
						wsstream >> tempWchar;
					}
					if (tempWchar == L'(')
					{
						//读取方法内参数。
						vector<Parameter> inputNewList;
						Parameter tempInputP = GetTempValue(wsstream);
						while (!tempInputP.IsEmpty())
						{
							inputNewList.push_back(tempInputP);
						}
						tempF->Run(&inputNewList);
					}
				}
				else
				{
					Parameter* pptr = GetParameter(tempWString);
					if (pptr != nullptr)
					{
						while (LLScriptGrammar::WCharCanIgnore(tempWchar))
						{
							wsstream.get(tempWchar);
						}

						if (tempWchar == L'=')
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
		}
	}
}

void Function::JumpOverSpace(wstringstream & wsstream)
{
	wchar_t tempWchar;
	bool inString = false;

	while (!wsstream.eof())
	{
		wsstream >> tempWchar;
		if (wsstream.fail())
		{
			break;
		}
		if (tempWchar == L'"')
		{
			if (inString)
			{
				inString = false;
			}
			else
			{
				inString = true;
			}
		}
		else if(tempWchar == L'{')
		{
			JumpOverSpace(wsstream);
		}
		else if(tempWchar == L'}')
		{
			break;
		}
	}
}
