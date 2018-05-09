#pragma once
#include <string>
#include <vector>
#include <codecvt>
#include "..\LLBase.h"

using namespace std;

class StringEnum : public EnumBase
{
public:
	StringEnum(int i) :EnumBase(i) {}

	static const int Int = 0;//int类型
	static const int Float = 1;//float类型（包括double）
	static const int UnKnown = 2;//其他类型
private:
	unordered_map<wstring, int>& GetEnumMap()override;
};

class WStringHelper
{
public:
	//简单判断传入的字符串的类型
	static StringEnum GetStringEnum(wstring input);
	static int GetInt(wstring& value);
	static float GetFloat(wstring& value);
	static bool GetBool(wstring& value);
	static wstring ToUpper(wstring value);
	static wstring ToLower(wstring value);
	static void Split(wstring ws, wchar_t w, vector<wstring>& v);
	static string WStringToUTF8Buffer(wstring value);
	static wstring UTF8BufferToWString(string value);
	static string WStringToString(wstring value);
	static wstring StringToWString(string value);
private:
	static wstring_convert<codecvt_utf8<wchar_t>> conv;
};