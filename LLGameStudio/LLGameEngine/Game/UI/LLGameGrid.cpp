#include "LLGameGrid.h"

LLGameGrid::LLGameGrid()
{
}

void LLGameGrid::Render()
{
	for (auto var : listNode)
	{
		var->Render();
	}
}
