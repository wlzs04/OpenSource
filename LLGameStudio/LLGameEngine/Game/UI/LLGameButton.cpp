#include "LLGameButton.h"

LLGameButton::LLGameButton()
{
	propertyMap[propertyImage.name] = &propertyImage;
	propertyMap[propertyText.name] = &propertyText;
}

void LLGameButton::Render()
{
	if (propertyImage.value != L"")
	{
		GraphicsApi::GetGraphicsApi()->DrawImage(propertyImage.value, actualLeft, actualTop, actualWidth, actualHeight);
	}
	else
	{
		GraphicsApi::GetGraphicsApi()->DrawRect(actualLeft, actualTop, actualWidth, actualHeight);
	}
	GraphicsApi::GetGraphicsApi()->DrawText(propertyText.value,actualLeft, actualTop, actualWidth, actualHeight);
}
