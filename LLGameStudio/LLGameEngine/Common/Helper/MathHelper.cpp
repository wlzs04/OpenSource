#include "MathHelper.h"

bool MathHelper::IsRange1To0(double d)
{
	return d > 0 && d <= 1;
}

bool MathHelper::IsPointInRect(Vector2 vector2, Rect rect)
{
	return vector2.x<rect.right
		&&vector2.x>rect.left
		&&vector2.y<rect.bottom
		&&vector2.y>rect.top;
}

int MathHelper::RoundFloat(float f)
{
	return f+0.5f;
}
