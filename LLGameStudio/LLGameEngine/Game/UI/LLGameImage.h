#pragma once
#include "IUINode.h"

class LLGameImage : public IUINode
{
public:
	LLGameImage();
	~LLGameImage() {};
	void Render()override;
private:
	PropertyImage propertyImage;
};