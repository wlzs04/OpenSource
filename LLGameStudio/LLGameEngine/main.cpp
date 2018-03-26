#include "MyLLGame.h"

int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nShowCmd)
{
	MyLLGame myGame;
	myGame.Init();
	myGame.Start();
	return 0;
}