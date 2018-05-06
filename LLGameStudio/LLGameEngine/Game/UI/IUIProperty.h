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

//UI节点属性

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

class PropertyRotation :public IUIProperty
{
public:
	PropertyRotation() :IUIProperty(L"rotation", L"{0,0}") {SetValue(defaultValue);}
	wstring GetValue() { return value.ToWString(); };
	void SetValue(wstring value) { this->value.GetValueFromWString(value); };
	Vector2 value;
};

class PropertyMargin :public IUIProperty
{
public:
	PropertyMargin() :IUIProperty(L"margin", L"{0}") {SetValue(defaultValue);}
	wstring GetValue() { return value.ToWString(); };
	void SetValue(wstring value) { this->value.GetValueFromWString(value); };
	Rect value;
};

class PropertyClipByParent :public IUIProperty
{
public:
	PropertyClipByParent() :IUIProperty(L"clipByParent", L"False") {SetValue(defaultValue);}
	wstring GetValue() { return value ? L"True" : L"False"; };
	void SetValue(wstring value) { this->value=WStringHelper::GetBool(value); };
	bool value;
};

class CheckStateMethod : public EnumBase
{
public:
	CheckStateMethod(int i) :EnumBase(i) {}

	static const int Rect = 0;
	static const int Alpha = 1;
	static const int AllowMouseThrough = 2;
private:
	unordered_map<wstring, int>& GetEnumMap()override;
};

class PropertyCheckStateMethod :public IUIProperty
{
public:
	PropertyCheckStateMethod() :IUIProperty(L"checkStateMethod", L"Rect") { SetValue(defaultValue); }
	wstring GetValue() { return value.ToWString(); };
	void SetValue(wstring value) { this->value.GetValueFromWString(value); };
	CheckStateMethod value = CheckStateMethod::Rect;
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

class PropertyModal :public IUIProperty
{
public:
	PropertyModal() :IUIProperty(L"modal", L"False") { SetValue(defaultValue); }
	wstring GetValue() { return value ? L"True" : L"False"; };
	void SetValue(wstring value) { this->value = WStringHelper::GetBool(value); };
	bool value;
};

//粒子系统属性

class ParticleType : public EnumBase
{
public:
	ParticleType(int i) :EnumBase(i) {}

	static const int Point = 0;//点
	static const int Star = 1;//星
	static const int Image = 2;//图片
	static const int Sequence = 3;//序列图
private:
	unordered_map<wstring, int>& GetEnumMap()override;
};

class PropertyParticleType :public IUIProperty
{
public:
	PropertyParticleType() :IUIProperty(L"particleType", L"Point") { SetValue(defaultValue); }
	wstring GetValue() { return value.ToWString(); };
	void SetValue(wstring value) { this->value.GetValueFromWString(value); };
	ParticleType value = ParticleType::Point;
};

class PropertyIsLoop :public IUIProperty
{
public:
	PropertyIsLoop() :IUIProperty(L"isLoop", L"True") { SetValue(defaultValue); }
	wstring GetValue() { return value ? L"True" : L"False"; };
	void SetValue(wstring value) { this->value = WStringHelper::GetBool(value); };
	bool value;
};

class PropertyLoopTime :public IUIProperty
{
public:
	PropertyLoopTime() :IUIProperty(L"loopTime", L"5") { SetValue(defaultValue); }
	wstring GetValue() { return to_wstring(value); };
	void SetValue(wstring value) { this->value = WStringHelper::GetFloat(value); };
	float value;
};

class PropertyMaxNumber :public IUIProperty
{
public:
	PropertyMaxNumber() :IUIProperty(L"maxNumber", L"10") { SetValue(defaultValue); }
	wstring GetValue() { return to_wstring(value); };
	void SetValue(wstring value) { this->value = WStringHelper::GetInt(value); };
	int value;
};

class PropertyStartNumber :public IUIProperty
{
public:
	PropertyStartNumber() :IUIProperty(L"startNumber", L"1") { SetValue(defaultValue); }
	wstring GetValue() { return to_wstring(value); };
	void SetValue(wstring value) { this->value = WStringHelper::GetInt(value); };
	int value;
};

class PropertyCreateNumberBySecond :public IUIProperty
{
public:
	PropertyCreateNumberBySecond() :IUIProperty(L"createNumberBySecond", L"2") { SetValue(defaultValue); }
	wstring GetValue() { return to_wstring(value); };
	void SetValue(wstring value) { this->value = WStringHelper::GetInt(value); };
	int value;
};

class PropertyRadius :public IUIProperty
{
public:
	PropertyRadius() :IUIProperty(L"radius", L"3") { SetValue(defaultValue); }
	wstring GetValue() { return to_wstring(value); };
	void SetValue(wstring value) { this->value = WStringHelper::GetFloat(value); };
	float value;
};

class PropertyRadiusError :public IUIProperty
{
public:
	PropertyRadiusError() :IUIProperty(L"radiusError", L"1") { SetValue(defaultValue); }
	wstring GetValue() { return to_wstring(value); };
	void SetValue(wstring value) { this->value = WStringHelper::GetFloat(value); };
	float value;
}; 

class PropertyColor :public IUIProperty
{
public:
	PropertyColor() :IUIProperty(L"color", L"#FFFFFFFF") { SetValue(defaultValue); }
	wstring GetValue() { return value; };
	void SetValue(wstring value) { this->value = value; };
	wstring value;
};

class PropertyVelocity :public IUIProperty
{
public:
	PropertyVelocity() :IUIProperty(L"velocity", L"20") { SetValue(defaultValue); }
	wstring GetValue() { return to_wstring(value); };
	void SetValue(wstring value) { this->value = WStringHelper::GetFloat(value); };
	float value;
}; 

class PropertyVelocityError :public IUIProperty
{
public:
	PropertyVelocityError() :IUIProperty(L"velocityError", L"2") { SetValue(defaultValue); }
	wstring GetValue() { return to_wstring(value); };
	void SetValue(wstring value) { this->value = WStringHelper::GetFloat(value); };
	float value;
};

class PropertyDirection :public IUIProperty
{
public:
	PropertyDirection() :IUIProperty(L"direction", L"{0,-1}") { SetValue(defaultValue); }
	wstring GetValue() { return value.ToWString(); };
	void SetValue(wstring value) { this->value.GetValueFromWString(value); };
	Vector2 value;
};

class PropertyAngleRange :public IUIProperty
{
public:
	PropertyAngleRange() :IUIProperty(L"angleRange", L"30") { SetValue(defaultValue); }
	wstring GetValue() { return to_wstring(value); };
	void SetValue(wstring value) { this->value = WStringHelper::GetFloat(value); };
	float value;
};

class PropertyPosition :public IUIProperty
{
public:
	PropertyPosition() :IUIProperty(L"position", L"{0,0}") { SetValue(defaultValue); }
	wstring GetValue() { return value.ToWString(); };
	void SetValue(wstring value) { this->value.GetValueFromWString(value); };
	Vector2 value;
};

class PropertyPositionError :public IUIProperty
{
public:
	PropertyPositionError() :IUIProperty(L"positionError", L"{0,0}") { SetValue(defaultValue); }
	wstring GetValue() { return value.ToWString(); };
	void SetValue(wstring value) { this->value.GetValueFromWString(value); };
	Vector2 value;
};

class PropertyImagePath :public IUIProperty
{
public:
	PropertyImagePath() :IUIProperty(L"imagePath", L"") { SetValue(defaultValue); }
	wstring GetValue()override { return value; };
	void SetValue(wstring value) {
		this->value = value;
		GraphicsApi::GetGraphicsApi()->AddImage(value);
	};
	wstring value;
};

class PropertyRow :public IUIProperty
{
public:
	PropertyRow() :IUIProperty(L"row", L"1") { SetValue(defaultValue); }
	wstring GetValue() { return to_wstring(value); };
	void SetValue(wstring value) { this->value = WStringHelper::GetInt(value); };
	int value;
};

class PropertyColumn :public IUIProperty
{
public:
	PropertyColumn() :IUIProperty(L"column", L"1") { SetValue(defaultValue); }
	wstring GetValue() { return to_wstring(value); };
	void SetValue(wstring value) { this->value = WStringHelper::GetInt(value); };
	int value;
};

//骨骼系统属性

class PropertyLength :public IUIProperty
{
public:
	PropertyLength() :IUIProperty(L"length", L"90") { SetValue(defaultValue); }
	wstring GetValue() { return to_wstring(value); };
	void SetValue(wstring value) { this->value = WStringHelper::GetFloat(value); };
	float value;
};

class PropertyAngle :public IUIProperty
{
public:
	PropertyAngle() :IUIProperty(L"angle", L"0") { SetValue(defaultValue); }
	wstring GetValue() { return to_wstring(value); };
	void SetValue(wstring value) { this->value = WStringHelper::GetFloat(value); };
	float value;
};

class PropertyFrameNumber :public IUIProperty
{
public:
	PropertyFrameNumber() :IUIProperty(L"frameNumber", L"0") { SetValue(defaultValue); }
	wstring GetValue() { return to_wstring(value); };
	void SetValue(wstring value) { this->value = WStringHelper::GetInt(value); };
	int value;
};

class PropertyTotalFrameNumber :public IUIProperty
{
public:
	PropertyTotalFrameNumber() :IUIProperty(L"totalFrameNumber", L"24") { SetValue(defaultValue); }
	wstring GetValue() { return to_wstring(value); };
	void SetValue(wstring value) { this->value = WStringHelper::GetInt(value); };
	int value;
};

class PropertyTotalTime :public IUIProperty
{
public:
	PropertyTotalTime() :IUIProperty(L"totalTime", L"4") { SetValue(defaultValue); }
	wstring GetValue() { return to_wstring(value); };
	void SetValue(wstring value) { this->value = WStringHelper::GetFloat(value); };
	float value;
};

//游戏配置属性

class PropertyGameName :public IUIProperty
{
public:
	PropertyGameName() :IUIProperty(L"gameName", L"游戏") { SetValue(defaultValue); }
	wstring GetValue()override { return value; };
	void SetValue(wstring value) { this->value = value; };
	wstring value;
};

class PropertyGameWidth :public IUIProperty
{
public:
	PropertyGameWidth() :IUIProperty(L"gameWidth", L"800") { SetValue(defaultValue); }
	wstring GetValue() { return to_wstring(value); };
	void SetValue(wstring value) { this->value = WStringHelper::GetFloat(value); };
	float value;
};

class PropertyGameHeight :public IUIProperty
{
public:
	PropertyGameHeight() :IUIProperty(L"gameHeight", L"600") { SetValue(defaultValue); }
	wstring GetValue() { return to_wstring(value); };
	void SetValue(wstring value) { this->value = WStringHelper::GetFloat(value); };
	float value;
};

class PropertyGameLeft :public IUIProperty
{
public:
	PropertyGameLeft() :IUIProperty(L"gameLeft", L"0") { SetValue(defaultValue); }
	wstring GetValue() { return to_wstring(value); };
	void SetValue(wstring value) { this->value = WStringHelper::GetFloat(value); };
	float value;
};

class PropertyGameTop :public IUIProperty
{
public:
	PropertyGameTop() :IUIProperty(L"gameTop", L"0") { SetValue(defaultValue); }
	wstring GetValue() { return to_wstring(value); };
	void SetValue(wstring value) { this->value = WStringHelper::GetFloat(value); };
	float value;
};

class PropertyMiddleInScreen :public IUIProperty
{
public:
	PropertyMiddleInScreen() :IUIProperty(L"middleInScreen", L"True") { SetValue(defaultValue); }
	wstring GetValue() { return value ? L"True" : L"False"; };
	void SetValue(wstring value) { this->value = WStringHelper::GetBool(value); };
	bool value;
};

class PropertyFullScreen :public IUIProperty
{
public:
	PropertyFullScreen() :IUIProperty(L"fullScreen", L"False") { SetValue(defaultValue); }
	wstring GetValue() { return value ? L"True" : L"False"; };
	void SetValue(wstring value) { this->value = WStringHelper::GetBool(value); };
	bool value;
};

class PropertyCanMultiGame :public IUIProperty
{
public:
	PropertyCanMultiGame() :IUIProperty(L"canMultiGame", L"False") { SetValue(defaultValue); }
	wstring GetValue() { return value ? L"True" : L"False"; };
	void SetValue(wstring value) { this->value = WStringHelper::GetBool(value); };
	bool value;
};

class PropertyStartScene :public IUIProperty
{
public:
	PropertyStartScene() :IUIProperty(L"startScene", L"layout/StartScene.scene") { SetValue(defaultValue); }
	wstring GetValue()override { return value; };
	void SetValue(wstring value) { this->value = value; };
	wstring value;
};

class GraphicsApiType : public EnumBase
{
public:
	GraphicsApiType(int i) :EnumBase(i) {}

	static const int Direct2D = 0;//Direct2D
	static const int LL2D = 1;//未实现
private:
	unordered_map<wstring, int>& GetEnumMap()override;
};

class PropertyGraphicsApi :public IUIProperty
{
public:
	PropertyGraphicsApi() :IUIProperty(L"graphicsApi", L"Direct2D") { SetValue(defaultValue); }
	wstring GetValue() { return value.ToWString(); };
	void SetValue(wstring value) { this->value.GetValueFromWString(value); };
	GraphicsApiType value = GraphicsApiType::Direct2D;
};

class PropertyOpenNetClient :public IUIProperty
{
public:
	PropertyOpenNetClient() :IUIProperty(L"openNetClient", L"False") { SetValue(defaultValue); }
	wstring GetValue() { return value ? L"True" : L"False"; };
	void SetValue(wstring value) { this->value = WStringHelper::GetBool(value); };
	bool value;
};

class PropertyOpenPhysics :public IUIProperty
{
public:
	PropertyOpenPhysics() :IUIProperty(L"openPhysics", L"False") { SetValue(defaultValue); }
	wstring GetValue() { return value ? L"True" : L"False"; };
	void SetValue(wstring value) { this->value = WStringHelper::GetBool(value); };
	bool value;
};

class PropertyIcon :public IUIProperty
{
public:
	PropertyIcon() :IUIProperty(L"icon", L"") { SetValue(defaultValue); }
	wstring GetValue()override { return value; };
	void SetValue(wstring value) { this->value = value; };
	wstring value;
};

class PropertyDefaultCursor :public IUIProperty
{
public:
	PropertyDefaultCursor() :IUIProperty(L"defaultCursor", L"") { SetValue(defaultValue); }
	wstring GetValue()override { return value; };
	void SetValue(wstring value) { this->value = value; };
	wstring value;
};

class PropertyServerIPPort :public IUIProperty
{
public:
	PropertyServerIPPort() :IUIProperty(L"serverIPPort", L"") { SetValue(defaultValue); }
	wstring GetValue()override { return value; };
	void SetValue(wstring value) { this->value = value; };
	wstring value;
};