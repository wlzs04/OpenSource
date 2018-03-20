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
	bool operator==(int i) const;
	int operator&(const EnumBase &ae) const;
	int operator&(int i) const;
	wstring ToWString();
	void GetValueFromWString(wstring ws);
	int value = 0;
protected:
	virtual unordered_map<wstring, int>& GetEnumMap()=0;
};