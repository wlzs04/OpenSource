#pragma once
#include "IUINode.h"

class LLGameBack : public IUINode
{
public:
	LLGameBack();
	~LLGameBack() {};
	void Render()override;
private:
	PropertyImage propertyImage;
};