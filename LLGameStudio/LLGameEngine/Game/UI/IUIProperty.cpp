#include "IUIProperty.h"

IUIProperty::IUIProperty(wstring name, wstring defaultValue)
{
	this->name = name;
	this->defaultValue = defaultValue;
}

bool IUIProperty::IsDefault()
{
	return defaultValue == GetValue();
}

unordered_map<wstring, int>& AnchorEnum::GetEnumMap()
{
	static unordered_map<wstring, int> anchorEnumMap;
	if (anchorEnumMap.size() == 0)
	{
		anchorEnumMap[L"Center"] = Center;
		anchorEnumMap[L"Left"] = Left;
		anchorEnumMap[L"Top"] = Top;
		anchorEnumMap[L"Right"] = Right;
		anchorEnumMap[L"Bottom"] = Bottom;
		anchorEnumMap[L"Left_Top"] = Left_Top;
		anchorEnumMap[L"Right_Top"] = Right_Top;
		anchorEnumMap[L"Right_Bottom"] = Right_Bottom;
		anchorEnumMap[L"Left_Bottom"] = Left_Bottom;
	}
	return anchorEnumMap;
}

unordered_map<wstring, int>& CheckStateMethod::GetEnumMap()
{
	static unordered_map<wstring, int> anchorEnumMap;
	if (anchorEnumMap.size() == 0)
	{
		anchorEnumMap[L"Rect"] = Rect; 
		anchorEnumMap[L"Alpha"] = Alpha;
		anchorEnumMap[L"AllowMouseThrough"] = AllowMouseThrough;
	}
	return anchorEnumMap;
}

unordered_map<wstring, int>& ParticleType::GetEnumMap()
{
	static unordered_map<wstring, int> anchorEnumMap;
	if (anchorEnumMap.size() == 0)
	{
		anchorEnumMap[L"Point"] = Point;
		anchorEnumMap[L"Star"] = Star;
		anchorEnumMap[L"Image"] = Image;
		anchorEnumMap[L"Sequence"] = Sequence;
	}
	return anchorEnumMap;
}