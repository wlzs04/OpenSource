#pragma once
#include "IUINode.h"

class PropertyImage :public IUIProperty
{
public:
	PropertyImage() :IUIProperty(L"image", L"") {}
	wstring GetValue()override { return value; };
	void SetValue(wstring value) { this->value = value; };
	wstring value;
};

class LLGameBack : public IUINode
{
public:
	LLGameBack();
	~LLGameBack() {};
	void Render()override;
private:
	PropertyImage propertyImage;
};