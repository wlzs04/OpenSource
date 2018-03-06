#include "MessageHelper.h"

void MessageHelper::ShowMessage(wstring message)
{
	MessageBox(NULL, message.c_str(), L"", NULL);
}
