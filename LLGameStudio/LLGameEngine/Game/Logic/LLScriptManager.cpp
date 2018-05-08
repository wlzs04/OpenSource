#include "LLScriptManager.h"
#include "ScriptClass\Int.h"
#include "ScriptClass\Void.h"
#include "ScriptClass\Float.h"

LLScriptManager::LLScriptManager()
{
	AddLegalType(new Int());
	AddLegalType(new Void());
	AddLegalType(new Float());
}

LLScriptManager* LLScriptManager::GetSingleInstance()
{
	LLScriptManager* scriptManager = new LLScriptManager();
	return scriptManager;
}

bool LLScriptManager::LoadScriptFromFile(wstring filePath)
{
	if (llScriptMap.count(filePath) == 0)
	{
		llScriptMap[filePath] = new LLScript(filePath);
	}
	return true;
	//SystemHelper::GetResourceRootPath() + L"\\" +
}

void LLScriptManager::UnLoadScriptFromFile(wstring filePath)
{
	if (llScriptMap.count(filePath) != 0)
	{
		delete llScriptMap[filePath];
		llScriptMap.erase(filePath);
	}
}

Parameter LLScriptManager::RunFunction(wstring scriptName, wstring functionName)
{
	if (llScriptMap.count(scriptName) != 0)
	{
		return llScriptMap[scriptName]->RunFunction(functionName);
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

Class* LLScriptManager::GetTypeInstanceByName(wstring typeName)
{
	return legalTypeMap[typeName]->GetInstance();
}
