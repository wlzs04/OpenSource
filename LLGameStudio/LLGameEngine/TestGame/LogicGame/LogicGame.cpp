#include "LogicGame.h"

void LogicGame::InitUserData()
{
	scriptManager = LLScriptManager::GetSingleInstance();
	scriptManager->LoadScriptFromFile(L"logic\\gameLogic.llscript");
	Parameter p;
	scriptManager->RunFunction(L"logic\\gameLogic.llscript", L"MakeTimerTo600");
	//p = scriptManager->RunFunction(L"logic\\gameLogic.llscript", L"GetB3");
	//p = scriptManager->RunFunction(L"logic\\gameLogic.llscript", L"GetB2");
	//p = scriptManager->RunFunction(L"logic\\gameLogic.llscript", L"GetI");
	startLayout = (LLGameLayout*)gameScene->GetNode(L"startButtonLayout");
	startButton = (LLGameButton*)startLayout->GetNode(L"grid1\\buttonStart");
}

void LogicGame::UpdateUserData()
{
	//scriptManager->RunFunction(L"logic\\gameLogic.llscript", L"AddTimer");
	Parameter p = scriptManager->RunFunction(L"logic\\gameLogic.llscript", L"GetS");
	
	startButton->SetText(p.GetValueToWString()+L":"+to_wstring(gameTimer.GetThisTickTime()));
}
