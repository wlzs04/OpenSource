#pragma once
#include "LLGameGrid.h"

class LLGameLayout : public IUINode
{
public:
	LLGameLayout();
	~LLGameLayout() {};
	void Render()override;
	void SetProperty(wstring name, wstring value)override;
private:
	void LoadLayoutFromFile(wstring filePath);
	PropertyFilePath propertyFilePath;
	LLGameGrid* uiGrid;
};