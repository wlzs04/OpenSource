#pragma once
#include "..\Class.h"

class String :public Class
{
public:
	String() :Class(L"string") {}
	Class* GetInstance() override;
	void SetValue(wstring value) override;
	wstring GetValueToWString() override;
	wstring GetValue();
	void Add(Class* classptr)override;
private:
	wstring value;
};