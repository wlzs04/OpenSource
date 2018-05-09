#pragma once
#include "IUINode.h"

class LLGameButton : public IUINode
{
public:
	LLGameButton();
	~LLGameButton() {};
	void Update()override;
	void Render()override;

	void SetText(wstring text);
private:
	PropertyImage propertyImage;
	PropertyText propertyText;
};
