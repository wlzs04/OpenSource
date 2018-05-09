#include "Ptr.h"

Class* Ptr::GetInstance()
{
	return new Ptr();
}

void Ptr::SetValue(wstring value)
{
}

wstring Ptr::GetValueToWString()
{
	return wstring();
}

Class * Ptr::GetValue()
{
	return classPtr;
}
