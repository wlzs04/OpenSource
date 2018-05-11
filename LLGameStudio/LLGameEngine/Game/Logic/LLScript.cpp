#include "LLScript.h"
#include "LLScriptManager.h"
#include "..\..\Common\Helper\SystemHelper.h"
#include "LLScriptGrammar.h"

LLScript::LLScript(wstring filePath)
{
	this->filePath = filePath;
	LoadScriptFromFile();
}

LLScript::~LLScript()
{
	for (auto var : parameterMap)
	{
		delete var.second;
	}
	for (auto var : functionMap)
	{
		delete var.second;
	}
	for (auto var : classMap)
	{
		delete var.second;
	}
}

Parameter LLScript::RunFunction(wstring functionName)
{
	if (functionMap.count(functionName) != 0)
	{
		return functionMap[functionName]->Run(nullptr);
	}
	return Parameter(L"int");
}

Parameter* LLScript::GetParameter(wstring pName)
{
	if (parameterMap.count(pName) != 0)
	{
		return parameterMap[pName];
	}
	return nullptr;
}

Function* LLScript::GetFunction(wstring fName)
{
	if (functionMap.count(fName) != 0)
	{
		return functionMap[fName];
	}
	return nullptr;
}

bool LLScript::LoadScriptFromFile()
{
	wifstream file(SystemHelper::GetResourceRootPath() + L"\\" + filePath);
	if (!file)
	{
		return false;
	}
	file.imbue(locale(locale::empty(), new codecvt_utf8<wchar_t>));

	LoadUnknown(file, nullptr);

	file.close();
	return true;
}

bool LLScript::LoadUnknown(wifstream& file, Class* classptr)
{
	wchar_t tempWChar;
	wstring tempWString;
	while (!file.eof())
	{
		file >> tempWChar;
		if (file.fail())
		{
			return true;
		}
		if (tempWChar == L'@')
		{
			file >> tempWString;
			LLScriptManager::GetSingleInstance()->LoadScriptFromFile(tempWString.substr(1, tempWString.size() - 2));
		}
		else if (tempWChar == L'/')
		{
			file.get(tempWChar);
			if (tempWChar == L'*')
			{
				file.get(tempWChar);
				while (true)
				{
					if (tempWChar == L'*')
					{
						file.get(tempWChar);
						if (tempWChar == L'/')
						{
							break;
						}
					}
					file.get(tempWChar);
				}
			}
			else if (tempWChar == L'/')
			{
				file.get(tempWChar);
				while (tempWChar != L'\n')
				{
					file.get(tempWChar);
				}
			}
		}
		else if (tempWChar == L'}')
		{
			return true;
		}
		else if (tempWChar == L';')
		{
		}
		else
		{
			wsstream.clear();
			wsstream.str(L"");
			wsstream << tempWChar;
			file.get(tempWChar);

			while (!LLScriptGrammar::WCharIsSpecial(tempWChar))
			{
				wsstream << tempWChar;
				file.get(tempWChar);
			}
			tempWString = wsstream.str();

			if (tempWString == L"class")
			{
				file >> tempWString;
				Class* classptr = new Class(tempWString);
				LLScriptManager::GetSingleInstance()->AddLegalType(classptr);
				classMap[tempWString] = classptr;
				LoadClass(file, classptr);
			}
			else
			{
				if (LLScriptManager::GetSingleInstance()->IsLegalType(tempWString))
				{
					wstring typeName = tempWString;
					file.get(tempWChar);
					wsstream.clear();
					wsstream.str(L"");

					while (!LLScriptGrammar::WCharIsSpecial(tempWChar))
					{
						wsstream << tempWChar;
						file.get(tempWChar);
					}

					wstring tempName = wsstream.str();

					while (LLScriptGrammar::WCharCanIgnore(tempWChar))
					{
						file.get(tempWChar);
					}
					
					if (tempWChar == L'=')
					{
						Parameter tempP = LoadValue(file);
						Parameter* p = new Parameter(typeName, tempName);
						p->SetValue(tempP.GetValueToWString());
						if (classptr == nullptr)
						{
							parameterMap[tempName] = p;
						}
						else
						{
							classptr->AddParamterDefine(p);
						}
					}
					else if (tempWChar == L';')
					{
						Parameter* p = new Parameter(typeName, tempName);
						if (classptr == nullptr)
						{
							parameterMap[tempName] = p;
						}
						else
						{
							classptr->AddParamterDefine(p);
						}
					}
					else if (tempWChar == L'(')
					{
						Function* function = new Function(tempName, typeName, this, classptr);
						
						if (tempName == L"SetInputValue")
						{
							int y = 0;
						}

						if (classptr == nullptr)
						{
							functionMap[tempName] = function;
						}
						else
						{
							classptr->AddFunctionDefine(function);
						}

						LoadFunction(file, function);
					}
				}
				else
				{
					return false;
				}
			}
		}
	}
	return true;
}

bool LLScript::LoadFunction(wifstream& file, Function* functionptr)
{
	wchar_t endFlag;
	int level = 0;
	wsstream.str(L"");//需要进行两步操作，只用clear的话wsstream内的保存的字符串是不变的。
	wsstream.clear();//需要清空缓存
	wstring tempWString;
	//读取参数信息
	file>> endFlag;
	while (endFlag!= L')')
	{
		while (endFlag == L',' || endFlag == L' ')
		{
			file.get(endFlag);
		}
		if (endFlag == L')')
		{
			break;
		}
		wsstream.str(L"");
		wsstream << endFlag;
		file.get(endFlag);

		while (!LLScriptGrammar::WCharIsSpecial(endFlag))
		{
			wsstream << endFlag;
			file.get(endFlag);
		}
		tempWString = wsstream.str();
		if (LLScriptManager::GetSingleInstance()->IsLegalType(tempWString))
		{
			wstring typeName = tempWString;
			file.get(endFlag);
			wsstream.clear();
			wsstream.str(L"");
			while (!LLScriptGrammar::WCharIsSpecial(endFlag))
			{
				wsstream << endFlag;
				file.get(endFlag);
			}
			tempWString= wsstream.str();
			Parameter* p=new Parameter(typeName, tempWString);
			functionptr->AddFunctionDefineInputValue(p);
		}
	}
	wsstream.str(L"");
	file >> endFlag;
	if (endFlag != L'{')
	{
		return false;
	}
	while (!file.eof())
	{
		file .get(endFlag);
		if (file.fail())
		{
			return false;
		}
		if (endFlag == L'}')
		{
			if (level <= 0)
			{
				functionptr->SetContent(wsstream.str());
				return true;
			}
			level--;
		}
		else if (endFlag == L'{')
		{
			level++;
		}
		wsstream << endFlag;
	}
	return false;
}

bool LLScript::LoadClass(wifstream& file, Class* classptr)
{
	wstring tempWstring;
	file >> tempWstring;
	if (tempWstring == L"{")
	{
		if (LoadUnknown(file, classptr))
		{
			return true;
		}
	}
	return false;
}

Parameter LLScript::LoadValue(wifstream & file)
{
	Parameter leftParameter;
	wsstream.clear();
	wsstream.str(L"");
	wchar_t tempWChar;
	wstring tempWStirng;
	file>>tempWChar;

	if (tempWChar==L'"')
	{
		while (!file.eof())
		{
			file.get(tempWChar);
			if (file.fail())
			{
				break;
			}
			if (tempWChar == L'"')
			{
				leftParameter.SetClassName(L"string");
				leftParameter.SetValue(wsstream.str());
				return leftParameter;
			}
			wsstream << tempWChar;
		}
	}
	else if(L'0' <= tempWChar && tempWChar <= L'9')
	{
		wsstream << tempWChar;
		while (!file.eof())
		{
			file.get(tempWChar);
			if (file.fail())
			{
				break;
			}
			if(tempWChar == L';'|| tempWChar == L' ')
			{
				tempWStirng = wsstream.str();
				StringEnum se = WStringHelper::GetStringEnum(tempWStirng);
				if (se == StringEnum::Int)
				{
					leftParameter.SetClassName(L"int");
				}
				else
				{
					leftParameter.SetClassName(L"float");
				}
				leftParameter.SetValue(tempWStirng);
				return leftParameter;
			}
			wsstream << tempWChar;
		}
	}
	else
	{
		wsstream << tempWChar;
		file.get(tempWChar);
		while (!LLScriptGrammar::WCharIsSpecial(tempWChar))
		{
			wsstream << tempWChar;
			file.get(tempWChar);
		}
		tempWStirng = wsstream.str();
		if (LLScriptGrammar::WStringIsKeyWord(tempWStirng))
		{
			if (tempWStirng == L"true" || tempWStirng == L"false")
			{
				leftParameter.SetClassName(L"bool");
				leftParameter.SetValue(tempWStirng);
			}
		}
	}
	return leftParameter;
}
