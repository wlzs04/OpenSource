﻿//#include "TestGame\TestLLGame.h"
#include "TestGame\LogicGame\LogicGame.h"

int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nShowCmd)
{
	//TestLLGame myGame;
	//TableHockeyGame myGame;
	LogicGame myGame;
	myGame.Init();
	myGame.Start();
	return 0;
}