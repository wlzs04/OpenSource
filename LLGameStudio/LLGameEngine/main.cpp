#include "Game/LLGame.h"

class MyLLGame:public LLGame
{
public:
	void InitUserData()override
	{
		/*IUINode* nodeButton= gameScene->GetNode(L"showPointLayout\\grid1\\buttonStart");
		nodeButton->OnMouseClick=bind(&MyLLGame::OnStartGame, this, placeholders::_1, placeholders::_2);

		nodeText = (LLGameText*) gameScene->GetNode(L"showPointLayout\\grid1\\textTurnNumber");

		nodeCanvas = (LLGameCanvas* ) gameScene->GetNode(L"showPointLayout\\grid1\\textTurnNumber");
		nodeCanvas->OnMouseMove = bind(&MyLLGame::MoveQizi, this, placeholders::_1, placeholders::_2);
		nodeCanvas->OnMouseClick = bind(&MyLLGame::PutQizi, this, placeholders::_1, placeholders::_2);
		nodeCanvas->OnRender = bind(&MyLLGame::RenderQizi, this, placeholders::_1, placeholders::_2);*/
		
	}

	void MoveQizi(void* iuiNode, int i)
	{
		OnShowTurnNumber();
	}

	void PutQizi(void* iuiNode, int i)
	{

	}

	void RenderQizi(void* iuiNode, int i)
	{

	}

	void OnStartGame(void* iuiNode, int i)
	{
		OnShowTurnNumber();
	}

	void OnShowTurnNumber()
	{
		turnNumber++;
		nodeText->SetText(L"第"+ to_wstring(turnNumber)+L"回合");
	}

	LLGameCanvas* nodeCanvas = nullptr;
	LLGameText* nodeText = nullptr;
	int turnNumber = 0;
};

int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nShowCmd)
{
	MyLLGame myGame;
	myGame.Init();
	myGame.Start();

	return 0;
}