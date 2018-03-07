#pragma once
#include <string>
#include "..\..\Common\Helper\WStringHelper.h"
using namespace std;

class IUIProperty
{
public:
	IUIProperty(wstring name, wstring defaultValue)
	{
		this->name = name;
		this->defaultValue = defaultValue;
	}

	bool IsDefault()
	{
		return defaultValue == GetValue();
	}

	virtual wstring GetValue() = 0;
	virtual void SetValue(wstring) = 0;

	wstring name;
	wstring defaultValue;
};

class PropertyName :IUIProperty
{
public:
	PropertyName() :IUIProperty(L"name", L"node") {}
	wstring GetValue() { return value; };
	void SetValue(wstring value) { this->value =value; };
	wstring value;
};

class PropertyWidth :IUIProperty
{
public:
	PropertyWidth() :IUIProperty(L"width", L"1") {}
	wstring GetValue() { to_wstring(value); };
	void SetValue(wstring value) { this->value = WStringHelper::GetFloat(value); };
	float value;
};

class PropertyHeight :IUIProperty
{
public:
	PropertyHeight() :IUIProperty(L"height", L"1") {}
	wstring GetValue() { to_wstring(value); };
	void SetValue(wstring value) { this->value = WStringHelper::GetFloat(value); };
	float value;
};


enum class AnchorEnum
{
	Center = 0,
	Left = 1,
	Top = 2,
	Right = 4,
	Bottom = 8,
	Left_Top = Left | Top,
	Right_Top = Right | Top,
	Right_Bottom = Right | Bottom,
	Left_Bottom = Left | Bottom,
};

const char* AnchorEnumMatch[]
= {
	"Center",


}
class PropertyAnchorEnum :IUIProperty
{
public:
	PropertyAnchorEnum() :IUIProperty(L"anchorEnum", L"1") {}
	wstring GetValue() { to_wstring(value); };
	void SetValue(wstring value) { this->value = WStringHelper::GetFloat(value); };
	GameUIAnchorEnum value;
};