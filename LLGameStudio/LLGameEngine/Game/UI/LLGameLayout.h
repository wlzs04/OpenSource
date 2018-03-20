#pragma once
#include "LLGameGrid.h"

class LLGameLayout : public IUINode
{
public:
	LLGameLayout();
	~LLGameLayout() {};
	void Render()override;
	void SetProperty(wstring name, wstring value)override;
	virtual bool CheckState()override;
	void LoadLayoutFromFile(wstring filePath);

private:
	PropertyFilePath propertyFilePath;
	PropertyModal propertyModal;
	LLGameGrid* uiGrid;
};