#include "LLGameImage.h"

LLGameImage::LLGameImage()
{
	propertyMap[propertyImage.name] = &propertyImage;
}

void LLGameImage::Render()
{
	if (0 < propertyWidth.value && propertyWidth.value <= 1)
	{
		actualWidth = parentNode->GetActualWidth()*propertyWidth.value;
	}
	else
	{
		actualWidth = propertyWidth.value;
	}
	if (0 < propertyHeight.value && propertyHeight.value <= 1)
	{
		actualHeight = parentNode->GetActualHeight()*propertyHeight.value;
	}
	else
	{
		actualHeight = propertyHeight.value;
	}
	if (propertyImage.value != L"")
	{
		GraphicsApi::GetGraphicsApi()->DrawImage(propertyImage.value, 0, 0, actualWidth, actualHeight);
	}
}