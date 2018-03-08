#pragma once
#include <string>
#include <vector>

using namespace std;

class WStringHelper
{
public:
	static int GetInt(wstring& value);
	static int GetFloat(wstring& value);
	static int GetBool(wstring& value);
	static wstring ToUpper(wstring value);
	static wstring ToLower(wstring value);
	static void Split(wstring ws, wchar_t w, vector<wstring>& v);
};