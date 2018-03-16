#pragma once
#include "IUINode.h"

class LLGameButton : public IUINode
{
public:
	LLGameButton();
	~LLGameButton() {};
	void Render()override;
private:
	PropertyImage propertyImage;
	PropertyText propertyText;
};
