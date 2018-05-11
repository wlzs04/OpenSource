#include "bool.h"

Class * Bool::GetInstance()
{
	Bool* b = new Bool();
	b->SetValue(value);
	return b;
}

void Bool::SetValue(bool value)
{
	this->value = value;
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

Parameter Bool::Intersection(Class* classptr)
{
	Parameter p(L"bool");
	if (classptr->GetName() == L"bool")
	{
		p.SetValue(to_wstring(value && ((Bool*)classptr)->GetValue()));
	}
	return p;
}

Parameter Bool::Union(Class* classptr)
{
	Parameter p(L"bool");
	if (classptr->GetName() == L"bool")
	{
		p.SetValue(to_wstring(value || ((Bool*)classptr)->GetValue()));
	}
	return p;
}
