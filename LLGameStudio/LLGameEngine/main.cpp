
#include "Common/Window/LLGameWindow.h"

int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nShowCmd)
{
	bool canMultiWindow = false;
	if(!canMultiWindow)
	{
		wstring title = L"窗体标题";
		HANDLE hMutex = CreateMutex(NULL, TRUE, title.c_str());
		if (GetLastError() == ERROR_ALREADY_EXISTS)
		{
			HWND hWnd = FindWindow(LLGameWindow::className.c_str(), title.c_str());
			SetForegroundWindow(hWnd);
			if (hMutex)
			{
				ReleaseMutex(hMutex);
			}
			return 0;
		}
	}
	LLGameWindow* gameWindow1;
	gameWindow1=new LLGameWindow(hInstance);
	gameWindow1->SetTitle(L"game1");
	gameWindow1->SetPosition(300, 300);
	gameWindow1->SetSize(1200, 800);
	gameWindow1->Run();

	delete gameWindow1;
	return 0;
}