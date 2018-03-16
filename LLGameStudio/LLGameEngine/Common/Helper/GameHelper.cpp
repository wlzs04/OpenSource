#include "GameHelper.h"

float GameHelper::width = 1;
float GameHelper::height = 1;

void GameHelper::SetGameWidth(float width)
{
	GameHelper::width = width;
}

void GameHelper::SetGameHeight(float height)
{
	GameHelper::height = height;
}

float GameHelper::GetGameWidth()
{
	return width;
}

float GameHelper::GetGameHeight()
{
	return height;
}
