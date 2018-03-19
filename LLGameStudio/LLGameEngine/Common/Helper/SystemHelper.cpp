#include "SystemHelper.h"

wstring SystemHelper::resourcePath = L"Resource";

int SystemHelper::GetScreenWidth()
{
	return GetSystemMetrics(SM_CXSCREEN);
}

int SystemHelper::GetScreenHeight()
{
	return GetSystemMetrics(SM_CYSCREEN);
}

wstring SystemHelper::GetCurrentPath()
{
	wchar_t currentPath[MAX_PATH + 1] = { 0 };
	GetModuleFileName(NULL, currentPath, MAX_PATH);
	(wcsrchr(currentPath, L'\\'))[0] = 0;
	return currentPath;
}

wstring SystemHelper::GetResourceRootPath()
{
	return GetCurrentPath() + L"\\" + resourcePath;
}

void SystemHelper::SetCursorPosition(int x, int y)
{
	SetCursorPos(x, y);
}

void SystemHelper::SetCursorCenter()
{
	SetCursorPos(GetScreenWidth()/2, GetScreenHeight() / 2);
}

POINT SystemHelper::GetCursorPosition()
{
	POINT mousePosition;
	GetCursorPos(&mousePosition);
	return mousePosition;
}
