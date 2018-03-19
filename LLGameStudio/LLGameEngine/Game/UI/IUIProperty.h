#pragma once
#include "..\..\Common\Helper\MathHelper.h"
#include "..\..\Common\LLBase.h"
#include "../../Common/Graphics/GraphicsApi.h"

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

class PropertyName :public IUIProperty
{
public:
	PropertyName() :IUIProperty(L"name", L"node") {SetValue(defaultValue);}
	wstring GetValue()override { return value; };
	void SetValue(wstring value) { this->value =value; };
	wstring value;
};

class PropertyEnable :public IUIProperty
{
public:
	PropertyEnable() :IUIProperty(L"enable", L"True") { SetValue(defaultValue); }
	wstring GetValue() { return value ? L"True" : L"False"; };
	void SetValue(wstring value) { this->value = WStringHelper::GetBool(value); };
	bool value;
};

class PropertyWidth :public IUIProperty
{
public:
	PropertyWidth() :IUIProperty(L"width", L"1") {SetValue(defaultValue);}
	wstring GetValue() { return to_wstring(value); };
	void SetValue(wstring value) { this->value = WStringHelper::GetFloat(value); };
	float value;
};

class PropertyHeight :public IUIProperty
{
public:
	PropertyHeight() :IUIProperty(L"height", L"1") {SetValue(defaultValue);}
	wstring GetValue() { return to_wstring(value); };
	void SetValue(wstring value) { this->value = WStringHelper::GetFloat(value); };
	float value;
};

class AnchorEnum : public EnumBase
{
public:
	AnchorEnum(int i) :EnumBase(i) {}

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
	unordered_map<wstring, int>& GetEnumMap()override;
};

class PropertyAnchorEnum :public IUIProperty
{
public:
	PropertyAnchorEnum() :IUIProperty(L"anchorEnum", L"Left_Top") {SetValue(defaultValue);}
	wstring GetValue() { return value.ToWString(); };
	void SetValue(wstring value) { this->value.GetValueFromWString(value); };
	AnchorEnum value = AnchorEnum::Left;
};

class PropertyrRotation :public IUIProperty
{
public:
	PropertyrRotation() :IUIProperty(L"rotation", L"{0,0}") {SetValue(defaultValue);}
	wstring GetValue() { return value.ToWString(); };
	void SetValue(wstring value) { this->value.GetValueFromWString(value); };
	Vector2 value;
};

class PropertyrMargin :public IUIProperty
{
public:
	PropertyrMargin() :IUIProperty(L"margin", L"{0}") {SetValue(defaultValue);}
	wstring GetValue() { return value.ToWString(); };
	void SetValue(wstring value) { this->value.GetValueFromWString(value); };
	Rect value;
};

class PropertyrClipByParent :public IUIProperty
{
public:
	PropertyrClipByParent() :IUIProperty(L"clipByParent", L"False") {SetValue(defaultValue);}
	wstring GetValue() { return value ? L"True" : L"False"; };
	void SetValue(wstring value) { this->value=WStringHelper::GetBool(value); };
	bool value;
};

class PropertyFilePath :public IUIProperty
{
public:
	PropertyFilePath() :IUIProperty(L"filePath", L"") { SetValue(defaultValue); }
	wstring GetValue()override { return value; };
	void SetValue(wstring value) { this->value = value; };
	wstring value;
};

class PropertyImage :public IUIProperty
{
public:
	PropertyImage() :IUIProperty(L"image", L"") { SetValue(defaultValue); }
	wstring GetValue()override { return value; };
	void SetValue(wstring value) {
		this->value = value;
		GraphicsApi::GetGraphicsApi()->AddImage(value);
	};
	wstring value;
};

class PropertyText :public IUIProperty
{
public:
	PropertyText() :IUIProperty(L"text", L"") { SetValue(defaultValue); }
	wstring GetValue()override { return value; };
	void SetValue(wstring value) {this->value = value;};
	wstring value;
};