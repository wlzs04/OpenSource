#pragma once

class GameHelper
{
public:
	static void SetGameWidth(float width);
	static void SetGameHeight(float height);
	static float GetGameWidth();
	static float GetGameHeight();

private:
	static float width;
	static float height;
};