#include "TestLLGame.h"

void TestLLGame::InitUserData()
{
	physicsManager = new PhysicsManager();
	physicsWorld = physicsManager->CreatePhysicsWorld();
	
	PhysRectangle* leftRectangle = physicsManager->CreateRectangle(10, gameConfig.height);
	leftRectangle->SetStatic();
	leftRectangle->SetPosition(0, gameConfig.height/2);
	physicsWorld->AddObject(leftRectangle);
	vectorRectangle.push_back(leftRectangle);
	PhysRectangle* topRectangle = physicsManager->CreateRectangle(gameConfig.width, 10);
	topRectangle->SetStatic();
	topRectangle->SetPosition(gameConfig.width / 2, 0);
	physicsWorld->AddObject(topRectangle);
	vectorRectangle.push_back(topRectangle);
	PhysRectangle* rightRectangle = physicsManager->CreateRectangle(10, gameConfig.height);
	rightRectangle->SetStatic();
	rightRectangle->SetPosition(gameConfig.width, gameConfig.height / 2);
	physicsWorld->AddObject(rightRectangle);
	vectorRectangle.push_back(rightRectangle);
	PhysRectangle* bottomRectangle = physicsManager->CreateRectangle(gameConfig.width, 10);
	bottomRectangle->SetStatic();
	bottomRectangle->SetPosition(gameConfig.width / 2, gameConfig.height);
	physicsWorld->AddObject(bottomRectangle);
	vectorRectangle.push_back(bottomRectangle);

	for (int i = 0; i < 5; i++)
	{
		PhysCircle* circle = physicsManager->CreateCircle(i * 10 + 10);
		circle->SetPosition(30 + i * 90, 100);
		circle->SetMass(2*i);
		circle->SetVelocity(MathHelper::GetNormalVector2(Vector2(1 - 2 * (rand() / (float)RAND_MAX), 1 - 2 * (rand() / (float)RAND_MAX))) * 3);
		physicsWorld->AddObject(circle);
		vectorCircle.push_back(circle);

		/*PhysCircle* circle = physicsManager->CreateCircle(30);

		physicsWorld->AddObject(circle);
		vectorCircle.push_back(circle);
		if (i == 0)
		{
			circle->SetMass(1);
			circle->SetPosition(100, 90);
			circle->SetVelocity(1, 0);
		}
		else if (i == 1)
		{
			circle->SetMass(1);
			circle->SetPosition(400, 200);
			circle->SetVelocity(0, -1);
		}*/
	}
	whiteBrush = GraphicsApi::GetGraphicsApi()->CreateColorBrush(1, 1, 1, 1);
	nodeCanvas = (LLGameCanvas*)gameScene->GetNode(L"canvas");
	nodeCanvas->OnRender = bind(&TestLLGame::RenderCanvas, this, placeholders::_1, placeholders::_2);
}

void TestLLGame::UpdateUserData()
{
	physicsWorld->Update(gameTimer.GetThisTickTime());
}

void TestLLGame::RenderCanvas(void* iuiNode, int i)
{
	GraphicsApi::GetGraphicsApi()->SetCurrentBrush(whiteBrush);

	for(auto var : vectorCircle)
	{
		GraphicsApi::GetGraphicsApi()->DrawEllipse(true, var->GetPosition().x, var->GetPosition().y, var->GetRedius(), var->GetRedius());
	}

	GraphicsApi::GetGraphicsApi()->SetCurrentBrush(whiteBrush);
	for (auto var : vectorRectangle)
	{
		Vector2 vp = var->GetPosition();
		GraphicsApi::GetGraphicsApi()->DrawRect(true, vp.x-var->GetWidth()/2, vp.y - var->GetHeight()/2, var->GetWidth(), var->GetHeight());
	}
	GraphicsApi::GetGraphicsApi()->ResetDefaultBrush();
}
