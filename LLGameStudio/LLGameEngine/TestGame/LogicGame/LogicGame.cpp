#include "LogicGame.h"
#include "..\..\Game\Logic\ScriptClass\Game.h"

Parameter LogicGame::RunGame(vector<Parameter>* inputList)
{
	int y = 0;
	return Parameter();
}

void LogicGame::OnStartGame(void* sender, int e)
{
	scriptManager->RunFunction(L"logic\\gameLogic.llscript", L"ReLoadScript");
}

void LogicGame::InitUserData()
{
	scriptManager = LLScriptManager::GetSingleInstance();
	scriptManager->LoadScriptFromFile(L"logic\\gameLogic.llscript");

	Parameter* gameParameter = scriptManager->GetGlobalParameter(L"game");

	Function* runGameFunction = new Function(L"RunGame",L"void",nullptr, gameParameter->GetClassPtr());
	runGameFunction->SetCppFunction(std::bind(&LogicGame::RunGame, this, placeholders::_1));

	((Game*)(gameParameter->GetClassPtr()))->AddCppFunction(runGameFunction);

	Parameter p;
	//scriptManager->RunFunction(L"logic\\gameLogic.llscript", L"MakeTimerTo600");
	/*vector<Parameter> tempPList;
	tempPList.push_back(Parameter(L"int", L"p1", L"5"));
	tempPList.push_back(Parameter(L"float", L"p2", L"7.2"));
	p = scriptManager->RunFunction(L"logic\\gameLogic.llscript", L"GetInputValueFromCpp", &tempPList);*/
	//p = scriptManager->RunFunction(L"logic\\gameLogic.llscript", L"GetB2");
	//p = scriptManager->RunFunction(L"logic\\gameLogic.llscript", L"GetI");
	p = scriptManager->RunFunction(L"logic\\gameLogic.llscript", L"UserCPPFunction");
	
	startLayout = (LLGameLayout*)gameScene->GetNode(L"startButtonLayout");
	startButton = (LLGameButton*)startLayout->GetNode(L"grid1\\buttonStart");
	startButton->OnMouseClick = bind(&LogicGame::OnStartGame, this, placeholders::_1, placeholders::_2);;
}

void LogicGame::UpdateUserData()
{
	//scriptManager->RunFunction(L"logic\\gameLogic.llscript", L"AddTimer");
	Parameter p = scriptManager->RunFunction(L"logic\\gameLogic.llscript", L"UserAnotherFunction");
	
	startButton->SetText(p.GetValueToWString());
}
