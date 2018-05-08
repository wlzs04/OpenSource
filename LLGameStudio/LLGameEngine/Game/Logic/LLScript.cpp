#include "LLScript.h"
#include "LLScriptManager.h"
#include "..\..\Common\Helper\SystemHelper.h"

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
		return functionMap[functionName]->Run();
	}
	return Parameter();
}

Parameter* LLScript::GetParameter(wstring pName)
{
	if (parameterMap.count(pName) != 0)
	{
		return parameterMap[pName];
	}
}

Function* LLScript::GetFunction(wstring fName)
{
	if (functionMap.count(fName) != 0)
	{
		return functionMap[fName];
	}
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
	wstring tempWstring;
	while (!file.eof())
	{
		file >> tempWstring;
		if (file.fail())
		{
			return true;
		}

		if (tempWstring == L"#include")
		{
			file >> tempWstring;
			LLScriptManager::GetSingleInstance()->LoadScriptFromFile(tempWstring);
		}
		else if (tempWstring == L"class")
		{
			file >> tempWstring;
			Class* classptr = new Class(tempWstring);
			LLScriptManager::GetSingleInstance()->AddLegalType(classptr);
			classMap[tempWstring] = classptr;
			LoadClass(file, classptr);
		}
		else if (tempWstring == L"}")
		{
			return true;
		}
		else if (tempWstring == L";")
		{
		}
		else
		{
			if (LLScriptManager::GetSingleInstance()->IsLegalType(tempWstring))
			{
				wstring typeName = tempWstring;
				wstring tempName;
				file >> tempName;
				wstring tempX;
				file >> tempX;
				if (tempX == L"=")
				{
					file >> tempX;
					Parameter* p = new Parameter();
					p->name = tempName;
					p->type = typeName;
					p->value = tempX;
					parameterMap[tempName] = p;
				}
				else if (tempX == L";")
				{
					Parameter* p = new Parameter();
					p->name = tempName;
					p->type = typeName;
					parameterMap[tempName] = p;
				}
				else if (tempX == L"(")
				{
					Function* function = new Function(tempName,this,classptr);
					functionMap[tempName] = function;
					LoadFunction(file, function);
				}
			}
			else
			{
				return false;
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

	//读取参数信息
	file>> endFlag;
	if (endFlag = L')')
	{
		file >> endFlag;
		if (endFlag != L'{')
		{
			return false;
		}
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
