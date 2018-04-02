#pragma once
#include "..\Game\LLGame.h"

//用来测试游戏功能的类。
class TestLLGame: public LLGame
{
public:
	void InitUserData()override;
};