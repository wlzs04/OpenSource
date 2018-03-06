#include "SystemHelper.h"

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