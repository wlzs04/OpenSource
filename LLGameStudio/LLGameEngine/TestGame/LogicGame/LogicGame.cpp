#include "LogicGame.h"
#include "..\..\Game\Logic\ScriptClass\Game.h"
#include "..\..\Game\Logic\ScriptClass\Float.h"

void LogicGame::InitUserData()
{
	//当physicsManager为NULL时不会报错，因为CreatePhysicsWorld方法没有用到physicsManager本身的内容。
	physicsWorld = physicsManager->CreatePhysicsWorld();

	scriptManager = LLScriptManager::GetSingleInstance();
	InitGameFunction();
	scriptManager->LoadScriptFromFile(gameConfig.defaultScript.value);
	scriptManager->RunFunction(L"InitUserData");
}

void LogicGame::UpdateUserData()
{
	scriptManager->RunFunction(L"UpdateUserData");
}

void LogicGame::InitGameFunction()
{
	Parameter* gameParameter = scriptManager->GetGlobalParameter(L"game");
	gamePtr = (Game*)gameParameter->GetClassPtr();

	AddGameCppFunction(L"AddLayout", L"void", &LogicGame::AddLayout, this);
	AddGameCppFunction(L"RemoveLayout", L"void", &LogicGame::RemoveLayout, this);
	AddGameCppFunction(L"SetUINodeProperty", L"void", &LogicGame::SetUINodeProperty, this);
	AddGameCppFunction(L"BindButtonClickEvent", L"void", &LogicGame::BindButtonClickEvent, this);
	AddGameCppFunction(L"BindCanvasRenderEvent", L"void", &LogicGame::BindCanvasRenderEvent, this);
	
	AddGameCppFunction(L"AddParticle", L"void", &LogicGame::AddParticle, this);
	AddGameCppFunction(L"RemoveParticle", L"void", &LogicGame::RemoveParticle, this);
	
	AddGameCppFunction(L"LoadActor", L"void", &LogicGame::LoadActor, this);
	AddGameCppFunction(L"RemoveActor", L"void", &LogicGame::RemoveActor, this);
	AddGameCppFunction(L"SetActorPosition", L"void", &LogicGame::SetActorPosition, this);
	
	AddGameCppFunction(L"CreatePhysicsWorld", L"void", &LogicGame::CreatePhysicsWorld, this);
	AddGameCppFunction(L"CreatePhysRectangle", L"void", &LogicGame::CreatePhysRectangle, this);
	AddGameCppFunction(L"CreatePhysCircle", L"void", &LogicGame::CreatePhysCircle, this);
	AddGameCppFunction(L"SetPhysPosition", L"void", &LogicGame::SetPhysPosition, this);
	AddGameCppFunction(L"BindCollisionEvent", L"void", &LogicGame::BindCollisionEvent, this);
	
	AddGameCppFunction(L"BindConnectServerSuccessEvent", L"void", &LogicGame::BindConnectServerSuccessEvent, this);
	AddGameCppFunction(L"BindConnectServerFailEvent", L"void", &LogicGame::BindConnectServerFailEvent, this);
	AddGameCppFunction(L"StartConnectServer", L"void", &LogicGame::StartConnectServer, this);
	
	AddGameCppFunction(L"BindKeyDownEvent", L"void", &LogicGame::BindKeyDownEvent, this);
	AddGameCppFunction(L"GetMouseX", L"float", &LogicGame::GetMouseX, this);
	AddGameCppFunction(L"GetMouseY", L"float", &LogicGame::GetMouseY, this);
	AddGameCppFunction(L"GetGameWidth", L"float", &LogicGame::GetGameWidth, this);
	AddGameCppFunction(L"GetGameHeight", L"float", &LogicGame::GetGameHeight, this);
	
	AddGameCppFunction(L"AddBrush", L"void", &LogicGame::AddBrush, this);
	AddGameCppFunction(L"SetCurrentBrush", L"void", &LogicGame::SetCurrentBrush, this);
	AddGameCppFunction(L"DrawRectangle", L"void", &LogicGame::DrawRectangle, this);
	AddGameCppFunction(L"DrawEllipse", L"void", &LogicGame::DrawEllipse, this);
}

Parameter LogicGame::AddLayout(vector<Parameter>* inputList)
{
	wstring layoutName = (*inputList)[0].GetValueToWString();
	wstring layoutFilePath = (*inputList)[1].GetValueToWString();

	LLGameLayout* gameLayout = new LLGameLayout();
	gameLayout->LoadLayoutFromFile(layoutFilePath);
	return Parameter();
}

Parameter LogicGame::RemoveLayout(vector<Parameter>* inputList)
{
	wstring layoutName = (*inputList)[0].GetValueToWString();
	gameScene->RemoveNode(layoutName);
	return Parameter();
}

Parameter LogicGame::SetUINodeProperty(vector<Parameter>* inputList)
{
	wstring layoutName = (*inputList)[0].GetValueToWString();
	wstring propertyName = (*inputList)[1].GetValueToWString();
	wstring propertyValue = (*inputList)[2].GetValueToWString();
	gameScene->GetNode(layoutName)->SetProperty(propertyName, propertyValue);
	return Parameter();
}

Parameter LogicGame::BindButtonClickEvent(vector<Parameter>* inputList)
{
	wstring buttonName = (*inputList)[0].GetValueToWString();
	wstring functionName = (*inputList)[1].GetValueToWString();
	LLGameButton* gameButton = (LLGameButton*)gameScene->GetNode(buttonName);
	gameButton->OnMouseClick = [&](void * sender, int e) {
		scriptManager->RunFunction(functionName);
	};
	return Parameter();
}

Parameter LogicGame::BindCanvasRenderEvent(vector<Parameter>* inputList)
{
	wstring canvasName = (*inputList)[0].GetValueToWString();
	wstring functionName = (*inputList)[1].GetValueToWString();
	LLGameCanvas* gameCanvas = (LLGameCanvas*)gameScene->GetNode(canvasName);
	gameCanvas->OnRender = [&](void * sender, int e) {
		scriptManager->RunFunction(functionName);
	};
	return Parameter();
}

Parameter LogicGame::AddParticle(vector<Parameter>* inputList)
{
	wstring particleName = (*inputList)[0].GetValueToWString();
	wstring particleFilePath = (*inputList)[1].GetValueToWString();
	ParticleSystem* particleSystem = new ParticleSystem();
	particleSystem->LoadParticleFromFile(particleFilePath);
	particleSystem->ResetTransform();
	particleSystem->StartPlay();
	particleSystem->SetEnable(true);
	particleMap[particleName] = particleSystem;
	return Parameter();
}

Parameter LogicGame::RemoveParticle(vector<Parameter>* inputList)
{
	wstring particleName = (*inputList)[0].GetValueToWString();
	delete particleMap[particleName];
	particleMap.erase(particleName);
	return Parameter();
}

Parameter LogicGame::LoadActor(vector<Parameter>* inputList)
{
	wstring actorName = (*inputList)[0].GetValueToWString();
	wstring actorFilePath = (*inputList)[1].GetValueToWString();
	Actor* actor = new Actor();
	actor->LoadActorFromFile(actorFilePath);
	actorMap[actorName] = actor;
	return Parameter();
}

Parameter LogicGame::RemoveActor(vector<Parameter>* inputList)
{
	wstring actorName = (*inputList)[0].GetValueToWString();
	delete actorMap[actorName];
	actorMap.erase(actorName);
	return Parameter();
}

Parameter LogicGame::SetActorPosition(vector<Parameter>* inputList)
{
	wstring actorName = (*inputList)[0].GetValueToWString();
	wstring actorPositionXWString = (*inputList)[1].GetValueToWString();
	float actorPositionX = WStringHelper::GetFloat(actorPositionXWString);
	wstring actorPositionYWString = (*inputList)[2].GetValueToWString();
	float actorPositionY = WStringHelper::GetFloat(actorPositionYWString);
	actorMap[actorName]->SetPosition(actorPositionX, actorPositionY);
	return Parameter();
}

Parameter LogicGame::CreatePhysicsWorld(vector<Parameter>* inputList)
{
	physicsWorld = physicsManager->CreatePhysicsWorld();
	return Parameter();
}

Parameter LogicGame::CreatePhysRectangle(vector<Parameter>* inputList)
{
	wstring physName = (*inputList)[0].GetValueToWString();
	wstring widthWString = (*inputList)[1].GetValueToWString();
	float width = WStringHelper::GetFloat(widthWString);
	wstring heightWString = (*inputList)[2].GetValueToWString();
	float height = WStringHelper::GetFloat(heightWString);
	wstring stateWString = (*inputList)[3].GetValueToWString();
	int state = WStringHelper::GetInt(stateWString);
	PhysRectangle* rectangle = physicsManager->CreateRectangle(width, height);
	rectangle->SetName(physName);
	if (state == 0)
	{
		rectangle->SetStatic();
	}
	else if(state == 1)
	{
		rectangle->SetDynamic();
	}
	else if (state == 2)
	{
		rectangle->SetActive();
	}
	physicsWorld->AddObject(rectangle);
	physicsMap[physName] = rectangle;
	return Parameter();
}

Parameter LogicGame::CreatePhysCircle(vector<Parameter>* inputList)
{
	wstring physName = (*inputList)[0].GetValueToWString();
	wstring radiusWString = (*inputList)[1].GetValueToWString();
	float radius = WStringHelper::GetFloat(radiusWString);
	wstring stateWString = (*inputList)[2].GetValueToWString();
	int state = WStringHelper::GetInt(stateWString);
	PhysCircle* circle = physicsManager->CreateCircle(radius);
	circle->SetName(physName);
	if (state == 0)
	{
		circle->SetStatic();
	}
	else if (state == 1)
	{
		circle->SetDynamic();
	}
	else if (state == 2)
	{
		circle->SetActive();
	}
	physicsWorld->AddObject(circle);
	physicsMap[physName] = circle;
	return Parameter();
}

Parameter LogicGame::SetPhysPosition(vector<Parameter>* inputList)
{
	wstring physName = (*inputList)[0].GetValueToWString();
	wstring xWString = (*inputList)[1].GetValueToWString();
	float x = WStringHelper::GetFloat(xWString);
	wstring yWString = (*inputList)[2].GetValueToWString();
	float y = WStringHelper::GetFloat(yWString);
	physicsMap[physName]->SetPosition(x, y);
	return Parameter();
}

Parameter LogicGame::BindCollisionEvent(vector<Parameter>* inputList)
{
	wstring functionName = (*inputList)[0].GetValueToWString();
	physicsWorld->OnCollisionEvent = [&](IPhysObject* object1, IPhysObject* object2) {
		vector<Parameter> tempV;
		tempV.push_back(Parameter(L"string",L"p1", object1->GetName()));
		tempV.push_back(Parameter(L"string", L"p2", object2->GetName()));
		scriptManager->RunFunction(functionName,&tempV);
	};
	return Parameter();
}

Parameter LogicGame::BindConnectServerSuccessEvent(vector<Parameter>* inputList)
{
	wstring functionName = (*inputList)[0].GetValueToWString();
	gameNetClient->OnConnectSuccessHandle = [&]() {
		scriptManager->RunFunction(functionName);
	};
	return Parameter();
}

Parameter LogicGame::BindConnectServerFailEvent(vector<Parameter>* inputList)
{
	wstring functionName = (*inputList)[0].GetValueToWString();
	gameNetClient->OnConnectFailHandle = [&]() {
		scriptManager->RunFunction(functionName);
	};
	gameNetClient->OnDisconnectHandle = [&]() {
		scriptManager->RunFunction(functionName);
	};
	return Parameter();
}

Parameter LogicGame::StartConnectServer(vector<Parameter>* inputList)
{
	gameNetClient->StartConnect(gameConfig.serverIPPort.value);
	return Parameter();
}

Parameter LogicGame::BindKeyDownEvent(vector<Parameter>* inputList)
{
	wstring functionName = (*inputList)[0].GetValueToWString();
	gameWindow->OnKeyDown = [&](void* sender, int key) {
		vector<Parameter> tempV;
		tempV.push_back(Parameter(L"int", L"p1", to_wstring(key)));
		scriptManager->RunFunction(functionName, &tempV);
	};
	return Parameter();
}

Parameter LogicGame::GetMouseX(vector<Parameter>* inputList)
{
	return Parameter(L"float",L"p1",to_wstring(GameHelper::mousePosition.x));
}

Parameter LogicGame::GetMouseY(vector<Parameter>* inputList)
{
	return Parameter(L"float", L"p1", to_wstring(GameHelper::mousePosition.y));
}

Parameter LogicGame::GetGameWidth(vector<Parameter>* inputList)
{
	return Parameter(L"float", L"p1", to_wstring(gameConfig.width.value));
}

Parameter LogicGame::GetGameHeight(vector<Parameter>* inputList)
{
	return Parameter(L"float", L"p1", to_wstring(gameConfig.height.value));
}

Parameter LogicGame::AddBrush(vector<Parameter>* inputList)
{
	wstring brushName = (*inputList)[0].GetValueToWString();
	wstring rWString = (*inputList)[1].GetValueToWString();
	float r = WStringHelper::GetFloat(rWString);
	wstring gWString = (*inputList)[2].GetValueToWString();
	float g = WStringHelper::GetFloat(gWString);
	wstring bWString = (*inputList)[3].GetValueToWString();
	float b = WStringHelper::GetFloat(bWString);
	wstring aWString = (*inputList)[4].GetValueToWString();
	float a = WStringHelper::GetFloat(aWString);
	brushMap[brushName] = GraphicsApi::GetGraphicsApi()->CreateColorBrush(r, g, b, a);
	return Parameter();
}

Parameter LogicGame::SetCurrentBrush(vector<Parameter>* inputList)
{
	wstring brushName = (*inputList)[0].GetValueToWString();
	GraphicsApi::GetGraphicsApi()->SetCurrentBrush(brushMap[brushName]);
	return Parameter();
}

Parameter LogicGame::DrawRectangle(vector<Parameter>* inputList)
{
	wstring isFullWString = (*inputList)[0].GetValueToWString();
	bool isFull = WStringHelper::GetBool(isFullWString);
	wstring xWString = (*inputList)[1].GetValueToWString();
	float x = WStringHelper::GetFloat(xWString);
	wstring yWString = (*inputList)[2].GetValueToWString();
	float y = WStringHelper::GetFloat(yWString);
	wstring widthWString = (*inputList)[3].GetValueToWString();
	float width = WStringHelper::GetFloat(widthWString);
	wstring heightWString = (*inputList)[4].GetValueToWString();
	float height = WStringHelper::GetFloat(heightWString);
	GraphicsApi::GetGraphicsApi()->DrawRect(isFull, x, y, width, height);
	return Parameter();
}

Parameter LogicGame::DrawEllipse(vector<Parameter>* inputList)
{
	wstring isFullWString = (*inputList)[0].GetValueToWString();
	bool isFull = WStringHelper::GetBool(isFullWString);
	wstring xWString = (*inputList)[1].GetValueToWString();
	float x = WStringHelper::GetFloat(xWString);
	wstring yWString = (*inputList)[2].GetValueToWString();
	float y = WStringHelper::GetFloat(yWString);
	wstring radius1WString = (*inputList)[3].GetValueToWString();
	float radius1 = WStringHelper::GetFloat(radius1WString);
	wstring radius2WString = (*inputList)[4].GetValueToWString();
	float radius2 = WStringHelper::GetFloat(radius2WString);
	GraphicsApi::GetGraphicsApi()->DrawEllipse(isFull, x, y, radius1, radius2);
	return Parameter();
}
