#include "MyLLGame.h"

void MyLLGame::ProcessProtocol(LLGameServerProtocol* protocol)
{
	protocol->Process(this);
}

void MyLLGame::InitUserData()
{
	nodeStartButton = (LLGameButton*)gameScene->GetNode(L"startButtonLayout\\grid1\\buttonStart");
	nodeStartButton->OnMouseClick = bind(&MyLLGame::OnStartGame, this, placeholders::_1, placeholders::_2);
	startButtonLayout = (LLGameLayout*)gameScene->GetNode(L"startButtonLayout");
	nodeTextTurnNumber = (LLGameText*)gameScene->GetNode(L"showTurnNumberLayout\\grid1\\textTurnNumber");
	nodeTextNetState = (LLGameText*)gameScene->GetNode(L"showTurnNumberLayout\\grid1\\textNetState");

	nodeTextNetState->SetText(L"开始连接！");

	nodeCanvas = (LLGameCanvas*)gameScene->GetNode(L"canvas");
	nodeCanvas->OnMouseMove = bind(&MyLLGame::MoveQizi, this, placeholders::_1, placeholders::_2);
	nodeCanvas->OnMouseClick = bind(&MyLLGame::OnPutQizi, this, placeholders::_1, placeholders::_2);
	nodeCanvas->OnRender = bind(&MyLLGame::RenderQizi, this, placeholders::_1, placeholders::_2);

	youResultLayout = new LLGameLayout();
	youResultLayout->SetProperty(L"name", L"resultLayout");
	youResultLayout->SetProperty(L"modal", L"True");
	youResultLayout->LoadLayoutFromFile(L"layout\\youResultLayout.layout");
	nodeResultImage = (LLGameImage*)youResultLayout->GetNode(L"grid1\\resultImage");
	nodeRestartButton = (LLGameButton*)youResultLayout->GetNode(L"grid1\\buttonRestart");
	nodeRestartButton->OnMouseClick = bind(&MyLLGame::OnRestartGame, this, placeholders::_1, placeholders::_2);

	qiziRadius = GameHelper::width / 18 / 2;

	blackBrush = GraphicsApi::GetGraphicsApi()->CreateColorBrush(0, 0, 0, 1);
	whiteBrush = GraphicsApi::GetGraphicsApi()->CreateColorBrush(1, 1, 1, 1);

	gameNetClient->OnConnectSuccessHandle = [&]() {nodeTextNetState->SetText(L"连接成功！"); };
	gameNetClient->OnConnectFailHandle = [&]() {nodeTextNetState->SetText(L"连接失败！"); };
	gameNetClient->OnDisconnectHandle = [&]() {nodeTextNetState->SetText(L"连接断开！"); };
	gameNetClient->OnProcessProtocolHandle = [this](LLGameServerProtocol* protocol) {ProcessProtocol(protocol); };

	gameNetClient->StartConnect(ip, port);
}

void MyLLGame::MoveQizi(void * iuiNode, int i)
{
	if (gameStart)
	{
		currentQiziPositionX = MathHelper::RoundFloat((GameHelper::mousePosition.x - 2 * qiziRadius) / qiziRadius / 2);
		currentQiziPositionY = MathHelper::RoundFloat((GameHelper::mousePosition.y - 2 * qiziRadius) / qiziRadius / 2);
		currentQiziPositionX = currentQiziPositionX > 16 ? 16 : currentQiziPositionX;
		currentQiziPositionY = currentQiziPositionY > 16 ? 16 : currentQiziPositionY;
	}
}

void MyLLGame::OnPutQizi(void* iuiNode, int i)
{
	if (gameStart)
	{
		CPutQiziProtocol cpp;
		cpp.AddContent(L"blackTurn", to_wstring(blackTurn));
		cpp.AddContent(L"currentQiziPositionX", to_wstring(currentQiziPositionX));
		cpp.AddContent(L"currentQiziPositionY", to_wstring(currentQiziPositionY));
		gameNetClient->SendProtocol(cpp);

		nodeTextNetState->SetText(L"等待服务器协议！");
	}
}

void MyLLGame :: GetQipanFromServer(int qipan[17][17])
{
	if (gameStart)
	{

	}
}

void MyLLGame::PutQiziFromServer(bool isBlack, int x, int y)
{
	if (gameStart)
	{
		if (qiziArray[x][y] == 0)
		{
			if (turnNumber >= 3)
			{
				OnYouLost();
				turnNumber = 0;
				return;
			}
			OnAddTurnNumber();
			qiziArray[x][y] = isBlack ? 1 : 2;
			blackTurn = !blackTurn;
		}
	}
}

void MyLLGame::RenderQizi(void * iuiNode, int i)
{
	if (gameStart)
	{
		float posX = 0;
		for (int i = 0; i < 17; i++)
		{
			posX += qiziRadius * 2;
			float posY = 0;
			for (int j = 0; j < 17; j++)
			{
				posY += qiziRadius * 2;
				if (qiziArray[i][j] == 0)
				{
					continue;
				}
				if (qiziArray[i][j] == 1)
				{
					GraphicsApi::GetGraphicsApi()->SetCurrentBrush(blackBrush);
				}
				else if (qiziArray[i][j] == 2)
				{
					GraphicsApi::GetGraphicsApi()->SetCurrentBrush(whiteBrush);
				}
				GraphicsApi::GetGraphicsApi()->DrawEllipse(true, posX, posY, qiziRadius, qiziRadius);
			}
		}

		blackTurn ? GraphicsApi::GetGraphicsApi()->SetCurrentBrush(blackBrush) : GraphicsApi::GetGraphicsApi()->SetCurrentBrush(whiteBrush);

		float centerX = (currentQiziPositionX + 1)*qiziRadius * 2;
		float centerY = (currentQiziPositionY + 1)*qiziRadius * 2;

		//左上
		GraphicsApi::GetGraphicsApi()->DrawLine(centerX - qiziRadius, centerY - qiziRadius / 2, centerX - qiziRadius / 2, centerY - qiziRadius / 2, lineWidth);
		GraphicsApi::GetGraphicsApi()->DrawLine(centerX - qiziRadius / 2, centerY - qiziRadius, centerX - qiziRadius / 2, centerY - qiziRadius / 2, lineWidth);
		//右上
		GraphicsApi::GetGraphicsApi()->DrawLine(centerX + qiziRadius, centerY - qiziRadius / 2, centerX + qiziRadius / 2, centerY - qiziRadius / 2, lineWidth);
		GraphicsApi::GetGraphicsApi()->DrawLine(centerX + qiziRadius / 2, centerY - qiziRadius, centerX + qiziRadius / 2, centerY - qiziRadius / 2, lineWidth);
		//左下
		GraphicsApi::GetGraphicsApi()->DrawLine(centerX - qiziRadius, centerY + qiziRadius / 2, centerX - qiziRadius / 2, centerY + qiziRadius / 2, lineWidth);
		GraphicsApi::GetGraphicsApi()->DrawLine(centerX - qiziRadius / 2, centerY + qiziRadius, centerX - qiziRadius / 2, centerY + qiziRadius / 2, lineWidth);
		//右下
		GraphicsApi::GetGraphicsApi()->DrawLine(centerX + qiziRadius, centerY + qiziRadius / 2, centerX + qiziRadius / 2, centerY + qiziRadius / 2, lineWidth);
		GraphicsApi::GetGraphicsApi()->DrawLine(centerX + qiziRadius / 2, centerY + qiziRadius, centerX + qiziRadius / 2, centerY + qiziRadius / 2, lineWidth);

		GraphicsApi::GetGraphicsApi()->ResetDefaultBrush();
	}
}

void MyLLGame::OnStartGame(void * iuiNode, int i)
{
	CStartGameProtocol csp;
	gameNetClient->SendProtocol(csp);
	nodeTextNetState->SetText(L"等待服务器协议！");
}

void MyLLGame::StartGameFromServer()
{
	gameStart = true;
	gameScene->RemoveNode(startButtonLayout);
	OnAddTurnNumber();
	nodeTextNetState->SetText(L"游戏开始！");

}

void MyLLGame::OnAddTurnNumber()
{
	if (blackTurn)
	{
		turnNumber++;
		nodeTextTurnNumber->SetText(L"第" + to_wstring(turnNumber) + L"回合");
	}
}

void MyLLGame::OnYouWin()
{
	nodeResultImage->SetImage(L"texture\\youWin.jpg");
	gameScene->AddNode(youResultLayout);
	youResultLayout->ResetTransform();
}

void MyLLGame::OnYouLost()
{
	nodeResultImage->SetImage(L"texture\\youLost.jpg");
	gameScene->AddNode(youResultLayout);
	youResultLayout->ResetTransform();
}

void MyLLGame::OnRestartGame(void * iuiNode, int i)
{
	gameScene->RemoveNode(youResultLayout);
}
