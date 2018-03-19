#pragma once
#include <windows.h>
#include "MathHelper.h"

class IUINode;

class GameHelper
{
public:
	static bool IsPointInRect(Rect rect);

	static float width;
	static float height;
	static float thisTickTime; 
	static IUINode* currentUINode;
	static POINT mousePosition;
	static bool mouseLeftButtonPassed;
	static bool mouseRightButtonPassed;
	static int mouseWheelValue;
};