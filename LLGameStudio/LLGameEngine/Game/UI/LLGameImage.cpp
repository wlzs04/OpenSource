#include "LLGameImage.h"

LLGameImage::LLGameImage()
{
	propertyMap[propertyImage.name] = &propertyImage;
}

void LLGameImage::Render()
{
	if (propertyImage.value != L"")
	{
		GraphicsApi::GetGraphicsApi()->DrawImage(propertyImage.value, actualLeft, actualTop, actualWidth, actualHeight);
	}
}