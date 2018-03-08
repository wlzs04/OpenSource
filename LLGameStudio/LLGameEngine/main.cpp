#include "Game/LLGame.h"

int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nShowCmd)
{
	LLGame llGame;
	llGame.Init();
	llGame.Start();

	return 0;
}