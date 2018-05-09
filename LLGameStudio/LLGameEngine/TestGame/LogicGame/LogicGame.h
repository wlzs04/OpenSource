#pragma once
#include "..\..\Game\LLGame.h"
#include "..\..\Game\Logic\LLScriptManager.h"

class LogicGame : public LLGame
{
public:

protected:
	virtual void InitUserData() override;

	virtual void UpdateUserData() override;

	LLScriptManager* scriptManager = nullptr;

	LLGameLayout* startLayout = nullptr;
	LLGameButton* startButton = nullptr;
};