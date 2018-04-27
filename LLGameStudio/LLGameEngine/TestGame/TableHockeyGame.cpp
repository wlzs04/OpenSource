#include "TableHockeyGame.h"

void TableHockeyGame::InitUserData()
{
	youResultLayout = new LLGameLayout();
	youResultLayout->SetProperty(L"name", L"resultLayout");
	youResultLayout->SetProperty(L"modal", L"True");
	youResultLayout->LoadLayoutFromFile(L"layout\\youResultLayout.layout");
	nodeResultImage = (LLGameImage*)youResultLayout->GetNode(L"grid1\\resultImage");
	nodeRestartButton = (LLGameButton*)youResultLayout->GetNode(L"grid1\\buttonRestart");
	nodeRestartButton->OnMouseClick = bind(&TableHockeyGame::OnRestartGame, this, placeholders::_1, placeholders::_2);

	startLayout = (LLGameLayout*)gameScene->GetNode(L"startButtonLayout");
	startButton= (LLGameButton*)startLayout->GetNode(L"grid1\\buttonStart");
	startButton->OnMouseClick = bind(&TableHockeyGame::OnStartGame, this, placeholders::_1, placeholders::_2);
	physicsManager = new PhysicsManager();
	physicsWorld = physicsManager->CreatePhysicsWorld();

	//添加边缘碰撞体
	PhysRectangle* leftRectangle = physicsManager->CreateRectangle(40, gameConfig.height);
	leftRectangle->SetStatic();
	leftRectangle->SetPosition(0, gameConfig.height / 2);
	physicsWorld->AddObject(leftRectangle);
	vectorRectangle.push_back(leftRectangle);
	PhysRectangle* topRectangle = physicsManager->CreateRectangle(gameConfig.width, 40);
	topRectangle->SetStatic();
	topRectangle->SetPosition(gameConfig.width / 2, 0);
	physicsWorld->AddObject(topRectangle);
	vectorRectangle.push_back(topRectangle);
	PhysRectangle* rightRectangle = physicsManager->CreateRectangle(40, gameConfig.height);
	rightRectangle->SetStatic();
	rightRectangle->SetPosition(gameConfig.width, gameConfig.height / 2);
	physicsWorld->AddObject(rightRectangle);
	vectorRectangle.push_back(rightRectangle);
	PhysRectangle* bottomRectangle = physicsManager->CreateRectangle(gameConfig.width, 40);
	bottomRectangle->SetStatic();
	bottomRectangle->SetPosition(gameConfig.width / 2, gameConfig.height);
	physicsWorld->AddObject(bottomRectangle);
	vectorRectangle.push_back(bottomRectangle);

	//添加冰球

	iceBallPhys = physicsManager->CreateCircle(40);
	iceBallPhys->SetPosition(gameConfig.width / 2, gameConfig.height / 2);

	physicsWorld->AddObject(iceBallPhys);

	//添加我方内容

	myHandBallPhys = physicsManager->CreateCircle(40);
	myHandBallPhys->SetPosition(gameConfig.width / 2, gameConfig.height*0.75);
	physicsWorld->AddObject(myHandBallPhys);
	vectorCircle.push_back(myHandBallPhys);

	myHolePhys = physicsManager->CreateRectangle(gameConfig.width*0.5, 80);
	myHolePhys->SetPosition(gameConfig.width / 2, gameConfig.height);
	myHolePhys->SetStatic();
	physicsWorld->AddObject(myHolePhys);

	//添加对方内容

	opponentHandBallPhys = physicsManager->CreateCircle(40);
	opponentHandBallPhys->SetPosition(gameConfig.width / 2, gameConfig.height*0.25);
	physicsWorld->AddObject(opponentHandBallPhys);
	vectorCircle.push_back(opponentHandBallPhys);

	opponentHolePhys = physicsManager->CreateRectangle(gameConfig.width*0.5, 80);
	opponentHolePhys->SetPosition(gameConfig.width / 2, 0);
	opponentHolePhys->SetStatic();
	physicsWorld->AddObject(opponentHolePhys);

	iceBallBrush = GraphicsApi::GetGraphicsApi()->CreateColorBrush(0.9, 0.8, 0.8, 1);
	handBallBrush = GraphicsApi::GetGraphicsApi()->CreateColorBrush(0.7, 0.4, 0.2, 1);
	blockBrush = GraphicsApi::GetGraphicsApi()->CreateColorBrush(0.8, 0.6, 0.6, 1);
	holeBrush = GraphicsApi::GetGraphicsApi()->CreateColorBrush(0.9, 0.1, 0.1, 1);

	nodeCanvas = (LLGameCanvas*)gameScene->GetNode(L"canvas");
	nodeCanvas->OnRender = bind(&TableHockeyGame::RenderCanvas, this, placeholders::_1, placeholders::_2);

	physicsWorld->OnCollisionEvent = bind(&TableHockeyGame::CollisionEvent, this, placeholders::_1, placeholders::_2);
	gameWindow->OnKeyDown = bind(&TableHockeyGame::KeyDownEvent, this, placeholders::_1, placeholders::_2);
}

void TableHockeyGame::KeyDownEvent(void * sender, int key)
{
	if (key == VK_ESCAPE)
	{
		gameExit = true;
		SendMessage(gameWindow->GetHWND(), WM_DESTROY, 0, 0);
	}
	//(GetAsyncKeyState(key) & 0x8000);
}

void TableHockeyGame::CollisionEvent(IPhysObject * object1, IPhysObject * object2)
{
	int y = 0;
	if (object1 == iceBallPhys)
	{
		if (object2 == myHolePhys)
		{
			nodeResultImage->SetImage(L"texture\\youLost.jpg");
			gameScene->AddNode(youResultLayout);
			youResultLayout->ResetTransform();
			gameStart = false;
			physicsWorld->Stop();
		}
		else if (object2 == opponentHolePhys)
		{
			nodeResultImage->SetImage(L"texture\\youWin.jpg");
			gameScene->AddNode(youResultLayout);
			youResultLayout->ResetTransform();
			gameStart = false;
			physicsWorld->Stop();
		}
	}
}

void TableHockeyGame::UpdateUserData()
{
	POINT currentMousePosition = GameHelper::mousePosition;

	float tickTime = gameTimer.GetThisTickTime();
	Vector2 handBallVelocity;
	handBallVelocity.x = (currentMousePosition.x - lastMousePosition.x) / tickTime;
	handBallVelocity.y = (currentMousePosition.y - lastMousePosition.y) / tickTime;

	myHandBallPhys->SetPosition(lastMousePosition.x, lastMousePosition.y);
	myHandBallPhys->SetVelocity(handBallVelocity);
	lastMousePosition = GameHelper::mousePosition;

	physicsWorld->Update(gameTimer.GetThisTickTime());
}

void TableHockeyGame::RenderCanvas(void * iuiNode, int i)
{
	//绘制冰球
	{
		GraphicsApi::GetGraphicsApi()->SetCurrentBrush(iceBallBrush);
		GraphicsApi::GetGraphicsApi()->DrawEllipse(true, iceBallPhys->GetPosition().x, iceBallPhys->GetPosition().y, iceBallPhys->GetRedius(), iceBallPhys->GetRedius());
	}

	//绘制手球
	{
		GraphicsApi::GetGraphicsApi()->SetCurrentBrush(handBallBrush);
		for (auto var : vectorCircle)
		{
			Vector2 vp = var->GetPosition();
			GraphicsApi::GetGraphicsApi()->DrawEllipse(true, var->GetPosition().x, var->GetPosition().y, var->GetRedius(), var->GetRedius());
		}
	}

	//绘制洞口
	{
		GraphicsApi::GetGraphicsApi()->SetCurrentBrush(holeBrush);
		GraphicsApi::GetGraphicsApi()->DrawRect(true, myHolePhys->GetPosition().x - myHolePhys->GetWidth() / 2, myHolePhys->GetPosition().y - myHolePhys->GetHeight() / 2, myHolePhys->GetWidth(), myHolePhys->GetHeight());
		GraphicsApi::GetGraphicsApi()->DrawRect(true, opponentHolePhys->GetPosition().x - opponentHolePhys->GetWidth() / 2, opponentHolePhys->GetPosition().y - opponentHolePhys->GetHeight() / 2, opponentHolePhys->GetWidth(), opponentHolePhys->GetHeight());
	}

	//绘制边缘
	{
		GraphicsApi::GetGraphicsApi()->SetCurrentBrush(blockBrush);

		for (auto var : vectorRectangle)
		{
			Vector2 vp = var->GetPosition();
			GraphicsApi::GetGraphicsApi()->DrawRect(true, vp.x - var->GetWidth() / 2, vp.y - var->GetHeight() / 2, var->GetWidth(), var->GetHeight());
		}
	}

	GraphicsApi::GetGraphicsApi()->ResetDefaultBrush();
}

void TableHockeyGame::OnStartGame(void * sender, int e)
{
	gameScene->RemoveNode(startLayout);
	gameStart = true;
	physicsWorld->Start();
}

void TableHockeyGame::OnRestartGame(void * sender, int e)
{
	gameScene->RemoveNode(youResultLayout);
	gameStart = true;
	physicsWorld->Start();
	iceBallPhys->SetPosition(gameConfig.width / 2, gameConfig.height / 2);
	iceBallPhys->SetVelocity(0, 0);
	myHandBallPhys->SetPosition(gameConfig.width / 2, gameConfig.height*0.75);
	myHandBallPhys->SetVelocity(0, 0);
	opponentHandBallPhys->SetPosition(gameConfig.width / 2, gameConfig.height*0.25);
	opponentHandBallPhys->SetVelocity(0, 0);
}
