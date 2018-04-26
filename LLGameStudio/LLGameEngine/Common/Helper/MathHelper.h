#pragma once
#include <string>
#include "WStringHelper.h"

using namespace std;

struct Vector2
{
public:
	Vector2() {}
	Vector2(float px, float py)
	{
		x = px;
		y = py;
	}

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

	Vector2 operator +(const Vector2& v2)
	{
		Vector2 v1;
		v1.x = x + v2.x;
		v1.y = y + v2.y;
		return v1;
	};

	Vector2 operator -(const Vector2& v2)
	{
		Vector2 v1;
		v1.x = x - v2.x;
		v1.y = y - v2.y;
		return v1;
	};

	Vector2 operator *(const float& i)
	{
		Vector2 v1;
		v1.x = x * i;
		v1.y = y * i;
		return v1;
	};

	Vector2 operator /(const float& i)
	{
		Vector2 v1;
		v1.x = x / i;
		v1.y = y / i;
		return v1;
	};

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
	//获得两点之间的距离。
	static float GetLengthBetweenPoints(Vector2 p1, Vector2 p2);

	//GJK 算法 https://blog.csdn.net/heyuchang666/article/details/55192932
	
	//获得单位向量
	static Vector2 GetNormalVector2(Vector2 v1);
	//获得向量的长度
	static float GetVector2Length(Vector2 v1);
	//获得点沿某方向移动指定距离后的位置
	static Vector2 GetPointMoveByVelocityAndLength(Vector2 point,Vector2 velocity,float length);
	//获得方向经过以某方向为镜面的反射方向
	static Vector2 GetReflectByPlainVector2(Vector2 v1,Vector2 v2);
	//获得方向经过以某方向为法向量的反射方向
	static Vector2 GetReflectByNormalVector2(Vector2 v1, Vector2 v2);
	//获得两向量点乘
	static float GetDotMultiply(Vector2 v1, Vector2 v2);
	//将单位向量扩大到指定长度
	static Vector2 GetVector2SetLength(Vector2 v1, float length);
	//获得以v1方向为轴，映射长度为length的真正标准轴向量
	static Vector2 GetRealVector2ByMapVector2AndLength(Vector2 v1, float length);
	//获得某向量经过某点的法向量
	static Vector2 GetNormalVector2AndPoint(Vector2 v1);
};
