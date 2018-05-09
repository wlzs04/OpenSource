#include "bool.h"

Class * Bool::GetInstance()
{
	return new Bool();
}

void Bool::SetValue(wstring value)
{
	this->value = WStringHelper::GetBool(value);
}

wstring Bool::GetValueToWString()
{
	return to_wstring(value);
}

bool Bool::GetValue()
{
	return value;
}
