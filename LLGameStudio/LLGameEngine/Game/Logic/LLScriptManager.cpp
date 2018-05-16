#include "LLScriptManager.h"
#include "ScriptClass\Int.h"
#include "ScriptClass\Void.h"
#include "ScriptClass\Float.h"
#include "ScriptClass\String.h"
#include "ScriptClass\Bool.h"
#include "ScriptClass\Ptr.h"
#include "ScriptClass\System.h"
#include "ScriptClass\Game.h"

LLScriptManager* LLScriptManager::scriptManager = nullptr;

LLScriptManager::LLScriptManager()
{
	AddLegalType(new Int());
	AddLegalType(new Void());
	AddLegalType(new Float());
	AddLegalType(new String());
	AddLegalType(new Bool());
	AddLegalType(new Ptr());
	AddLegalType(new System());
	AddLegalType(new Game());
}

LLScriptManager::~LLScriptManager()
{
	for (auto var : llScriptMap)
	{
		delete var.second;
	}
	for (auto var : globalFunctionMap)
	{
		delete var.second;
	}
	for (auto var : globalParameterMap)
	{
		delete var.second;
	}
}

LLScriptManager* LLScriptManager::GetSingleInstance()
{
	if (scriptManager==nullptr)
	{
		scriptManager = new LLScriptManager();
		scriptManager->LoadSystemAndGame();
	}
	return scriptManager;
}

bool LLScriptManager::LoadScriptFromFile(wstring filePath)
{
	if (llScriptMap.count(filePath) == 0)
	{
		llScriptMap[filePath] = new LLScript(filePath);
	}
	return true;
}

void LLScriptManager::UnLoadScriptFromFile(wstring filePath)
{
	if (llScriptMap.count(filePath) != 0)
	{
		delete llScriptMap[filePath];
		llScriptMap.erase(filePath);
	}
}

Parameter LLScriptManager::RunFunction(wstring scriptName, wstring functionName, vector<Parameter>* inputList)
{
	if (llScriptMap.count(scriptName) != 0)
	{
		return llScriptMap[scriptName]->RunFunction(functionName, inputList);
	}
	return Parameter();
}

Parameter LLScriptManager::RunFunction(wstring functionName, vector<Parameter>* inputList)
{
	if (globalFunctionMap.count(functionName) != 0)
	{
		return globalFunctionMap[functionName]->Run(nullptr, inputList);
	}
	return Parameter();
}

bool LLScriptManager::IsLegalType(wstring typeName)
{
	return legalTypeMap.count(typeName) != 0;
}

void LLScriptManager::AddLegalType(Class* classPtr)
{
	legalTypeMap[classPtr->GetName()] = classPtr;
}

void LLScriptManager::AddGlobalFunction(Function * functionPtr)
{
	globalFunctionMap[functionPtr->GetName()] = functionPtr;
}

void LLScriptManager::AddGlobalParameter(Parameter * parameterPtr)
{
	globalParameterMap[parameterPtr->GetName()] = parameterPtr;
}

Function* LLScriptManager::GetGlobalFunction(wstring functionName)
{
	if (globalFunctionMap.count(functionName) != 0)
	{
		return globalFunctionMap[functionName];
	}
	return nullptr;
}

Parameter * LLScriptManager::GetGlobalParameter(wstring parameterName)
{
	if (globalParameterMap.count(parameterName) != 0)
	{
		return globalParameterMap[parameterName];
	}
	return nullptr;
}

void LLScriptManager::RemoveLegalType(Class * classPtr)
{
	if (legalTypeMap.count(classPtr->GetName()) != 0)
	{
		legalTypeMap.erase(classPtr->GetName());
		classPtr->RemoveClassDefine();
		delete classPtr;
	}
}

void LLScriptManager::RemoveGlobalFunction(Function * functionPtr)
{
	if (globalFunctionMap.count(functionPtr->GetName()) != 0)
	{
		globalFunctionMap.erase(functionPtr->GetName());
		delete functionPtr;
	}
}

void LLScriptManager::RemoveGlobalParameter(Parameter * parameterPtr)
{
	if (globalParameterMap.count(parameterPtr->GetName()) != 0)
	{
		globalParameterMap.erase(parameterPtr->GetName());
		delete parameterPtr;
	}
}

Class* LLScriptManager::GetTypeInstanceByName(wstring typeName)
{
	return legalTypeMap[typeName]->GetInstance();
}

void LLScriptManager::LoadSystemAndGame()
{
	systemParameter = new Parameter(L"System", L"system");
	gameParameter = new Parameter(L"Game", L"game");

	AddGlobalParameter(systemParameter);
	AddGlobalParameter(gameParameter);
}
