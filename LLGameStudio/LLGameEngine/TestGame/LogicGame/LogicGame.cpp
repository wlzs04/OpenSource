#include "LogicGame.h"

void LogicGame::InitUserData()
{
	scriptManager = LLScriptManager::GetSingleInstance();
	scriptManager->LoadScriptFromFile(L"logic\\gameLogic.llscript");
	scriptManager->RunFunction(L"logic\\gameLogic.llscript", L"AddI");
	Parameter p = scriptManager->RunFunction(L"logic\\gameLogic.llscript",L"GetI");
	wstring y = p.value;
}

void LogicGame::UpdateUserData()
{

}
