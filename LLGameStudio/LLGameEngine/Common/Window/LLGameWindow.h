#pragma once
#include <windows.h>
#include <windowsx.h>
#include <string>
#include <functional>

using namespace std;

typedef function<void()> HandlerEvent;
typedef function<void(void*,int)> HandlerUIEvent;

class LLGameWindow
{
public:
	LLGameWindow(HINSTANCE hInstance);
	~LLGameWindow();
	void Run();
	void SetPosition(double left,double top);
	void SetSize(double width, double height);
	void SetTitle(wstring title);

	//开始事件
	HandlerEvent OnBeginEvent = nullptr;
	//运行事件
	HandlerEvent OnRunEvent = nullptr;
	//结束事件
	HandlerEvent OnEndEvent = nullptr;
	//获得焦点
	HandlerEvent OnActivate = nullptr;
	//失去焦点
	HandlerEvent OnInActivate = nullptr;
	//左键点击
	HandlerUIEvent OnLeftMouseDown = nullptr;
	//左键弹起
	HandlerUIEvent OnLeftMouseUp = nullptr;
	//右键点击
	HandlerUIEvent OnRightMouseDown = nullptr;
	//右键弹起
	HandlerUIEvent OnRightMouseUp = nullptr;
	//鼠标移动
	HandlerUIEvent OnMouseOver = nullptr;
	//中键滚动
	HandlerUIEvent OnMouseWheel = nullptr;
	//获得字符
	HandlerUIEvent OnGetChar = nullptr;
	//键盘点击
	HandlerUIEvent OnKeyDown = nullptr;
	static const wstring className;
private:
	void InitWindow();
	LRESULT WindowProcess(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam);
	//对于窗体的处理方法要求是静态的，但可以通过特殊方法处理。
	static LRESULT CALLBACK WndProcess(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam);
	HINSTANCE hInstance = NULL;
	HWND hWnd = NULL;
	double left;
	double top;
	double width;
	double height;
	wstring title;
};

