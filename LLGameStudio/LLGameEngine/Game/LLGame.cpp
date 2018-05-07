#include "LLGame.h"
#include "..\Common\Encrypt\EncryptNumber.h"

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
		gameTimer.Start();
		gameWindow->Run();
	}
}

void LLGame::LoadConfig()
{
	LLXMLDocument xmlDocument;
	if(xmlDocument.LoadXMLFromFile(currentPath + L"\\" + L"Game.xml"))
	{
		LLXMLNode* rootNode = xmlDocument.GetRootNode();
		gameConfig.LoadFromXMLNode(rootNode);
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
	
	xmlDocument.SetRootNode(gameConfig.ExportToXMLNode());
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
	UpdateUserData();
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
	if (!gameConfig.canMultiGame.value)
	{
		HANDLE hMutex = CreateMutex(NULL, TRUE, gameConfig.gameName.value.c_str());
		if (GetLastError() == ERROR_ALREADY_EXISTS)
		{
			HWND hWnd = FindWindow(LLGameWindow::className.c_str(), gameConfig.gameName.value.c_str());
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
	if (gameConfig.icon.value != L"")
	{
		gameWindow->SetIcon((HICON)LoadImage(GetModuleHandle(0), (SystemHelper::GetResourceRootPath() + L"\\" + gameConfig.icon.value).c_str(), IMAGE_ICON, 0, 0, LR_DEFAULTCOLOR | LR_CREATEDIBSECTION | LR_LOADFROMFILE));
	}
	gameWindow->InitWindow();
	int screenWidth = SystemHelper::GetScreenWidth();
	int screenHeight = SystemHelper::GetScreenHeight();

	if (gameConfig.fullScreen.value)
	{
		gameWindow->SetPosition(0, 0);
		gameWindow->SetSize(screenWidth, screenHeight);
		gameWindow->SetTitle(gameConfig.gameName.value);
	}
	else
	{
		if (gameConfig.middleInScreen.value)
		{
			gameWindow->SetPosition((screenWidth - gameConfig.width.value) / 2, (screenHeight - gameConfig.height.value) / 2);
		}
		else
		{
			gameWindow->SetPosition(gameConfig.left.value, gameConfig.top.value);
		}
		gameWindow->SetSize(gameConfig.width.value, gameConfig.height.value);
		gameWindow->SetTitle(gameConfig.gameName.value);
	}

	//注册事件
	gameWindow->OnActivate = [&]() {
		gameTimer.Start();
	};
	gameWindow->OnInActivate = [&]() {
		gameTimer.Stop();
	};

	gameWindow->OnRunEvent = bind(&LLGame::OnRunEvent, this);
	gameWindow->OnMouseOver = bind(&LLGame::OnMouseOver, this, placeholders::_1, placeholders::_2);
	gameWindow->OnLeftMouseDown = bind(&LLGame::OnLeftMouseDown, this, placeholders::_1, placeholders::_2);
	gameWindow->OnLeftMouseUp = bind(&LLGame::OnLeftMouseUp, this, placeholders::_1, placeholders::_2);
	gameWindow->OnRightMouseDown = bind(&LLGame::OnRightMouseDown, this, placeholders::_1, placeholders::_2);
	gameWindow->OnRightMouseUp = bind(&LLGame::OnRightMouseUp, this, placeholders::_1, placeholders::_2);
}

void LLGame::InitData()
{
	if (!gameExit)
	{
		GameHelper::width = gameConfig.width.value;
		GameHelper::height = gameConfig.height.value;

		//设置默认光标
		{
			HCURSOR defaultCursor;
			if (gameConfig.defaultCursor.value == L"")
			{
				defaultCursor = LoadCursor(GetModuleHandle(0), IDC_ARROW);
			}
			else
			{
				defaultCursor = LoadCursorFromFile((SystemHelper::GetResourceRootPath() + L"\\" + gameConfig.defaultCursor.value).c_str());
			}
			gameWindow->SetDefaultCursor(defaultCursor);
		}

		InitGraphics();
		if (gameConfig.openNetClient.value){InitNetClient();}
		if (gameConfig.openPhysics.value){InitPhysics();}
		
		gameScene = new LLGameScene();
		gameScene->LoadSceneFromFile(SystemHelper::GetResourceRootPath() + L"\\" + gameConfig.startScene.value);

		InitUserData();
	}
}

void LLGame::InitGraphics()
{
	GraphicsApi* graphicsApi;
	if (gameConfig.graphicsApi.value == GraphicsApiType::Direct2D)
	{
		graphicsApi = new Direct2DApi(gameWindow->GetHWND());
		graphicsApi->SetSize(gameConfig.width.value, gameConfig.height.value);
		graphicsApi->Init();
		GraphicsApi::SetGraphicsApi(graphicsApi);
	}
	else
	{
		MessageHelper::ShowMessage(L"其它底层图形API正在开发当中！");
		gameExit = true;
	}
}

void LLGame::InitNetClient()
{
	gameNetClient = new LLGameNetClient();
	EncryptNumber* encryptNumber = new EncryptNumber();
	encryptNumber->SetKey(WStringHelper::WStringToString(gameConfig.encryptKey.value));
	gameNetClient->SetEncryptClass(encryptNumber);
}

void LLGame::InitPhysics()
{
	physicsManager = new PhysicsManager();
}
