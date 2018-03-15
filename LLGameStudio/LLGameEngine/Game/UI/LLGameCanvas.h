#pragma once
#include "IUINode.h"

class LLGameCanvas : public IUINode
{
public:
	LLGameCanvas();
	~LLGameCanvas(){};
	void Render()override;
private:
};