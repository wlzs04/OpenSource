#include "LLBase.h"

bool EnumBase::operator==(const EnumBase & ae) const
{
	return this->value == ae.value;
}

bool EnumBase::operator==(int i) const
{
	return value == i;
}

int EnumBase::operator&(const EnumBase & ae) const
{
	return value & ae.value;
}

int EnumBase::operator&(int i) const
{
	return value & i;
}

wstring EnumBase::ToWString()
{
	for (auto anchorEnumPair : GetEnumMap())
	{
		if (anchorEnumPair.second == value)
		{
			return anchorEnumPair.first;
		}
	}
	return wstring();
}

void EnumBase::GetValueFromWString(wstring ws)
{
	value = GetEnumMap()[ws];
}

void Color::GetValueFromWString(wstring ws)
{
	int length = ws.size();
	if (length == 9 && ws[0] == L'#')
	{
		int color[8];
		for (int i = 1; i < length; i++)
		{
			color[i - 1] = L'A' <= ws[i] && ws[i] <= L'F' ? ws[i] - L'A' + 10 : ws[i] - L'0';
		}
		a = color[0] + color[1];
		r = color[2] + color[3];
		g = color[4] + color[5];
		b = color[6] + color[7];
	}
	else if (length == 7 && ws[0] == L'#')
	{
		int color[6];
		for (int i = 1; i < length; i++)
		{
			color[i - 1] = L'A' <= ws[i] && ws[i] <= L'F' ? ws[i] - L'A' + 10 : ws[i] - L'0';
		}
		r = color[0] + color[1];
		g = color[2] + color[3];
		b = color[4] + color[5];
	}
}
