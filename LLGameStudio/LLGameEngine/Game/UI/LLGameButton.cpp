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
	if (propertyEnable.value)
	{
		if (propertyImage.value != L"")
		{
			GraphicsApi::GetGraphicsApi()->DrawImage(propertyImage.value, actualRect.left, actualRect.top, actualWidth, actualHeight);
		}
		else
		{
			GraphicsApi::GetGraphicsApi()->DrawRect(true,actualRect.left, actualRect.top, actualWidth, actualHeight);
		}
		GraphicsApi::GetGraphicsApi()->DrawText(propertyText.value, actualRect.left, actualRect.top, actualWidth, actualHeight);
	}
}
