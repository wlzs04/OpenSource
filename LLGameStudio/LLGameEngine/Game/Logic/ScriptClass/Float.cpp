#include "Float.h"
#include "Int.h"

Class * Float::GetInstance()
{
	return new Float();
}

void Float::SetValue(wstring value)
{
	this->value = WStringHelper::GetFloat(value);
}

wstring Float::GetValueToWString()
{
	return to_wstring(value);
}

float Float::GetValue()
{
	return value;
}

void Float::Add(Class * classptr)
{
	if (classptr->GetName() == L"int")
	{
		value = value + ((Int*)classptr)->GetValue();
	}
	else if (classptr->GetName() == L"float")
	{
		value = value + ((Float*)classptr)->GetValue();
	}
}

void Float::Subtract(Class * classptr)
{
	if (classptr->GetName() == L"int")
	{
		value = value - ((Int*)classptr)->GetValue();
	}
	else if (classptr->GetName() == L"float")
	{
		value = value - ((Float*)classptr)->GetValue();
	}
}

void Float::Multiple(Class * classptr)
{
	if (classptr->GetName() == L"int")
	{
		value = value * ((Int*)classptr)->GetValue();
	}
	else if (classptr->GetName() == L"float")
	{
		value = value * ((Float*)classptr)->GetValue();
	}
}

void Float::Divide(Class * classptr)
{
	if (classptr->GetName() == L"int")
	{
		value = value / ((Int*)classptr)->GetValue();
	}
	else if (classptr->GetName() == L"float")
	{
		value = value / ((Float*)classptr)->GetValue();
	}
}
