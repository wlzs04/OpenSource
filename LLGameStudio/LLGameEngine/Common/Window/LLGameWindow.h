#pragma once
#include <windows.h>
#include <windowsx.h>
#include <string>
#include <unordered_map>
#include <functional>

using namespace std;

typedef function<void()> HandleEvent;
typedef function<void(void*,int)> HandleUIEvent;

class LLGameWindow
{
public:
	LLGameWindow();
	~LLGameWindow();
	HWND GetHWND();
	void Run();
	void SetPosition(double left,double top);
	void SetSize(double width, double height);
	void SetTitle(wstring title);
	void SetMaximize();
	void SetMinimize();
	void AddHandleUIEvent(wstring eventName, HandleUIEvent handleUIEvent);
	void RemoveHandleUIEvent(wstring eventName);
	static const wstring className;
	//开始事件
	HandleEvent OnBeginEvent = nullptr;
	//运行事件(每帧执行一次)
	HandleEvent OnRunEvent = nullptr;
	//结束事件
	HandleEvent OnEndEvent = nullptr;
	//获得焦点
	HandleEvent OnActivate = nullptr;
	//失去焦点
	HandleEvent OnInActivate = nullptr;
	//左键点击
	HandleUIEvent OnLeftMouseDown = nullptr;
	//左键弹起
	HandleUIEvent OnLeftMouseUp = nullptr;
	//右键点击
	HandleUIEvent OnRightMouseDown = nullptr;
	//右键弹起
	HandleUIEvent OnRightMouseUp = nullptr;
	//鼠标移动
	HandleUIEvent OnMouseOver = nullptr;
	//中键滚动
	HandleUIEvent OnMouseWheel = nullptr;
	//获得字符
	HandleUIEvent OnGetChar = nullptr;
	//键盘点击
	HandleUIEvent OnKeyDown = nullptr;
private:
	void InitWindow();
	LRESULT WindowProcess(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam);
	//对于窗体的处理方法要求是静态的，但可以通过特殊方法处理。
	static LRESULT CALLBACK WndProcess(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam);

	HWND hWnd = NULL;
	double left;
	double top;
	double width;
	double height;
	wstring title;
	unordered_map<wstring, HandleUIEvent> eventMap;
};

