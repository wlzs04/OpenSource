#pragma once
#include "IUINode.h"

class LLGameCanvas : public IUINode
{
public:
	LLGameCanvas();
	~LLGameCanvas(){};
	bool CheckState()override;
	void Render()override;

	HandleUIEvent OnMouseMove;
	HandleUIEvent OnRender;
private:
};