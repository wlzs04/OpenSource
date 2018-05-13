#include "Int.h"
#include "Float.h"
#include "String.h"

Class * Int::GetInstance()
{
	Int* i = new Int();
	i->SetValue(value);
	return i;
}

void Int::SetValue(int value)
{
	this->value = value;
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

Parameter Int::Add(Class * classptr)
{
	Parameter p;
	if (classptr->GetName() == L"int")
	{
		p.SetClassName(L"int");
		p.SetValue(to_wstring(value + ((Int*)classptr)->GetValue()));
	}
	else if (classptr->GetName() == L"float")
	{
		p.SetClassName(L"float");
		p.SetValue(to_wstring(value + ((Float*)classptr)->GetValue()));
	}
	else if (classptr->GetName() == L"string")
	{
		p.SetClassName(L"string");
		p.SetValue(to_wstring(value) + ((String*)classptr)->GetValue());
	}
	return p;
}

Parameter Int::Subtract(Class * classptr)
{
	Parameter p;
	if (classptr->GetName() == L"int")
	{
		p.SetClassName(L"int");
		p.SetValue(to_wstring(value - ((Int*)classptr)->GetValue()));
	}
	else if (classptr->GetName() == L"float")
	{
		p.SetClassName(L"float");
		p.SetValue(to_wstring(value - ((Float*)classptr)->GetValue()));
	}
	return p;
}

Parameter Int::Multiple(Class * classptr)
{
	Parameter p;
	if (classptr->GetName() == L"int")
	{
		p.SetClassName(L"int");
		p.SetValue(to_wstring(value * ((Int*)classptr)->GetValue()));
	}
	else if (classptr->GetName() == L"float")
	{
		p.SetClassName(L"float");
		p.SetValue(to_wstring(value * ((Float*)classptr)->GetValue()));
	}
	return p;
}

Parameter Int::Divide(Class * classptr)
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

Parameter Int::Complementation(Class* classptr)
{
	Parameter p(L"int");
	if (classptr->GetName() == L"int")
	{
		p.SetValue(to_wstring(value % ((Int*)classptr)->GetValue()));
	}
	return p;
}

Parameter Int::Greater(Class * classptr)
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

Parameter Int::Less(Class * classptr)
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

Parameter Int::Equal(Class* classptr)
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

Parameter Int::UnEqual(Class * classptr)
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
