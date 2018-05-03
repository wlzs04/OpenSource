#include "TableHockeyGame.h"

void TableHockeyGame::InitUserData()
{
	InitLayout();
	InitObject();
	InitConnectNet();
	
	physicsWorld->OnCollisionEvent = bind(&TableHockeyGame::CollisionEvent, this, placeholders::_1, placeholders::_2);
	gameWindow->OnKeyDown = bind(&TableHockeyGame::KeyDownEvent, this, placeholders::_1, placeholders::_2);
	
	handBallConstraintPoint.x = gameConfig.width.value / 2;
	handBallConstraintPoint.y = gameConfig.height.value -holeWidth/2;

	PhysicsConstraint* myHandBallConstraint = new PhysicsConstraint(myHandBallPhys);
	myHandBallConstraint->SetPointConstrain(handBallConstraintPoint, handBallMaxLength);
	physicsWorld->AddConstraint(myHandBallConstraint);
}

void TableHockeyGame::InitLayout()
{
	//结果界面
	youResultLayout = new LLGameLayout();
	youResultLayout->SetProperty(L"name", L"resultLayout");
	youResultLayout->SetProperty(L"modal", L"True");
	youResultLayout->LoadLayoutFromFile(L"layout\\youResultLayout.layout");
	nodeResultImage = (LLGameImage*)youResultLayout->GetNode(L"grid1\\resultImage");	
	nodeRestartButton = (LLGameButton*)youResultLayout->GetNode(L"grid1\\buttonRestart");
	nodeRestartButton->OnMouseClick = bind(&TableHockeyGame::OnRestartGame, this, placeholders::_1, placeholders::_2);

	//开始界面
	startLayout = (LLGameLayout*)gameScene->GetNode(L"startButtonLayout");
	startButton = (LLGameButton*)startLayout->GetNode(L"grid1\\buttonStart");
	startButton->OnMouseClick = bind(&TableHockeyGame::OnStartGame, this, placeholders::_1, placeholders::_2);

	//计分界面
	gameRecordLayout = (LLGameLayout*)gameScene->GetNode(L"gameRecordLayout");
	textMyRecord = (LLGameText*)gameRecordLayout->GetNode(L"grid1\\textMyRecord");
	textMyRecord->SetText(L"我的得分：" + to_wstring(myRecord));
	textOpponentRecord = (LLGameText*)gameRecordLayout->GetNode(L"grid1\\textOpponentRecord");
	textOpponentRecord->SetText(L"对手得分：" + to_wstring(opponentRecord));

	//倒计时界面
	gameCountDownLayout = new LLGameLayout();
	gameCountDownLayout->SetProperty(L"name", L"resultLayout");
	gameCountDownLayout->LoadLayoutFromFile(L"layout\\gameCountDownLayout.layout");
	textCountDown = (LLGameText*)gameCountDownLayout->GetNode(L"grid1\\textCountDown");

	//画布界面
	nodeCanvas = (LLGameCanvas*)gameScene->GetNode(L"canvas");
	nodeCanvas->OnRender = bind(&TableHockeyGame::RenderCanvas, this, placeholders::_1, placeholders::_2);

	particleSystem = new ParticleSystem();
	particleSystem->LoadParticleFromFile(L"particle\\particle1.particle");
	particleSystem->ResetTransform();
	particleSystem->StartPlay();
	particleSystem->SetEnable(false);

	actor = new Actor();
	actor->LoadActorFromFile(L"actor\\actor2.actor");
	actor->SetPosition(gameConfig.width.value / 2, gameConfig.height.value);
	myHandbone = actor->GetBoneByName(actor->rootBone,L"上骨骼");
	//actor->SetCurrentAction(L"测试");
	//actor->Start();
}

void TableHockeyGame::InitObject()
{
	//当physicsManager为NULL时不会报错，因为CreatePhysicsWorld方法没有用到physicsManager本身的内容。
	physicsWorld = physicsManager->CreatePhysicsWorld();

	//添加边缘碰撞体
	PhysRectangle* leftRectangle = physicsManager->CreateRectangle(blockWidth, gameConfig.height.value);
	leftRectangle->SetStatic();
	leftRectangle->SetPosition(0, gameConfig.height.value / 2);
	physicsWorld->AddObject(leftRectangle);
	vectorRectangle.push_back(leftRectangle);
	PhysRectangle* topRectangle = physicsManager->CreateRectangle(gameConfig.width.value, blockWidth);
	topRectangle->SetStatic();
	topRectangle->SetPosition(gameConfig.width.value / 2, 0);
	physicsWorld->AddObject(topRectangle);
	vectorRectangle.push_back(topRectangle);
	PhysRectangle* rightRectangle = physicsManager->CreateRectangle(blockWidth, gameConfig.height.value);
	rightRectangle->SetStatic();
	rightRectangle->SetPosition(gameConfig.width.value, gameConfig.height.value / 2);
	physicsWorld->AddObject(rightRectangle);
	vectorRectangle.push_back(rightRectangle);
	PhysRectangle* bottomRectangle = physicsManager->CreateRectangle(gameConfig.width.value, blockWidth);
	bottomRectangle->SetStatic();
	bottomRectangle->SetPosition(gameConfig.width.value / 2, gameConfig.height.value);
	physicsWorld->AddObject(bottomRectangle);
	vectorRectangle.push_back(bottomRectangle);

	//添加冰球
	iceBallPhys = physicsManager->CreateCircle(ballRadius);
	iceBallPhys->SetPosition(gameConfig.width.value / 2, gameConfig.height.value / 2);
	physicsWorld->AddObject(iceBallPhys);

	//添加我方内容
	myHandBallPhys = physicsManager->CreateCircle(ballRadius);
	myHandBallPhys->SetPosition(gameConfig.width.value / 2, gameConfig.height.value*0.75);
	myHandBallPhys->SetActive();
	physicsWorld->AddObject(myHandBallPhys);
	vectorCircle.push_back(myHandBallPhys);

	myHolePhys = physicsManager->CreateRectangle(gameConfig.width.value*0.5, holeWidth);
	myHolePhys->SetPosition(gameConfig.width.value / 2, gameConfig.height.value);
	myHolePhys->SetStatic();
	physicsWorld->AddObject(myHolePhys);

	//添加对方内容
	opponentHandBallPhys = physicsManager->CreateCircle(ballRadius);
	opponentHandBallPhys->SetPosition(gameConfig.width.value / 2, gameConfig.height.value*0.25);
	opponentHandBallPhys->SetActive();
	physicsWorld->AddObject(opponentHandBallPhys);
	vectorCircle.push_back(opponentHandBallPhys);

	opponentHolePhys = physicsManager->CreateRectangle(gameConfig.width.value*0.5, holeWidth);
	opponentHolePhys->SetPosition(gameConfig.width.value / 2, 0);
	opponentHolePhys->SetStatic();
	physicsWorld->AddObject(opponentHolePhys);

	//添加画刷
	iceBallBrush = GraphicsApi::GetGraphicsApi()->CreateColorBrush(0.9, 0.8, 0.8, 1);
	handBallBrush = GraphicsApi::GetGraphicsApi()->CreateColorBrush(0.7, 0.4, 0.2, 1);
	blockBrush = GraphicsApi::GetGraphicsApi()->CreateColorBrush(0.8, 0.6, 0.6, 1);
	holeBrush = GraphicsApi::GetGraphicsApi()->CreateColorBrush(0.9, 0.1, 0.1, 1);
}

void TableHockeyGame::InitConnectNet()
{
	gameNetClient->OnConnectSuccessHandle = [&]() {
		MessageHelper::ShowMessage(L"连接服务器成功！"); };
	gameNetClient->OnConnectFailHandle = [&]() {
		MessageHelper::ShowMessage(L"连接服务器失败！"); };
	gameNetClient->OnDisconnectHandle = [&]() {
		MessageHelper::ShowMessage(L"与服务器断开连接！"); };
	gameNetClient->OnProcessProtocolHandle = [&](LLGameProtocol protocol) {ProcessProtocol(protocol); };

	gameNetClient->StartConnect(ip, port);
}

void TableHockeyGame::ProcessProtocol(LLGameProtocol protocol)
{
	protocol.Process(this);
}

void TableHockeyGame::KeyDownEvent(void * sender, int key)
{
	if (key == VK_ESCAPE)
	{
		gameExit = true;
		SendMessage(gameWindow->GetHWND(), WM_DESTROY, 0, 0);
	}
	if (key == VK_SPACE)
	{
		OnRestartGame(NULL,0);
	}
	//(GetAsyncKeyState(key) & 0x8000);
}

void TableHockeyGame::CollisionEvent(IPhysObject * object1, IPhysObject * object2)
{
	if (object1 == iceBallPhys)
	{
		if (object2 == myHolePhys)
		{
			OnLost();
		}
		else if (object2 == opponentHolePhys)
		{
			OnWin();
		}
	}
}

void TableHockeyGame::UpdateUserData()
{
	particleSystem->Update();

	//actor->Update();

	//判断是否开始游戏
	/*if (!gameStart)
	{
		return;
	}*/

	float tickTime = gameTimer.GetThisTickTime();

	//倒计时
	if (isCountDownState)
	{
		realCountDown -= tickTime;
		textCountDown->SetText(to_wstring((int)realCountDown));
		if (realCountDown < 0)
		{
			gameScene->RemoveNode(gameCountDownLayout);
			isCountDownState = false;
			ServeBall();
		}
		//lastMousePosition = GameHelper::mousePosition;
		//return;
	}

	POINT currentMousePosition = GameHelper::mousePosition;
	Vector2 currentMouseVector = Vector2(currentMousePosition.x, currentMousePosition.y);
	
	Vector2 handBallVelocity;
	handBallVelocity.x = (currentMouseVector.x - lastMousePosition.x) / tickTime;
	handBallVelocity.y = (currentMouseVector.y - lastMousePosition.y) / tickTime;
	
	myHandBallPhys->SetPosition(currentMouseVector);
	myHandBallPhys->SetVelocity(handBallVelocity);
	lastMousePosition = GameHelper::mousePosition;

	actor->SetBoneTrandformByIK(myHandbone, currentMouseVector);

	actor->Update();

	physicsWorld->Update(tickTime);
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

	particleSystem->Render();
	actor->Render();

	GraphicsApi::GetGraphicsApi()->ResetDefaultBrush();
}

void TableHockeyGame::OnWin()
{
	nodeResultImage->SetImage(L"texture\\youWin.jpg");
	gameScene->AddNode(youResultLayout);
	youResultLayout->ResetTransform();
	gameStart = false;
	physicsWorld->Stop();
	myRecord++;
	textMyRecord->SetText(L"我的得分：" + to_wstring(myRecord));
	particleSystem->SetEnable(true);
}

void TableHockeyGame::OnLost()
{
	nodeResultImage->SetImage(L"texture\\youLost.jpg");
	gameScene->AddNode(youResultLayout);
	youResultLayout->ResetTransform();
	gameStart = false;
	physicsWorld->Stop();
	opponentRecord++;
	textOpponentRecord->SetText(L"对手得分：" + to_wstring(opponentRecord));
}

void TableHockeyGame::OnStartGame(void * sender, int e)
{
	gameScene->RemoveNode(startLayout);
	gameStart = true;
	physicsWorld->Start();
	isCountDownState = true;
	gameScene->AddNode(gameCountDownLayout);
	gameCountDownLayout->ResetTransform();
	realCountDown = serveCountDown;
	textCountDown->SetText(to_wstring(realCountDown));
}

void TableHockeyGame::OnRestartGame(void * sender, int e)
{
	gameScene->RemoveNode(youResultLayout);
	gameStart = true;
	physicsWorld->Start();
	iceBallPhys->SetPosition(gameConfig.width.value / 2, gameConfig.height.value / 2);
	iceBallPhys->SetVelocity(0, 0);
	myHandBallPhys->SetPosition(gameConfig.width.value / 2, gameConfig.height.value*0.75);
	myHandBallPhys->SetVelocity(0, 0);
	opponentHandBallPhys->SetPosition(gameConfig.width.value / 2, gameConfig.height.value*0.25);
	opponentHandBallPhys->SetVelocity(0, 0);
	isCountDownState = true;
	gameScene->AddNode(gameCountDownLayout);
	gameCountDownLayout->ResetTransform();
	realCountDown = serveCountDown;
	textCountDown->SetText(to_wstring(realCountDown));
	particleSystem->SetEnable(false);
}

void TableHockeyGame::ServeBall()
{
	iceBallPhys->SetVelocity(MathHelper::GetNormalVector2(Vector2(1 - 2 * (rand() / (float)RAND_MAX), 1 - 2 * (rand() / (float)RAND_MAX))) * 300);
}
