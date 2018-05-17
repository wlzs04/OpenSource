#include "System.h"
#include "..\..\..\Common\Helper\MessageHelper.h"
#include <functional>
#include "..\LLScriptManager.h"
#include "..\..\..\Common\Helper\MathHelper.h"

System::System() :Class(L"System")
{
	Function* showMessageFunction=new Function(L"ShowMessage",L"void",nullptr,this);
	showMessageFunction->SetCppFunction(std::bind(&System::ShowMessage, this, placeholders::_1));
	AddFunctionDefine(showMessageFunction);

	Function* getNowTimeFunction = new Function(L"GetNowTime", L"int", nullptr, this);
	getNowTimeFunction->SetCppFunction(std::bind(&System::GetNowTime, this, placeholders::_1));
	AddFunctionDefine(getNowTimeFunction);

	Function* loadScriptFileFunction = new Function(L"LoadScriptFile", L"void", nullptr, this);
	loadScriptFileFunction->SetCppFunction(std::bind(&System::LoadScriptFile, this, placeholders::_1));
	AddFunctionDefine(loadScriptFileFunction);

	Function* unloadScriptFileFunction = new Function(L"UnloadScriptFile", L"void", nullptr, this);
	unloadScriptFileFunction->SetCppFunction(std::bind(&System::UnloadScriptFile, this, placeholders::_1));
	AddFunctionDefine(unloadScriptFileFunction);

	Function* getRandomFloatFunction = new Function(L"GetRandomFloat", L"void", nullptr, this);
	getRandomFloatFunction->SetCppFunction(std::bind(&System::GetRandomFloat, this, placeholders::_1));
	AddFunctionDefine(getRandomFloatFunction);
}

Class * System::GetInstance()
{
	return new System();
}

Parameter System::ShowMessage(vector<Parameter>* inputList)
{
	wstring content = L"";
	if (inputList != nullptr)
	{
		content = (*inputList)[0].GetValueToWString();
	}
	MessageHelper::ShowMessage(content);
	return Parameter();
}

Parameter System::GetNowTime(vector<Parameter>* inputList)
{
	time_t now_time;
	now_time = time(NULL);
	return Parameter(L"int",L"p1", to_wstring(now_time));
}

Parameter System::LoadScriptFile(vector<Parameter>* inputList)
{
	wstring content = L"";
	if (inputList != nullptr)
	{
		content = (*inputList)[0].GetValueToWString();
	}
	LLScriptManager::GetSingleInstance()->LoadScriptFromFile(content);
	return Parameter();
}

Parameter System::UnloadScriptFile(vector<Parameter>* inputList)
{
	wstring content = L"";
	if (inputList != nullptr)
	{
		content = (*inputList)[0].GetValueToWString();
	}
	LLScriptManager::GetSingleInstance()->UnLoadScriptFromFile(content);
	return Parameter();
}

Parameter System::GetRandomFloat(vector<Parameter>* inputList)
{
	return Parameter(L"float",L"p1",to_wstring(MathHelper::RandFloat()));
}
