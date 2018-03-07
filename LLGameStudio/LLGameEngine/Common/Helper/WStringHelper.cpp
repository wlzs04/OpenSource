#include "WStringHelper.h"

int WStringHelper::GetInt(wstring value)
{
	return _wtoi(value.c_str());
}

int WStringHelper::GetFloat(wstring value)
{
	return _wtof(value.c_str());
}

int WStringHelper::GetBool(wstring value)
{
	//如果value很长，判断效率会不会比只判断三个false低？还是加上判断吧！
	if (value.size() > 5)
	{
		return true;
	}
	return !(value == L""
		|| value == L"0"
		|| WStringHelper::ToLower(value)== L"false")
		;
}

wstring WStringHelper::ToUpper(wstring value)
{
	for (int i=0;i<value.size();i++)
	{
		value[i] = toupper(value[i]);
	}
	return value;
}

wstring WStringHelper::ToLower(wstring value)
{
	for (int i = 0; i < value.size(); i++)
	{
		value[i] = tolower(value[i]);
	}
	return value;
}
