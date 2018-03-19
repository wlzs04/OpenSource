﻿#pragma once
#include <string>
#include "WStringHelper.h"

using namespace std;

struct Vector2
{
public:
	wstring ToWString()
	{
		return L"{" + to_wstring(x) + L"," + to_wstring(y) + L"}";
	}

	void GetValueFromWString(wstring ws)
	{
		if (ws.size() > 4)
		{
			ws = ws.substr(1, ws.size() - 1);
			vector<wstring> vc;
			WStringHelper::Split(ws, L',', vc);
			if (vc.size() > 1)
			{
				x = WStringHelper::GetFloat(vc[0]);
				y = WStringHelper::GetFloat(vc[1]);
			}
		}
	}
	float x = 0, y = 0;
};

struct Rect
{
public:
	wstring ToWString()
	{
		if (left == top && left == right && left == bottom)
		{
			return L"{" + to_wstring(left) + L"}";
		}
		else if (left == right && top == bottom)
		{
			return L"{" + to_wstring(left) + L"," + to_wstring(top) + L"}";
		}
		return L"{" + to_wstring(left) + L"," + to_wstring(top) + L"," + to_wstring(right) + L"," + to_wstring(bottom) + L"}";
	}

	void GetValueFromWString(wstring ws)
	{
		if (ws.size() > 2)
		{
			ws = ws.substr(1, ws.size() - 1);
			vector<wstring> vc;
			WStringHelper::Split(ws, L',', vc);
			if (vc.size() > 3)
			{
				left = WStringHelper::GetFloat(vc[0]);
				top = WStringHelper::GetFloat(vc[1]);
				right = WStringHelper::GetFloat(vc[2]);
				bottom = WStringHelper::GetFloat(vc[3]);
			}
			else if (vc.size() == 2)
			{
				left = WStringHelper::GetFloat(vc[0]);
				top = WStringHelper::GetFloat(vc[1]);
				right = left;
				bottom = top;
			}
			else if (vc.size() == 1)
			{
				left = WStringHelper::GetFloat(vc[0]);
				top = left;
				right = left;
				bottom = left;
			}
		}
	}

	float left = 0, top = 0, right = 0, bottom = 0;
};

class MathHelper
{
public:
	//判断参数是否在1和0之间，包括1不包括0。
	static bool IsRange1To0(double d);
	//判断一点是否在矩形中。
	static bool IsPointInRect(Vector2 vector2, Rect rect);
	//返回浮点数的四舍五入值。
	static int RoundFloat(float f);
};
