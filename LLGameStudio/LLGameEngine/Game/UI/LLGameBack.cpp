#include "LLGameBack.h"

LLGameBack::LLGameBack()
{
	propertyMap[propertyImage.name] = &propertyImage;
}

void LLGameBack::Render()
{
	if (propertyImage.value != L"")
	{
		GraphicsApi::GetGraphicsApi()->DrawImage(propertyImage.value, actualRect.left, actualRect.top, actualWidth, actualHeight);
	}
}
