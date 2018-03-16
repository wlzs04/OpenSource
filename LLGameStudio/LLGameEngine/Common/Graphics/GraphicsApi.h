#pragma once
#include <windows.h>
#include <string>

using namespace std;

class GraphicsApi
{
public:
	GraphicsApi(HWND hWnd);
	virtual void DrawRect(float x, float y,float width,float height)=0;
	virtual void Clear() = 0;
	virtual void BeginRender() = 0;
	virtual void EndRender() = 0;
	virtual void Init() = 0;
	virtual void AddImage(wstring image) = 0;
	virtual void DrawImage(wstring image,float x,float y, float width, float height)=0;
	virtual void DrawText(wstring text, float x, float y, float width, float height,wstring  textFormatName=L"") = 0;
	virtual void AddTextFormat(wstring textFormatName, wstring fontFamilyName, float fontSize) = 0;
	virtual void SetSize(float width,float height);
	static void SetGraphicsApi(GraphicsApi* currentGraphicsApi);
	static GraphicsApi* GetGraphicsApi();
	static void ReleaseGraphicsApi();

protected:
	HWND hWnd;
	float width = 0;
	float height = 0;
private:
	static GraphicsApi* graphicsApi;
};