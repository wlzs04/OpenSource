#include "LLBase.h"

bool EnumBase::operator==(const EnumBase & ae) const
{
	return this->value == ae.value;
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
