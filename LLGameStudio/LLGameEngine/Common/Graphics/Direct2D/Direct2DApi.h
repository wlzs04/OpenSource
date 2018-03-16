#pragma once
#include "..\GraphicsApi.h"
#include <d2d1.h>
#include <Wincodec.h>
#include <dwrite.h>
#include <wrl.h>
#include <unordered_map>

#pragma comment(lib,"d2d1.lib")
#pragma comment(lib,"dwrite.lib")

class Direct2DApi :public GraphicsApi
{
public:
	Direct2DApi(HWND hWnd);
	~Direct2DApi();
	virtual void Init() override;
	virtual void DrawRect(float x, float y, float width, float height) override;
	virtual void DrawImage(std::wstring image, float x, float y, float width, float height) override;
	virtual void DrawText(wstring text, float x, float y, float width, float height, wstring  textFormatName = L"") override;
	virtual void AddImage(wstring image) override;
	virtual void AddTextFormat(wstring textFormatName, wstring fontFamilyName, float fontSize) override;
	virtual void Clear() override;
	virtual void BeginRender() override;
	virtual void EndRender() override;
private:
	Microsoft::WRL::ComPtr<ID2D1Factory> d2dFactory; 
	Microsoft::WRL::ComPtr<ID2D1HwndRenderTarget> d2dRenderTarget;
	Microsoft::WRL::ComPtr<ID2D1SolidColorBrush> d2dBlackBrush;
	Microsoft::WRL::ComPtr<IWICImagingFactory> wicImageFactory;
	ID2D1SolidColorBrush* currentD2DBrush;
	unordered_map<std::wstring, Microsoft::WRL::ComPtr<ID2D1Bitmap>> imageMap;

	Microsoft::WRL::ComPtr<IDWriteFactory> writeFactory;
	unordered_map<std::wstring, Microsoft::WRL::ComPtr<IDWriteTextFormat>> textFormatMap;
};