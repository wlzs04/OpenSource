#include "LLGame.h"

LLGame::LLGame()
{
}

LLGame::~LLGame()
{
	GraphicsApi::ReleaseGraphicsApi();
	if (gameWindow)
	{
		delete gameWindow;
		gameWindow = nullptr;
	}
}

void LLGame::Init()
{
	currentPath = SystemHelper::GetCurrentPath();
	LoadConfig();
	InitWindow();
	InitData();
}

void LLGame::Start()
{
	if (!gameExit)
	{
		SystemHelper::SetCursorCenter();
		gameTimer.Start();
		gameWindow->Run();
	}
}

void LLGame::LoadConfig()
{
	LLXMLDocument xmlDocument;
	if(xmlDocument.LoadXMLFromFile(currentPath + L"\\" + L"Game.xml"))
	{
		LLXMLNode* rootNode=xmlDocument.GetRootNode();
		gameConfig.gameName = rootNode->GetProperty(L"gameName")->GetValue();
		gameConfig.width = rootNode->GetProperty(L"width")->GetValueInt();
		gameConfig.height = rootNode->GetProperty(L"height")->GetValueInt();
		gameConfig.fullScreen = rootNode->GetProperty(L"fullScreen")->GetValueBool();
		gameConfig.canMultiGame = rootNode->GetProperty(L"canMultiGame")->GetValueBool(); 
		gameConfig.startScene = rootNode->GetProperty(L"startScene")->GetValue();
		gameConfig.graphicsApi = rootNode->GetProperty(L"graphicsApi")->GetValue();
		gameConfig.openNetClient = rootNode->GetProperty(L"openNetClient")->GetValueBool();
		gameConfig.defaultCursor = rootNode->GetProperty(L"defaultCursor")->GetValue();
		SystemHelper::resourcePath = gameConfig.resourcePath;
	}
	else
	{
		MessageHelper::ShowMessage(L"配置文件加载失败！");
	}
}

void LLGame::SaveConfig()
{
	LLXMLDocument xmlDocument;
	LLXMLNode* node = new LLXMLNode(L"Game");
	node->AddProperty(new LLXMLProperty(L"gameName", gameConfig.gameName));
	node->AddProperty(new LLXMLProperty(L"width", to_wstring(gameConfig.width)));
	node->AddProperty(new LLXMLProperty(L"height", to_wstring(gameConfig.height)));
	node->AddProperty(new LLXMLProperty(L"fullScreen", to_wstring(gameConfig.fullScreen)));
	node->AddProperty(new LLXMLProperty(L"canMultiGame", to_wstring(gameConfig.canMultiGame)));
	node->AddProperty(new LLXMLProperty(L"startScene", gameConfig.startScene));
	node->AddProperty(new LLXMLProperty(L"graphicsApi", gameConfig.graphicsApi));
	node->AddProperty(new LLXMLProperty(L"openNetClient", to_wstring(gameConfig.openNetClient)));
	node->AddProperty(new LLXMLProperty(L"defaultCursor", gameConfig.defaultCursor));
	xmlDocument.SetRootNode(node);
	if (!xmlDocument.SaveXMLToFile(currentPath + L"\\" + L"Game.xml"))
	{
		MessageHelper::ShowMessage(L"配置文件保存失败！");
	}
}

void LLGame::OnRunEvent()
{
	Update();
	Render();
}

void LLGame::OnLeftMouseDown(void* iuiNode, int i)
{
	GameHelper::mouseLeftButtonPassed = true;
	gameScene->CheckState();
}

void LLGame::OnLeftMouseUp(void* iuiNode, int i)
{
	GameHelper::mouseLeftButtonPassed = false;
	gameScene->CheckState();
	IUINode::uiLock = false;
}

void LLGame::OnRightMouseDown(void* iuiNode, int i)
{
	GameHelper::mouseRightButtonPassed = true;
	gameScene->CheckState();
}

void LLGame::OnRightMouseUp(void* iuiNode, int i)
{
	GameHelper::mouseRightButtonPassed = false;
	gameScene->CheckState();
}

void LLGame::OnMouseOver(void* iuiNode, int i)
{
	GameHelper::mousePosition.x = GET_X_LPARAM(i);
	GameHelper::mousePosition.y = GET_Y_LPARAM(i);
	gameScene->CheckState();
}

void LLGame::OnMouseWheel(void * iuiNode, int i)
{
	GameHelper::mouseWheelValue += i; 
	gameScene->CheckState();
}

void LLGame::Update()
{
	gameTimer.Tick();
	GameHelper::thisTickTime=gameTimer.GetThisTickTime();
	gameScene->Update();
}

void LLGame::Render()
{
	GraphicsApi::GetGraphicsApi()->BeginRender();
	GraphicsApi::GetGraphicsApi()->Clear();
	gameScene->Render();
	GraphicsApi::GetGraphicsApi()->EndRender();
}

void LLGame::InitWindow()
{
	if (!gameConfig.canMultiGame)
	{
		HANDLE hMutex = CreateMutex(NULL, TRUE, gameConfig.gameName.c_str());
		if (GetLastError() == ERROR_ALREADY_EXISTS)
		{
			HWND hWnd = FindWindow(LLGameWindow::className.c_str(), gameConfig.gameName.c_str());
			SetForegroundWindow(hWnd);
			if (hMutex)
			{
				ReleaseMutex(hMutex);
			}
			gameExit = true;
			return;
		}
	}
	gameWindow = new LLGameWindow();
	int screenWidth = SystemHelper::GetScreenWidth();
	int screenHeight = SystemHelper::GetScreenHeight();

	if (gameConfig.fullScreen)
	{
		gameWindow->SetPosition(0, 0);
		gameWindow->SetSize(screenWidth, screenHeight);
		gameWindow->SetTitle(gameConfig.gameName);
	}
	else
	{
		gameWindow->SetPosition((screenWidth - gameConfig.width) / 2, (screenHeight - gameConfig.height) / 2);
		gameWindow->SetSize(gameConfig.width, gameConfig.height);
		gameWindow->SetTitle(gameConfig.gameName);
	}
}

void LLGame::InitData()
{
	if (!gameExit)
	{
		GraphicsApi* graphicsApi;
		if (gameConfig.graphicsApi == L"Direct2D")
		{
			graphicsApi = new Direct2DApi(gameWindow->GetHWND());
			graphicsApi->SetSize(gameConfig.width, gameConfig.height);
			graphicsApi->Init();
			GraphicsApi::SetGraphicsApi(graphicsApi);
		}
		else
		{
			MessageHelper::ShowMessage(L"其它底层图形API正在开发当中！");
			gameExit = true;
		}

		if (gameConfig.defaultCursor==L"")
		{
			SetCursor(LoadCursor(GetModuleHandle(0), IDC_ARROW));
		}
		else
		{
			SetCursor(LoadCursor(GetModuleHandle(0), gameConfig.defaultCursor.c_str()));
		}
		

		GameHelper::width = gameConfig.width;
		GameHelper::height = gameConfig.height;

		gameScene = new LLGameScene();
		gameScene->LoadSceneFromFile(SystemHelper::GetResourceRootPath() + L"\\" + gameConfig.startScene);

		gameWindow->OnRunEvent = bind(&LLGame::OnRunEvent, this);
		gameWindow->OnMouseOver = bind(&LLGame::OnMouseOver, this, placeholders::_1, placeholders::_2);
		gameWindow->OnLeftMouseDown = bind(&LLGame::OnLeftMouseDown, this, placeholders::_1, placeholders::_2);
		gameWindow->OnLeftMouseUp = bind(&LLGame::OnLeftMouseUp, this, placeholders::_1, placeholders::_2);
		gameWindow->OnRightMouseDown = bind(&LLGame::OnRightMouseDown, this, placeholders::_1, placeholders::_2);
		gameWindow->OnRightMouseUp = bind(&LLGame::OnRightMouseUp, this, placeholders::_1, placeholders::_2);

		InitUserData();
	}
}