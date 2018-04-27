//#include "TestGame\TestLLGame.h"
#include "TestGame\TableHockeyGame.h"


int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nShowCmd)
{
	//TestLLGame myGame;
	TableHockeyGame myGame;
	myGame.Init();
	myGame.Start();
	return 0;
}