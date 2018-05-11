#include "String.h"

Class * String::GetInstance()
{
	String* s = new String();
	s->SetValue(value);
	return s;
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

Parameter String::Add(Class * classptr)
{
	Parameter p(L"string");
	if (classptr->GetName() == L"int"
		|| classptr->GetName() == L"float"
		|| classptr->GetName() == L"string")
	{
		p.SetValue(value + classptr->GetValueToWString());
	}
	return p;
}
