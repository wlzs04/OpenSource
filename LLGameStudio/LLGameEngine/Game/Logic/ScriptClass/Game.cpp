#include "Game.h"

Game::Game() :Class(L"Game")
{

}

Game * Game::GetInstance()
{
	return new Game();
}

void Game::AddCppFunction(Function * functionPtr)
{
	AddFunctionDefine(functionPtr);
}
