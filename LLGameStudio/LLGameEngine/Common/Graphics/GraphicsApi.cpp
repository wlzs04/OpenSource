#include "GraphicsApi.h"

GraphicsApi*  GraphicsApi::graphicsApi = nullptr;

void GraphicsApi::SetSize(float width, float height)
{
	this->width = width;
	this->height = height;
}

void GraphicsApi::SetGraphicsApi(GraphicsApi * currentGraphicsApi)
{
	graphicsApi = currentGraphicsApi;
}

GraphicsApi * GraphicsApi::GetGraphicsApi()
{
	return graphicsApi;
}

void GraphicsApi::ReleaseGraphicsApi()
{
	if (graphicsApi)
	{
		delete graphicsApi;
		graphicsApi = nullptr;
	}
}

GraphicsApi::GraphicsApi(HWND hWnd)
{
	this->hWnd = hWnd;
}
