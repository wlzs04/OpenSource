#include "LLGameWindow.h"

const wstring LLGameWindow::className = L"LLGameWindow";

void LLGameWindow::AddHandleUIEvent(wstring eventName, HandleUIEvent handleUIEvent)
{
	eventMap[eventName] = handleUIEvent;
}

void LLGameWindow::RemoveHandleUIEvent(wstring eventName)
{
	if (eventMap.count(eventName) != 0)
	{
		eventMap[eventName] == nullptr;
	}
}

LLGameWindow::LLGameWindow()
{
	left = 0;
	top = 0;
	width = 800;
	height = 600;
	InitWindow();
}

LLGameWindow::~LLGameWindow()
{
	UnregisterClass(className.c_str(), GetModuleHandle(0));
}

HWND LLGameWindow::GetHWND()
{
	return hWnd;
}

void LLGameWindow::Run()
{
	if (OnBeginEvent != nullptr)
	{
		OnBeginEvent();
	}

	ShowWindow(hWnd, SW_SHOW);

	MSG msg = { 0 };
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

void LLGameWindow::SetMaximize()
{
	ShowWindow(hWnd, SW_MAXIMIZE);
}

void LLGameWindow::SetMinimize()
{
	ShowWindow(hWnd, SW_MINIMIZE);
}

void LLGameWindow::InitWindow()
{
	WNDCLASSEX wc = {};
	wc.cbSize = sizeof(WNDCLASSEX);
	wc.lpfnWndProc = WndProcess;
	wc.hbrBackground = (HBRUSH)GetStockObject(WHITE_BRUSH);
	wc.hInstance = GetModuleHandle(0);
	wc.lpszClassName = className.c_str();
	RegisterClassEx(&wc);
	hWnd = CreateWindow(className.c_str(), L"", WS_POPUP, 0, 0, 800, 600, 0, 0, GetModuleHandle(0), 0);

	//使用系统提供的方法将类指针和hWnd关联起来，可以在窗体处理方法中获得。
	SetWindowLong(hWnd, GWLP_USERDATA, (LONG)this);

	AddHandleUIEvent(L"OnLeftMouseDown", OnLeftMouseDown);
	AddHandleUIEvent(L"OnLeftMouseUp", OnLeftMouseUp);
	AddHandleUIEvent(L"OnRightMouseDown", OnRightMouseDown);
	AddHandleUIEvent(L"OnRightMouseUp", OnRightMouseUp);
	AddHandleUIEvent(L"OnMouseOver", OnMouseOver);
	AddHandleUIEvent(L"OnMouseWheel", OnMouseWheel);
	AddHandleUIEvent(L"OnGetChar", OnGetChar);
	AddHandleUIEvent(L"OnKeyDown", OnKeyDown);
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
			OnMouseOver(this, lParam);
		}
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
