#include "LLGameText.h"

LLGameText::LLGameText()
{
	propertyMap[propertyText.name] = &propertyText;
}

void LLGameText::SetText(wstring text)
{
	propertyText.value = text;
}

void LLGameText::Render()
{
	GraphicsApi::GetGraphicsApi()->DrawText(propertyText.value, actualRect.left, actualRect.top, actualWidth, actualHeight);
}
