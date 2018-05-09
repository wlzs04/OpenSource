#include "String.h"

Class * String::GetInstance()
{
	return new String();
}

void String::SetValue(wstring value)
{
	this->value = value;
}

wstring String::GetValueToWString()
{
	return value;
}

wstring String::GetValue()
{
	return value;
}

void String::Add(Class * classptr)
{
	if (classptr->GetName() == L"int"
		|| classptr->GetName() == L"float"
		|| classptr->GetName() == L"string")
	{
		value += classptr->GetValueToWString();
	}
}
