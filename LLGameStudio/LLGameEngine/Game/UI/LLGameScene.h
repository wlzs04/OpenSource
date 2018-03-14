#pragma once
#include "LLGameBack.h"
#include "..\..\Common\XML\LLXML.h"

class PropertyFilePath :public IUIProperty
{
public:
	PropertyFilePath() :IUIProperty(L"filePath", L"") {}
	wstring GetValue()override { return value; };
	void SetValue(wstring value) { this->value = value; };
	wstring value;
};

class LLGameScene : IUINode
{
public:
	LLGameScene();
	~LLGameScene() {};
	void LoadSceneFromFile(wstring filePath);
	void Render()override;
private:
	PropertyFilePath propertyFilePath;
};