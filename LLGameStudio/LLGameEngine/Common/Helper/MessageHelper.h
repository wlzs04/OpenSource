#pragma once
#include <string>
#include <windows.h>

using namespace std;

class MessageHelper
{
public:
	//弹出提示框。
	static void ShowMessage(wstring message);
};