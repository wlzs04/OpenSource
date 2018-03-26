#include "UserProtocol.h"
#include "MyLLGame.h"

void SStartGameProtocol::Process(void * ptr)
{
	((MyLLGame*)ptr)->StartGameFromServer();
}