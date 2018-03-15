#pragma once
#include "IUINode.h"

class LLGameGrid : public IUINode
{
public:
	LLGameGrid();
	~LLGameGrid() {};
	void Render() override;
private:
};