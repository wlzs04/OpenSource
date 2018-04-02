#include "UserProtocol.h"
#include "MyLLGame.h"

void SStartGameProtocol::Process(void * ptr)
{
	((MyLLGame*)ptr)->StartGameFromServer();
}

CPutQiziProtocol::CPutQiziProtocol(bool isBlack, int x, int y)
{
	this->isBlack = isBlack;
	this->x = x;
	this->y = y;
}

void SGetQipanProtocol::Process(void * ptr)
{
	int qipan[17][17] = { 0 };
	wstring c = content;
	((MyLLGame*)ptr)->GetQipanFromServer(qipan);
}

SPutQiziProtocol::SPutQiziProtocol(bool isBlack, int x, int y)
{
	this->isBlack = isBlack;
	this->x = x;
	this->y = y;
}

void SPutQiziProtocol::Process(void* ptr)
{
	((MyLLGame*)ptr)->PutQiziFromServer(isBlack,x,y);
}
