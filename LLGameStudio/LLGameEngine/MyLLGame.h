#pragma once
#include "Game/LLGame.h"
#include "UserProtocol.h"

class MyLLGame :public LLGame
{
public:
	void ProcessProtocol(LLGameProtocol protocol);
	void InitUserData()override;
	void MoveQizi(void* iuiNode, int i);
	void PutQizi(void* iuiNode, int i);
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
