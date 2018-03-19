#include "LLGameButton.h"

LLGameButton::LLGameButton()
{
	propertyMap[propertyImage.name] = &propertyImage;
	propertyMap[propertyText.name] = &propertyText;
}

void LLGameButton::Update()
{
}

void LLGameButton::Render()
{
	if (propertyImage.value != L"")
	{
		GraphicsApi::GetGraphicsApi()->DrawImage(propertyImage.value, actualRect.left, actualRect.top, actualWidth, actualHeight);
	}
	else
	{
		GraphicsApi::GetGraphicsApi()->DrawRect(actualRect.left, actualRect.top, actualWidth, actualHeight);
	}
	GraphicsApi::GetGraphicsApi()->DrawText(propertyText.value, actualRect.left, actualRect.top, actualWidth, actualHeight);
}
