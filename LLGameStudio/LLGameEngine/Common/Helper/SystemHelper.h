#pragma once
#include <windows.h>
#include <string>

using namespace std;

class SystemHelper
{
public:
	//获得屏幕宽度。
	static int GetScreenWidth();
	//获得屏幕高度。
	static int GetScreenHeight();
	//获得当前路径。
	static wstring GetCurrentPath();
	//获得资源路径。
	static wstring GetResourceRootPath();
	//资源文件夹名
	static wstring resourcePath;
	//设置鼠标在屏幕上的位置
	static void SetCursorPosition(int x, int y);
	//无参数时设置鼠标位置到屏幕中心
	static void SetCursorCenter();
	//获得鼠标在屏幕上的位置
	static POINT GetCursorPosition();
};