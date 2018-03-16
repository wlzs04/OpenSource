#pragma once
#include "IUINode.h"

class LLGameText : public IUINode
{
public:
	LLGameText();
	~LLGameText() {};
	void Render()override;
private:
	PropertyText propertyText;
};
