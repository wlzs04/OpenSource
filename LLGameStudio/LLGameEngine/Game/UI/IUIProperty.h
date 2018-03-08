#pragma once
#include <string>
#include "..\..\Common\Helper\WStringHelper.h"
#include "..\..\Common\LLBase.h"

using namespace std;

class IUIProperty
{
public:
	IUIProperty(wstring name, wstring defaultValue);

	bool IsDefault();

	virtual wstring GetValue() { return wstring(); };
	virtual void SetValue(wstring) {};

	wstring name;
	wstring defaultValue;
};

class PropertyName :IUIProperty
{
public:
	PropertyName() :IUIProperty(L"name", L"node") {}
	wstring GetValue()override { return value; };
	void SetValue(wstring value) { this->value =value; };
	wstring value;
};

class PropertyWidth :IUIProperty
{
public:
	PropertyWidth() :IUIProperty(L"width", L"1") {}
	wstring GetValue() { return to_wstring(value); };
	void SetValue(wstring value) { this->value = WStringHelper::GetFloat(value); };
	float value;
};

class PropertyHeight :IUIProperty
{
public:
	PropertyHeight() :IUIProperty(L"height", L"1") {}
	wstring GetValue() { return to_wstring(value); };
	void SetValue(wstring value) { this->value = WStringHelper::GetFloat(value); };
	float value;
};

class AnchorEnum : public EnumBase
{
public:
	AnchorEnum(int i) :EnumBase(i) {}

	unordered_map<wstring, int>& GetEnumMap()override;

	static const int Center = 0;
	static const int Left = 1;
	static const int Top = 2;
	static const int Right = 4;
	static const int Bottom = 8;
	static const int Left_Top = Left | Top;
	static const int Right_Top = Right | Top;
	static const int Right_Bottom = Right | Bottom;
	static const int Left_Bottom = Left | Bottom;
private:
	static unordered_map<wstring, int> anchorEnumMap;
};

class PropertyAnchorEnum :IUIProperty
{
public:
	PropertyAnchorEnum() :IUIProperty(L"anchorEnum", L"Left_Top") {}
	wstring GetValue() { return value.ToWString(); };
	void SetValue(wstring value) { this->value.GetValueFromWString(value); };
	AnchorEnum value = AnchorEnum::Left;
};

struct Vector2
{
public :
	wstring ToWString()
	{
		return L"{" + to_wstring(x) + L"," + to_wstring(y) + L"}";
	}

	void GetValueFromWString(wstring ws)
	{
		if (ws.size() > 4)
		{
			ws=ws.substr(1, ws.size() - 1);
			vector<wstring> vc;
			WStringHelper::Split(ws,L',',vc);
			if (vc.size() > 1)
			{
				x = WStringHelper::GetFloat(vc[0]);
				y = WStringHelper::GetFloat(vc[1]);
			}
		}
	}

	float x=0, y=0;
};

class PropertyrRotation :IUIProperty
{
public:
	PropertyrRotation() :IUIProperty(L"rotation", L"{0,0}") {}
	wstring GetValue() { return value.ToWString(); };
	void SetValue(wstring value) { this->value.GetValueFromWString(value); };
	Vector2 value;
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
			else if (vc.size()== 1)
			{
				left = WStringHelper::GetFloat(vc[0]);
				top = left;
				right = left;
				bottom = left;
			}
		}
	}

	float left=0, top = 0, right = 0, bottom = 0;
};

class PropertyrMargin :IUIProperty
{
public:
	PropertyrMargin() :IUIProperty(L"margin", L"{0}") {}
	wstring GetValue() { return value.ToWString(); };
	void SetValue(wstring value) { this->value.GetValueFromWString(value); };
	Rect value;
};

class PropertyrClipByParent :IUIProperty
{
public:
	PropertyrClipByParent() :IUIProperty(L"clipByParent", L"False") {}
	wstring GetValue() { return value ? L"True" : L"False"; };
	void SetValue(wstring value) { this->value=WStringHelper::GetBool(value); };
	bool value;
};