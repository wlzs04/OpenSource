#include "UserProtocol.h"
#include "MyLLGame.h"

void SStartGameProtocol::Process(LLGame* ptr)
{
	((MyLLGame*)ptr)->StartGameFromServer();
}

void SGetQipanProtocol::Process(LLGame * ptr)
{
	/*int qipan[17][17] = { 0 };
	wstring c = content;
	((MyLLGame*)ptr)->GetQipanFromServer(qipan);*/
}

void SPutQiziProtocol::Process(LLGame* ptr)
{
	((MyLLGame*)ptr)->PutQiziFromServer(isBlack,x,y);
}
