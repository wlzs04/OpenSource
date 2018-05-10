#include "bool.h"

Class * Bool::GetInstance()
{
	return new Bool();
}

void Bool::SetValue(wstring value)
{
	this->value = value!=L"0" && value != L"false";
}

wstring Bool::GetValueToWString()
{
	return to_wstring(value);
}

bool Bool::GetValue()
{
	return value;
}

void Bool::Intersection(Class* classptr)
{
	if (classptr->GetName() == L"bool")
	{
		value = value && ((Bool*)classptr)->GetValue();
	}
}

void Bool::Union(Class* classptr)
{
	if (classptr->GetName() == L"bool")
	{
		value = value || ((Bool*)classptr)->GetValue();
	}
}
