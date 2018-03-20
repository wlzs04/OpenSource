#include "LLGameCanvas.h"

LLGameCanvas::LLGameCanvas()
{
}

bool LLGameCanvas::CheckState()
{
	for (auto rNode = listNode.rbegin(); rNode != listNode.rend(); rNode++)
	{
		(*rNode)->CheckState();
	}
	if (propertyEnable.value)
	{
		if (GameHelper::IsPointInRect(actualRect))
		{
			if (GameHelper::mouseLeftButtonPassed && !uiLock)
			{
				if (uiState != UIState::Click)
				{
					uiState = UIState::Click;
					if (OnMouseClick)
					{
						OnMouseClick(this, 0);
					}
					uiLock = true;
				}
			}
			else
			{
				if (uiState != UIState::Hovor)
				{
					uiState = UIState::Hovor;
					if (OnMouseEnter)
					{
						OnMouseEnter(this, 0);
					}
				}
			}
			if (OnMouseMove)
			{
				OnMouseMove(this, 0);
			}
			return true;
		}
		else
		{
			if (uiState == UIState::Hovor)
			{
				uiState = UIState::Normal;
				if (OnMouseLeave)
				{
					OnMouseLeave(this, 0);
				}
			}
			return false;
		}
	}
	return false;
}

void LLGameCanvas::Render()
{
	if (propertyEnable.value)
	{
		if (OnRender)
		{
			OnRender(this, 0);
		}
	}
}
