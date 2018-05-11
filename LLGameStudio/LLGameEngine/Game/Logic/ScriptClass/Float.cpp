#include "Float.h"
#include "Int.h"

Class * Float::GetInstance()
{
	Float* f = new Float();
	f->SetValue(value);
	return f;
}

void Float::SetValue(float value)
{
	this->value = value;
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

Parameter Float::Add(Class * classptr)
{
	Parameter p(L"float");
	if (classptr->GetName() == L"int")
	{
		p.SetValue(to_wstring(value + ((Int*)classptr)->GetValue()));
	}
	else if (classptr->GetName() == L"float")
	{
		p.SetValue(to_wstring(value + ((Float*)classptr)->GetValue()));
	}
	return p;
}

Parameter Float::Subtract(Class * classptr)
{
	Parameter p(L"float");
	if (classptr->GetName() == L"int")
	{
		p.SetValue(to_wstring(value - ((Int*)classptr)->GetValue()));
	}
	else if (classptr->GetName() == L"float")
	{
		p.SetValue(to_wstring(value - ((Float*)classptr)->GetValue()));
	}
	return p;
}

Parameter Float::Multiple(Class * classptr)
{
	Parameter p(L"float");
	if (classptr->GetName() == L"int")
	{
		p.SetValue(to_wstring(value * ((Int*)classptr)->GetValue()));
	}
	else if (classptr->GetName() == L"float")
	{
		p.SetValue(to_wstring(value * ((Float*)classptr)->GetValue()));
	}
	return p;
}

Parameter Float::Divide(Class * classptr)
{
	Parameter p(L"float");
	if (classptr->GetName() == L"int")
	{
		p.SetValue(to_wstring(value / ((Int*)classptr)->GetValue()));
	}
	else if (classptr->GetName() == L"float")
	{
		p.SetValue(to_wstring(value / ((Float*)classptr)->GetValue()));
	}
	return p;
}

Parameter Float::Greater(Class* classptr)
{
	Parameter p(L"bool");
	if (classptr->GetName() == L"int")
	{
		p.SetValue(to_wstring(value > ((Int*)classptr)->GetValue()));
	}
	else if (classptr->GetName() == L"float")
	{
		p.SetValue(to_wstring(value > ((Float*)classptr)->GetValue()));
	}
	return p;
}

Parameter Float::Less(Class* classptr)
{
	Parameter p(L"bool");
	if (classptr->GetName() == L"int")
	{
		p.SetValue(to_wstring(value < ((Int*)classptr)->GetValue()));
	}
	else if (classptr->GetName() == L"float")
	{
		p.SetValue(to_wstring(value < ((Float*)classptr)->GetValue()));
	}
	return p;
}

Parameter Float::Equal(Class* classptr)
{
	Parameter p(L"bool");
	if (classptr->GetName() == L"int")
	{
		p.SetValue(to_wstring(value == ((Int*)classptr)->GetValue()));
	}
	else if (classptr->GetName() == L"float")
	{
		p.SetValue(to_wstring(value == ((Float*)classptr)->GetValue()));
	}
	return p;
}

Parameter Float::UnEqual(Class * classptr)
{
	Parameter p(L"bool");
	if (classptr->GetName() == L"int")
	{
		p.SetValue(to_wstring(value != ((Int*)classptr)->GetValue()));
	}
	else if (classptr->GetName() == L"float")
	{
		p.SetValue(to_wstring(value != ((Float*)classptr)->GetValue()));
	}
	return p;
}
