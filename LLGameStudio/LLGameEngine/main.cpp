//#include <windows.h>
//#include <D2D1.h>
//#include <string>
//#include <Wincodec.h>
//#include <dwrite.h>
//
//#pragma comment(lib,"d2d1.lib")
//#pragma comment(lib,"dwrite.lib")
//
//using namespace std;
//
//ID2D1Factory* pD2DFactory = NULL; 
//ID2D1HwndRenderTarget* pRenderTarget = NULL; 
//ID2D1SolidColorBrush* pBlackBrush = NULL;
//
//IWICImagingFactory* wicFactory = NULL;
//ID2D1Bitmap* pBitmap = NULL;
//IDWriteFactory* writeFactory;
//IDWriteTextFormat* textFormat;
//
//wstring text=L"asdasdasd";
//
//LRESULT WINAPI MsgProc(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam)
//{
//	switch (msg)
//	{
//	case WM_DESTROY:
//	{
//		PostQuitMessage(0);
//		return 0;
//	}
//	}
//	return DefWindowProc(hwnd, msg, wParam, lParam);
//}
//
//void LoadImageFromFile(wstring bitmapName)
//{
//	CoInitialize(NULL);
//	CoCreateInstance(CLSID_WICImagingFactory, nullptr, CLSCTX_INPROC_SERVER, IID_PPV_ARGS(&wicFactory));
//
//	IWICBitmapDecoder* decoder;
//	IWICBitmapFrameDecode* source;
//	IWICFormatConverter* converter;
//	IWICBitmap* wicBitmap;
//
//	wicFactory->CreateDecoderFromFilename(bitmapName.c_str(), NULL, GENERIC_READ, WICDecodeMetadataCacheOnLoad, &decoder);
//
//	decoder->GetFrame(0, &source);
//	wicFactory->CreateFormatConverter(&converter);
//	converter->Initialize(source, GUID_WICPixelFormat32bppPBGRA, WICBitmapDitherTypeNone, nullptr, 0, WICBitmapPaletteTypeMedianCut);
//	wicFactory->CreateBitmapFromSource(converter, WICBitmapCacheOnDemand, &wicBitmap);
//	pRenderTarget->CreateBitmapFromWicBitmap(wicBitmap, &pBitmap);
//
//	decoder->Release();
//	decoder = NULL;
//	source->Release();
//	source = NULL;
//	converter->Release();
//	converter = NULL;
//	wicBitmap->Release();
//	wicBitmap = NULL;
//}
//
//VOID DrawRectangle()
//{
//	pRenderTarget->BeginDraw();
//
//	pRenderTarget->Clear(D2D1::ColorF(D2D1::ColorF::White));
//
//	pRenderTarget->DrawRectangle(
//		D2D1::RectF(100.f, 100.f, 500.f, 500.f),
//		pBlackBrush
//	);
//
//	pRenderTarget->EndDraw();
//}
//
//
//VOID DrawImageAndRectangle()
//{
//	pRenderTarget->BeginDraw();
//
//	pRenderTarget->Clear(D2D1::ColorF(D2D1::ColorF::White));
//	
//	pRenderTarget->DrawBitmap(pBitmap, D2D1::RectF(100.f, 100.f, 500.f, 500.f));
//	pRenderTarget->DrawRectangle(
//		D2D1::RectF(100.f, 100.f, 500.f, 500.f),
//		pBlackBrush
//	);
//	pRenderTarget->EndDraw();
//}
//
//void DrawImageAndRectangleAndText()
//{
//	pRenderTarget->BeginDraw();
//
//	pRenderTarget->Clear(D2D1::ColorF(D2D1::ColorF::White));
//
//	pRenderTarget->DrawBitmap(pBitmap, D2D1::RectF(100.f, 100.f, 500.f, 500.f));
//	pRenderTarget->DrawRectangle(
//		D2D1::RectF(100.f, 100.f, 500.f, 500.f),
//		pBlackBrush
//	);
//	pRenderTarget->DrawTextW(text.c_str(), text.size(), textFormat, D2D1::RectF(100.f, 100.f, 500.f, 500.f), pBlackBrush);
//
//	pRenderTarget->EndDraw();
//}
//
//int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nShowCmd)
//{
//	//当使用WNDCLASS时，无cbSize属性。使用WNDCLASSEX需要cbSize属性。
//	WNDCLASSEX wc = {};
//	wc.cbSize = sizeof(WNDCLASSEX);
//	wc.lpfnWndProc = MsgProc;
//	wc.hInstance = hInstance;
//	wc.lpszClassName = L"gameClass";
//
//	RegisterClassEx(&wc);
//	//WS_POPUP无边框并且不绘制时不显示窗体，WS_OVERLAPPEDWINDOW有边框。
//	//WS_POPUPWINDOW无边框不绘制时显示窗体,但在边缘处有一像素的边框。
//	HWND hWnd = CreateWindow(L"gameClass", L"game标题", WS_POPUPWINDOW, 200, 200, 800, 600, NULL, NULL, hInstance, NULL);
//	
//	ShowWindow(hWnd, SW_SHOWDEFAULT);
//	UpdateWindow(hWnd);
//
//	D2D1CreateFactory(D2D1_FACTORY_TYPE_SINGLE_THREADED, &pD2DFactory);
//
//	pD2DFactory->CreateHwndRenderTarget(
//		D2D1::RenderTargetProperties(),
//		D2D1::HwndRenderTargetProperties(
//			hWnd,
//			D2D1::SizeU(800, 600)
//		),
//		&pRenderTarget
//	);
//
//	pRenderTarget->CreateSolidColorBrush(
//		D2D1::ColorF(D2D1::ColorF::Red),
//		&pBlackBrush
//	);
//
//	LoadImageFromFile(L"C:\\Users\\admin\\Desktop\\qipan.jpg");
//
//	DWriteCreateFactory(DWRITE_FACTORY_TYPE_SHARED, __uuidof(IDWriteFactory), (IUnknown**)&writeFactory);
//	writeFactory->CreateTextFormat(L"宋体", NULL,
//		DWRITE_FONT_WEIGHT_NORMAL, DWRITE_FONT_STYLE_NORMAL, DWRITE_FONT_STRETCH_NORMAL, 30, L"chs", &textFormat);
//	MSG msg = { 0 };
//	while (msg.message != WM_QUIT)
//	{
//		if (PeekMessage(&msg, nullptr, 0, 0, PM_REMOVE))
//		{
//			TranslateMessage(&msg);
//			DispatchMessage(&msg);
//		}
//		else
//		{
//			//DrawImageAndRectangle();
//			DrawImageAndRectangleAndText();
//		}
//	}
//
//	pBitmap->Release();
//	pBitmap = NULL;
//	pBlackBrush->Release();
//	pBlackBrush = NULL;
//	pRenderTarget->Release();
//	pRenderTarget = NULL;
//	pD2DFactory->Release();
//	pD2DFactory = NULL;
//
//	CoFreeUnusedLibraries();
//
//	UnregisterClass(L"gameClass", hInstance);
//	return 0;
//}

#include "Common\XML\LLXML.h"
#include <stdio.h>
#include <stdlib.h>

int main()
{
	LLXMLDocument document;
	if (document.LoadXMLFromFile(L"layout1.layout"))
	{
		int y = 0;
	}
	else
	{
		printf("XML文件读取失败！");
	}
	LLXMLNode* llXMLNode = document.GetRootNode();
	system("pause");
}