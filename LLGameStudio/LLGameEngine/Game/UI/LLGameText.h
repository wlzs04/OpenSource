#pragma once
#include "IUINode.h"

class LLGameText : public IUINode
{
public:
	LLGameText();
	~LLGameText() {};
	void SetText(wstring text);
	void Render()override;
private:
	PropertyText propertyText;
};
