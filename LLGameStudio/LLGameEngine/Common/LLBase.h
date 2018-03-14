#pragma once
#include <windows.h>
#include <unordered_map>
#include <string>

using namespace std;

class EnumBase
{
public:
	EnumBase(int i) :value(i) {}
	bool operator==(const EnumBase &ae) const;
	wstring ToWString();
	void GetValueFromWString(wstring ws);
protected:
	virtual unordered_map<wstring, int>& GetEnumMap()=0;
	int value = 0;
};