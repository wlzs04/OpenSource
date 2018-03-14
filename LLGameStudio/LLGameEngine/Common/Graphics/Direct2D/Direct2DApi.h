#pragma once
#include "..\GraphicsApi.h"
#include <d2d1.h>
#include <Wincodec.h>
#include <wrl.h>
#include <unordered_map>

#pragma comment(lib,"d2d1.lib")

class Direct2DApi :public GraphicsApi
{
public:
	Direct2DApi(HWND hWnd);
	~Direct2DApi();
	virtual void Init() override;
	virtual void DrawRect(float x, float y, float width, float height) override;
	virtual void DrawImage(std::wstring image, float x, float y, float width, float height) override;
	virtual void AddImage(wstring image) override;
	virtual void Clear() override;
	virtual void BeginRender() override;
	virtual void EndRender() override;
private:
	Microsoft::WRL::ComPtr<ID2D1Factory> d2dFactory; 
	Microsoft::WRL::ComPtr<ID2D1HwndRenderTarget> d2dRenderTarget;
	Microsoft::WRL::ComPtr<ID2D1SolidColorBrush> d2dBlackBrush;
	Microsoft::WRL::ComPtr<IWICImagingFactory> wicImageFactory;
	ID2D1SolidColorBrush* d2dCurrentBrush;
	unordered_map<std::wstring, Microsoft::WRL::ComPtr<ID2D1Bitmap>> imageMap;
};