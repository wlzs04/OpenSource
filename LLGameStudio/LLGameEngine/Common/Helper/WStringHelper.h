#pragma once
#include <string>
#include <vector>
#include <codecvt>

using namespace std;

class WStringHelper
{
public:
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