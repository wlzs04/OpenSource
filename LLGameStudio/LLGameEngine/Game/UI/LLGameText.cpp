#include "LLGameText.h"

LLGameText::LLGameText()
{
	propertyMap[propertyText.name] = &propertyText;
}

void LLGameText::Render()
{
	GraphicsApi::GetGraphicsApi()->DrawText(propertyText.value, actualLeft, actualTop, actualWidth, actualHeight);
}
