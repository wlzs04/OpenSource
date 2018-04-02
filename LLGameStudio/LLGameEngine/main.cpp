#include "TestGame\TestLLGame.h"

int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nShowCmd)
{
	TestLLGame myGame;
	myGame.Init();
	myGame.Start();
	return 0;
}