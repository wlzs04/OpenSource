#pragma once
#include <windows.h>
#include <D2D1.h>

#pragma comment(lib,"d2d1.lib")

ID2D1Factory* pD2DFactory = NULL; 
ID2D1HwndRenderTarget* pRenderTarget = NULL; 
ID2D1SolidColorBrush* pBlackBrush = NULL; 

RECT rc;

LRESULT WINAPI MsgProc(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam)
{
	switch (msg)
	{
	case WM_DESTROY:
	{
		PostQuitMessage(0);
		return 0;
	}
	}
	return DefWindowProc(hwnd, msg, wParam, lParam);
}

VOID DrawRectangle()
{
	pRenderTarget->BeginDraw();

	pRenderTarget->Clear(D2D1::ColorF(D2D1::ColorF::White));

	pRenderTarget->DrawRectangle(
		D2D1::RectF(100.f, 100.f, 500.f, 500.f),
		pBlackBrush
	);

	pRenderTarget->EndDraw();
}

int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nShowCmd)
{
	//当时用WNDCLASS时，无cbSize属性。使用WNDCLASSEX需要cbSize属性。
	WNDCLASSEX wc = {};
	wc.cbSize = sizeof(WNDCLASSEX);
	wc.lpfnWndProc = MsgProc;
	wc.hInstance = hInstance;
	wc.lpszClassName = L"game";

	RegisterClassEx(&wc);
	//WS_POPUP无边框并且不绘制时不显示窗体，WS_OVERLAPPEDWINDOW有边框。
	//WS_POPUPWINDOW无边框不绘制时显示窗体,但在边缘处有一像素的边框。
	HWND hWnd = CreateWindow(L"game", L"game", WS_POPUP, 200, 200, 800, 600, NULL, NULL, hInstance, NULL);
	
	ShowWindow(hWnd, SW_SHOWDEFAULT);
	UpdateWindow(hWnd);

	rc.left = 0;
	rc.right = 800;
	rc.top = 0;
	rc.bottom = 600;

	D2D1CreateFactory(D2D1_FACTORY_TYPE_SINGLE_THREADED, &pD2DFactory);

	pD2DFactory->CreateHwndRenderTarget(
		D2D1::RenderTargetProperties(),
		D2D1::HwndRenderTargetProperties(
			hWnd,
			D2D1::SizeU(rc.right - rc.left, rc.bottom - rc.top)
		),
		&pRenderTarget
	);

	pRenderTarget->CreateSolidColorBrush(
		D2D1::ColorF(D2D1::ColorF::Red),
		&pBlackBrush
	);

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
			DrawRectangle();
		}
	}
	UnregisterClass(L"game", hInstance);
	return 0;
}