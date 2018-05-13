#pragma once
#include "..\..\Game\LLGame.h"
#include "..\..\Game\Logic\LLScriptManager.h"

class LogicGame : public LLGame
{
public:
	Parameter RunGame(vector<Parameter>* inputList=nullptr);
	void OnStartGame(void* sender, int e);
protected:
	virtual void InitUserData() override;

	virtual void UpdateUserData() override;

	LLScriptManager* scriptManager = nullptr;

	LLGameLayout* startLayout = nullptr;
	LLGameButton* startButton = nullptr;
};