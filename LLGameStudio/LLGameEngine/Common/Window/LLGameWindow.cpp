#include "LLGameWindow.h"

const wstring LLGameWindow::className = L"LLGameWindow";

LLGameWindow::LLGameWindow(HINSTANCE hInstance)
{
	this->hInstance = hInstance;
	left = 0;
	top = 0;
	width = 800;
	height = 600;
	InitWindow();
}

LLGameWindow::~LLGameWindow()
{
	UnregisterClass(className.c_str(), hInstance);
}

void LLGameWindow::Run()
{
	if (OnBeginEvent)
	{
		OnBeginEvent();
	}

	ShowWindow(hWnd, SW_SHOW);

	MSG msg = { 0 };
	while (GetMessage(&msg, NULL, 0, 0))
	{
		TranslateMessage(&msg);
		DispatchMessage(&msg);
	}
	while (msg.message != WM_QUIT)
	{
		if (PeekMessage(&msg, nullptr, 0, 0, PM_REMOVE))
		{
			TranslateMessage(&msg);
			DispatchMessage(&msg);
		}
		else
		{
			if (OnRunEvent)
			{
				OnRunEvent();
			}
		}
	}
	if (OnEndEvent)
	{
		OnEndEvent();
	}
}

void LLGameWindow::SetPosition(double left, double top)
{
	this->left = left;
	this->top = top;
	SetWindowPos(hWnd, nullptr, left, top, width, height, SWP_NOZORDER);
}

void LLGameWindow::SetSize(double width, double height)
{
	this->width = width;
	this->height = height;
	SetWindowPos(hWnd, nullptr, left, top, width, height, SWP_NOZORDER);
}

void LLGameWindow::SetTitle(wstring title)
{
	this->title = title;
	SetWindowText(hWnd, title.c_str());
}

void LLGameWindow::InitWindow()
{
	WNDCLASSEX wc = {};
	wc.cbSize = sizeof(WNDCLASSEX);
	wc.lpfnWndProc = WndProcess;
	wc.hbrBackground = (HBRUSH)GetStockObject(WHITE_BRUSH);
	wc.hInstance = hInstance;
	wc.lpszClassName = L"GameWindow";
	RegisterClassEx(&wc);
	hWnd = CreateWindow(L"GameWindow", L"", WS_POPUP, 0, 0, 800, 600, 0, 0, hInstance, 0);

	//使用系统提供的方法将类指针和hWnd关联起来，可以在窗体处理方法中获得。
	SetWindowLong(hWnd, GWLP_USERDATA, (LONG)this);
}

LRESULT LLGameWindow::WindowProcess(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam)
{
	switch (msg)
	{
	case WM_ACTIVATE:
	{
		if (LOWORD(wParam) == WA_INACTIVE)
		{
			if (OnInActivate)
			{
				OnInActivate();
			}
		}
		else
		{
			if (OnActivate)
			{
				OnActivate();
			}
		}
		return 0;
	}
	case WM_DESTROY:
	{
		PostQuitMessage(0);
		return 0;
	}
	case WM_CHAR://不经过输入法获得字符
	case WM_IME_CHAR://经过输入法获得字符
	{
		if (OnGetChar)
		{
			OnGetChar(this, wParam);
		}
		return 0;
	}
	case WM_KEYDOWN:
	{
		if (OnKeyDown)
		{
			OnKeyDown(this, wParam);
		}
		return 0;
	}
	case WM_LBUTTONDOWN:
	{
		if (OnLeftMouseDown)
		{
			OnLeftMouseDown(this, wParam);
		}
		return 0;
	}
	case WM_RBUTTONDOWN:
	{
		if (OnRightMouseDown)
		{
			OnRightMouseDown(this, wParam);
		}
		return 0;
	}
	case WM_LBUTTONUP:
	{
		if (OnLeftMouseUp)
		{
			OnLeftMouseUp(this, wParam);
		}
		return 0;
	}
	case WM_RBUTTONUP:
	{
		if (OnRightMouseUp)
		{
			OnRightMouseUp(this, wParam);
		}
		return 0;
	}
	case WM_MOUSEMOVE:
	{
		if (OnMouseOver)
		{
			OnMouseOver(this, wParam);
		}
		//OnMouseOver(GET_X_LPARAM(lParam), GET_Y_LPARAM(lParam));
		return 0;
	}
	case WM_MOUSEWHEEL:
	{
		if (OnMouseWheel)
		{
			OnMouseWheel(this, GET_WHEEL_DELTA_WPARAM(wParam));
		}
		return 0;
	}
	}
	return DefWindowProc(hwnd, msg, wParam, lParam);
}

LRESULT LLGameWindow::WndProcess(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam)
{
	//64位：GWLP_USERDATA 、32位：GWL_USERDATA
	LLGameWindow* gameWindow=(LLGameWindow*)GetWindowLong(hwnd, GWLP_USERDATA);
	if (gameWindow)
	{
		return gameWindow->WindowProcess(hwnd, msg, wParam, lParam);
	}
	return DefWindowProc(hwnd, msg, wParam, lParam);
}
