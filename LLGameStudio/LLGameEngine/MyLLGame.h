#pragma once
#include "Game/LLGame.h"
#include "UserProtocol.h"
#include <vector>

using namespace MyLLGameProtocol;

//一个棋子类游戏，仅用来测试功能，因缺少一台电脑用来传输网络协议而暂停。
class MyLLGame :public LLGame
{
public:
	void ProcessProtocol(LLGameServerProtocol* protocol);
	void InitUserData()override;
	void MoveQizi(void* iuiNode, int i);
	void OnPutQizi(void* iuiNode, int i);
	void GetQipanFromServer(int qipan[17][17]);
	void PutQiziFromServer(bool isBlack, int x, int y);
	void RenderQizi(void* iuiNode, int i);
	void OnStartGame(void* iuiNode, int i);
	void StartGameFromServer();
	void OnAddTurnNumber();
	void OnYouWin();
	void OnYouLost();
	void OnRestartGame(void* iuiNode, int i);

	LLGameCanvas* nodeCanvas = nullptr;
	LLGameLayout* startButtonLayout = nullptr;
	LLGameText* nodeTextTurnNumber = nullptr;
	LLGameText* nodeTextNetState = nullptr;
	LLGameButton* nodeStartButton = nullptr;
	LLGameButton* nodeRestartButton = nullptr;
	LLGameLayout* youResultLayout = nullptr;
	LLGameImage* nodeResultImage = nullptr;

	void* blackBrush = nullptr;
	void* whiteBrush = nullptr;

	float qiziRadius = 0;
	int turnNumber = 0;
	int currentQiziPositionX = -1;
	int currentQiziPositionY = -1;
	int qiziArray[17][17] = { 0 };//0：无棋子，1：黑棋，2：白棋
	float lineWidth = 3;
	bool blackTurn = true;
	bool gameStart = false;

	wstring ip = L"10.237.37.106";
	int port = 1234;
};
