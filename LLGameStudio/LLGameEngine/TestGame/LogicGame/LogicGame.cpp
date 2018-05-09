#include "LogicGame.h"

void LogicGame::InitUserData()
{
	scriptManager = LLScriptManager::GetSingleInstance();
	scriptManager->LoadScriptFromFile(L"logic\\gameLogic.llscript");
	Parameter p;
	scriptManager->RunFunction(L"logic\\gameLogic.llscript", L"AddS");
	p = scriptManager->RunFunction(L"logic\\gameLogic.llscript", L"GetS");
	//p = scriptManager->RunFunction(L"logic\\gameLogic.llscript", L"GetI");
	int u = 0;
}

void LogicGame::UpdateUserData()
{

}
