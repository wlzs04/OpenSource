#include "LLGame.h"

LLGame::LLGame()
{
}

LLGame::~LLGame()
{
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
}

void LLGame::Start()
{
	if (gameWindow)
	{
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
	xmlDocument.SetRootNode(node);
	if (!xmlDocument.SaveXMLToFile(currentPath + L"\\" + L"Game.xml"))
	{
		MessageHelper::ShowMessage(L"配置文件保存失败！");
	}
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
}
