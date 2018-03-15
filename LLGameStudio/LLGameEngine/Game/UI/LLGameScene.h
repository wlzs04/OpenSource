#pragma once
#include "LLGameBack.h"
#include "LLGameCanvas.h"
#include "LLGameLayout.h"
#include "..\..\Common\XML\LLXML.h"

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