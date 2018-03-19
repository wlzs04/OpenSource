#include "GameHelper.h"

float GameHelper::width = 1;
float GameHelper::height = 1;
float GameHelper::thisTickTime = 0;
IUINode* GameHelper::currentUINode = nullptr;
POINT GameHelper::mousePosition;
bool GameHelper::mouseLeftButtonPassed;
bool GameHelper::mouseRightButtonPassed;
int GameHelper::mouseWheelValue;

bool GameHelper::IsPointInRect(Rect rect)
{
	return mousePosition.x<rect.right
		&&mousePosition.x>rect.left
		&&mousePosition.y<rect.bottom
		&&mousePosition.y>rect.top;
}
