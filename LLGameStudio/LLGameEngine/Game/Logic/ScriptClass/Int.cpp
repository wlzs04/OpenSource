#include "Int.h"
#include "Float.h"

Class * Int::GetInstance()
{
	return new Int();
}

void Int::SetValue(wstring value)
{
	this->value = WStringHelper::GetInt(value);
}

wstring Int::GetValueToWString()
{
	return to_wstring(value);
}

int Int::GetValue()
{
	return value;
}

void Int::Add(Class * classptr)
{
	if (classptr->GetName() == L"int")
	{
		value += ((Int*)classptr)->GetValue();
	}
	else if (classptr->GetName() == L"float")
	{
		value += ((Float*)classptr)->GetValue();
	}
}

void Int::Subtract(Class * classptr)
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

void Int::Multiple(Class * classptr)
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

void Int::Divide(Class * classptr)
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

void Int::Complementation(Class* classptr)
{
	if (classptr->GetName() == L"int")
	{
		value = value % ((Int*)classptr)->GetValue();
	}
}
