#include "Game/LLGame.h"

class MyLLGame:public LLGame
{
public:

	
};

int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nShowCmd)
{
	MyLLGame myGame;
	myGame.Init();
	myGame.Start();

	return 0;
}